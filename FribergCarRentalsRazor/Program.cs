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
using MvcRazorPages.Shared.Helpers;

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

                var adminRepository = services.GetRequiredService<IAdminRepository>();
                var customerRepository = services.GetRequiredService<ICustomerRepository>();
                var carCategoryRepository = services.GetRequiredService<ICarCategoryRepository>();
                var carRepository = services.GetRequiredService<ICarRepository>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

                SeedAdmins(userManager, adminRepository).Wait();
                SeedCustomers(userManager, customerRepository).Wait();
                SeedCars(carCategoryRepository, carRepository).Wait();
            }

            app.Run();
        }

        private static async Task SeedAdmins(UserManager<ApplicationUser> userManager, IAdminRepository adminRepository)
        {
            if (await adminRepository.AnyAsync() == false)
            {
                var user = new ApplicationUser("Adam", "Friberg", "admin@rental.com", "admin@rental.com", "070-123456789", emailConfirmed: true);
                var createUserResult = await userManager.CreateAsync(user, "Aa1!123456789");
                IdentityResult? addRoleResult = null;

                if (createUserResult.Succeeded)
                {
                    addRoleResult = await userManager.AddToRoleAsync(user, ApplicationUserRoles.Admin);

                    if (addRoleResult.Succeeded)
                    {
                        var admin = new AdminEntity(user!);
                        await adminRepository.AddAsync(admin);

                        return;
                    }
                }

                throw new Exception("Failed to seed admins");
            }
        }

        private static async Task SeedCustomers(UserManager<ApplicationUser> userManager, ICustomerRepository customerRepository)
        {
            if (await customerRepository.AnyAsync() == false)
            {
                List<ApplicationUser> users = new()
            {
                new ApplicationUser("Kalle", "Anka", "kalle@ankeborg.com", "kalle@ankeborg.com", "070-123456789", emailConfirmed: true),
                new ApplicationUser("Kajsa", "Anka", "kajsa@ankeborg.com", "kajsa@ankeborg.com", "070-123456789", emailConfirmed: true),
            };

                foreach (var user in users)
                {
                    var createUserResult = await userManager.CreateAsync(user, "Aa1!123456789");
                    IdentityResult? addRoleResult = null;

                    if (createUserResult.Succeeded)
                    {
                        addRoleResult = await userManager.AddToRoleAsync(user, ApplicationUserRoles.Customer);

                        if (addRoleResult.Succeeded)
                        {
                            var admin = new CustomerEntity(user!);
                            await customerRepository.AddAsync(admin);

                            return;
                        }
                    }
                }
            }
        }

        private static async Task SeedCars(ICarCategoryRepository carCategoryRepository, ICarRepository carRepository)
        {
            if (await carCategoryRepository.AnyAsync() == false && await carRepository.AnyAsync() == false)
            {
                //=====================================================
                // Cleanup
                //=====================================================
                ImageHelper.ClearAllImagesFromDisk();

                //=====================================================
                // Categories
                //=====================================================
                List<CarCategoryEntity> carCategories = new()
                {
                    new CarCategoryEntity("Sedan"),
                    new CarCategoryEntity("SUV"),
                    new CarCategoryEntity("Truck"),
                };

                foreach (var carCategory in carCategories)
                {
                    await carCategoryRepository.AddAsync(carCategory);
                }

                //=====================================================
                // Cars
                //=====================================================
                List<CarEntity> cars = new()
                {
                    new CarEntity(carCategories.Single(x => x.CategoryName == "Sedan"), "Tesla", "Black", "Model S", 2024, "TOP666",
                    VehiclePropulsionEntity.CreateFromType(Data.Types.VehiclePropulsionType.BEV), CarRentalStatusEntity.CreateFromType(Data.Types.RentalCarStatus.Idle), 3500)
                    {
                        Images = new ()
                        {
                            new ImageEntity("tesla-model-s-1.webp"),
                            new ImageEntity("tesla-model-s-2.webp"),
                        }
                    },

                    new CarEntity(carCategories.Single(x => x.CategoryName == "Sedan"), "Tesla", "Red", "Model 3", 2024, "MIN123",
                    VehiclePropulsionEntity.CreateFromType(Data.Types.VehiclePropulsionType.BEV), CarRentalStatusEntity.CreateFromType(Data.Types.RentalCarStatus.Idle), 2500)
                    {
                        Images = new ()
                        {
                            new ImageEntity("tesla-model-3-1.webp"),
                            new ImageEntity("tesla-model-3-2.webp"),
                        }
                    },

                    new CarEntity(carCategories.Single(x => x.CategoryName == "SUV"), "Tesla", "Gray", "Model X", 2024, "MAX999",
                    VehiclePropulsionEntity.CreateFromType(Data.Types.VehiclePropulsionType.BEV), CarRentalStatusEntity.CreateFromType(Data.Types.RentalCarStatus.Idle), 3500)
                    {
                        Images = new ()
                        {
                            new ImageEntity("tesla-model-x-1.webp"),
                            new ImageEntity("tesla-model-x-2.webp"),
                        }
                    },

                    new CarEntity(carCategories.Single(x => x.CategoryName == "Sedan"), "Tesla", "Blue", "Model Y", 2024, "MID456",
                    VehiclePropulsionEntity.CreateFromType(Data.Types.VehiclePropulsionType.BEV), CarRentalStatusEntity.CreateFromType(Data.Types.RentalCarStatus.Idle), 2500)
                    {
                        Images = new ()
                        {
                            new ImageEntity("tesla-model-Y-1.webp"),
                            new ImageEntity("tesla-model-y-2.webp"),
                        }
                    },
                };

                string imageFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MockData", "CarImages");

                foreach (var car in cars)
                {
                    List<IFormFile> formFiles = new();

                    foreach (var image in car.Images)
                    {
                        FileStream fileStream = File.OpenRead(Path.Combine(imageFolder, image.FileName));
                        formFiles.Add(new FormFile(fileStream, 0, fileStream.Length, "FormFile", image.FileName));
                    }

                    if (formFiles.Count > 0)
                    {
                        var imageNames = await ImageHelper.SaveUploadedImagesToDisk(formFiles);
                        car.Images = imageNames.Select(x => new ImageEntity(x)).ToList();
                    }

                    await carRepository.AddAsync(car);
                }
            }
        }
    }
}
