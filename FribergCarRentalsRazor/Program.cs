using FribergCarRentals.Data.DatabaseContexts;
using FribergCarRentals.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using AppSettings.Shared.Settings;
using MvcRazorPages.Shared.ModelBinders;
using FribergFastigheter.Server.AutoMapper;
using FribergFastigheter.Server.Data.Entities;
using FribergFastigheter.Shared.Constants;
using Microsoft.AspNetCore.Identity;
using FribergCarRentals.Data.EntityClasses;

namespace FribergCarRentals
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Pages with model binders
            builder.Services.AddRazorPages().AddMvcOptions(options => options.ModelBinderProviders.Insert(0, new CustomModelBinderProvider()));

            // DB Context
            builder.Services.AddDbContext<ApplicationDbContext>(options => 
                options.UseSqlServer(builder.Configuration.GetConnectionString(AppSettingsHelper.ApplicationDbContextConnectionStringKey)));

            // ==================================================================================================================
            // Mapping
            // ==================================================================================================================
            builder.Services.AddAutoMapper(typeof(ViewModelToEntityAutoMapperProfile));

            // ==================================================================================================================
            //  Repositories
            // ==================================================================================================================
            builder.Services.AddTransient<ICarRepository, CarRepository>();
            builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
            builder.Services.AddTransient<ICarOrderRepository, CarOrderRepository>();
            builder.Services.AddTransient<IAdminRepository, AdminRepository>();
            builder.Services.AddTransient<ICarCategoryRepository, CarCategoryRepository>();

            // ==================================================================================================================
            //  Sessions
            // ==================================================================================================================
            // TODO - Check if safe to remove
            //builder.Services.AddDistributedMemoryCache();
            //builder.Services.AddSession(options =>
            //{
            //    options.IdleTimeout = TimeSpan.FromMinutes(15);
            //    options.Cookie.HttpOnly = true;
            //    options.Cookie.IsEssential = true;
            //});            

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

            //app.UseSession();

            app.MapRazorPages();

            // ==================================================================================================================
            // Migration
            // ==================================================================================================================

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();

                var customerRepository = services.GetRequiredService<ICustomerRepository>();

                if (!customerRepository.AnyAsync().Result)
                {
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    SeedAdmins(userManager, customerRepository).Wait();
                }
            }

            app.Run();
        }

        private static async Task SeedAdmins(UserManager<ApplicationUser> userManager, ICustomerRepository customerRepository)
        {
            var user = new ApplicationUser("Adam", "Friberg", "admin@rental.com", "admin@rental.com", "070-123456789", emailConfirmed: true);
            var createUserResult = await userManager.CreateAsync(user, "Aa1!123456789");
            IdentityResult? addRoleResult = null;

            if (createUserResult.Succeeded)
            {
                addRoleResult = await userManager.AddToRoleAsync(user, ApplicationUserRoles.Customer);

                if (addRoleResult.Succeeded)
                {
                    var userId = await userManager.GetUserIdAsync(user);
                    var customer = new CustomerEntity(user!);
                    await customerRepository.AddAsync(customer);
                    return;
                }
            }

            throw new Exception("Failed to seed admins");
        }
    }
}
