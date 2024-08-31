
using AppSettings.Shared.Settings;
using FribergCarRentals.Data.DatabaseContexts;
using FribergCarRentals.Data.Repositories;
using FribergCarRentals.Shared.Constants;
using FribergCarRentalsApi.Filters;
using FribergCarRentalsApi.Services;
using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using FribergCarRentalsApi.AutoMapper;
using FribergCarRentals.Shared.Mvc.Services;

namespace FribergCarRentalsApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ==================================================================================================================
            // DB Context
            // ==================================================================================================================
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString(AppSettingsHelper.ApplicationDbContextConnectionStringKey)));

            // ==================================================================================================================
            // Documentation
            // ==================================================================================================================

            // Swagger
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Friberg Car Rentals API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            // ==================================================================================================================
            // Images
            // ==================================================================================================================
            builder.Services.AddScoped<IImageUploadService, ImageUploadService>();
            builder.Services.AddScoped<IImageApiDownloadService, ImageApiDownloadService>();

            // ==================================================================================================================
            // Mapping
            // ==================================================================================================================

            // Auto Mapper
            builder.Services.AddAutoMapper(typeof(EntityToDtoAutoMapperProfile));
            builder.Services.AddAutoMapper(typeof(DtoToEntityAutoMapperProfile));
            builder.Services.AddAutoMapper(typeof(DtoToDtoAutoMapperProfile));

            // ==================================================================================================================
            // Network (converters, cors, data transfers, filters)
            // ==================================================================================================================

            // Add serialization converters
            builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            // Cors policy
#if DEBUG
            string corsPolicyName = "LocalHostingCorsPolicy";

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(corsPolicyName, builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
#else
            string corsPolicyName = "ProductionCorsPolicy";

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(corsPolicyName, builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
#endif

            /// Reformats validation problems details from bad requests into an ApiErrorResponseDto object.
            builder.Services.AddControllers(options => options.Filters.Add(typeof(ReformatValidationProblemAttribute)));

            // ==================================================================================================================
            //  Repositories
            // ==================================================================================================================
            builder.Services.AddTransient<ICarRepository, CarRepository>();
            builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
            builder.Services.AddTransient<ICarOrderRepository, CarOrderRepository>();
            builder.Services.AddTransient<IAdminRepository, AdminRepository>();
            builder.Services.AddTransient<ICarCategoryRepository, CarCategoryRepository>();

            // ==================================================================================================================
            // Security (authentication, authorization, identity)
            // ==================================================================================================================

            // Identity Services
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 10;
                options.User.AllowedUserNameCharacters += "едц";

            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                options.DefaultChallengeScheme =
                options.DefaultForbidScheme =
                options.DefaultScheme =
                options.DefaultSignInScheme =
                options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]!)),
                    RoleClaimType = ApplicationUserJwtClaims.UserRole
                };
            });

            builder.Services.AddAuthorizationCore(options =>
            {
                options.AddPolicy(ApplicationUserPolicies.Admin, policy =>
                    policy.RequireClaim(ApplicationUserClaims.UserRole, ApplicationUserRoles.Admin));

                options.AddPolicy(ApplicationUserPolicies.Customer, policy =>
                   policy.RequireClaim(ApplicationUserClaims.UserRole, ApplicationUserRoles.Customer));
            });

            // Token service
            builder.Services.AddTransient<ITokenService, TokenService>();

            // ==================================================================================================================
            //  Seeding
            // ==================================================================================================================
            builder.Services.AddTransient<IMockDataSeeder, MockDataSeeder>();

            var app = builder.Build();

            app.UseCors(corsPolicyName);

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            // ==================================================================================================================
            // Migration
            // ==================================================================================================================

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();

                var mockDataSeeder = services.GetRequiredService<IMockDataSeeder>();
                mockDataSeeder.SeedAdmins(mockDataSeeder.GetDefaultAdmins()).Wait();
                mockDataSeeder.SeedCustomers(mockDataSeeder.GetDefaultCustomers()).Wait();
                mockDataSeeder.SeedCarsAndCategories(mockDataSeeder.GetDefaultCarsAndCategories()).Wait();
            }

            app.Run();
        }
    }
}
