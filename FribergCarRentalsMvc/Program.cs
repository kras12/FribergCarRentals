using FribergCarRentals.Data.DatabaseContexts;
using FribergCarRentals.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using AppSettings.Shared.Settings;
using MvcRazorPages.Shared.ModelBinders;
using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Identity;
using MvcRazorPages.Shared.Services;
using FribergCarRentals.Shared.Constants;
using FribergCarRentals.Shared.Mapping.AutoMapper;
using FribergCarRentals.Shared.Mapping.Mvc.AutoMapper;

namespace FribergCarRentals
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ==================================================================================================================
            // Controllers
            // ==================================================================================================================
            builder.Services.AddControllersWithViews(options => options.ModelBinderProviders.Insert(0, new CustomModelBinderProvider()));

            // ==================================================================================================================
            // DB Context
            // ==================================================================================================================
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString(AppSettingsHelper.ApplicationDbContextConnectionStringKey)));

            // ==================================================================================================================
            // Images
            // ==================================================================================================================
            builder.Services.AddScoped<IImageUploadService, ImageUploadService>();
            builder.Services.AddScoped<IImageDownloadService, ImageDownloadService>();

            // ==================================================================================================================
            // Mapping
            // ==================================================================================================================
            builder.Services.AddAutoMapper(
                typeof(Shared.Mapping.AutoMapper.ViewModelToEntityAutoMapperProfile), 
                typeof(Shared.Mapping.Mvc.AutoMapper.ViewModelToEntityAutoMapperProfile),                 
                typeof(EntityToViewModelAutoMapperProfile), typeof(DtoToViewModelAutoMapperProfile));

            // ==================================================================================================================
            //  Repositories
            // ==================================================================================================================
            builder.Services.AddTransient<ICarRepository, CarRepository>();
            builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
            builder.Services.AddTransient<ICarOrderRepository, CarOrderRepository>();
            builder.Services.AddTransient<IAdminRepository, AdminRepository>();
            builder.Services.AddTransient<ICarCategoryRepository, CarCategoryRepository>();

            // ==================================================================================================================
            //  Seeding
            // ==================================================================================================================
            builder.Services.AddTransient<IMockDataSeeder, MockDataSeeder>();

            // ==================================================================================================================
            // Security (authentication, authorization)
            // ==================================================================================================================

            // Identity
            builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 10;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

            // Authorization
            builder.Services.AddAuthorizationCore(options =>
            {
                options.AddPolicy(ApplicationUserPolicies.Admin, policy =>
                    policy.RequireClaim(ApplicationUserClaims.UserRole, ApplicationUserRoles.Admin));

                options.AddPolicy(ApplicationUserPolicies.Customer, policy =>
                   policy.RequireClaim(ApplicationUserClaims.UserRole, ApplicationUserRoles.Customer));
            });

            // ==================================================================================================================
            // Build
            // ==================================================================================================================

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

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
