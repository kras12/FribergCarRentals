using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Data.Repositories;
using FribergFastigheter.Server.Data.Entities;
using FribergFastigheter.Shared.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MvcRazorPages.Shared.Helpers;

namespace FribergCarRentals.Shared.Services
{
    /// <summary>
    /// Service for seeding mock data to the database. 
    /// </summary>
    public class MockDataSeeder : IMockDataSeeder
    {
        #region Constants

        /// <summary>
        /// The configuration key for the car images folder name.
        /// </summary>
        private const string CarImagesFolderNameConfigEntryKey = "MockData:CarImagesFolderName";

        /// <summary>
        /// The configuration key for the default user password
        /// </summary>
        private const string DefaultUserPasswordConfigEntryKey = "MockData:DefaultUserPassword";

        /// <summary>
        /// The configuration key for the mock data folder name.
        /// </summary>
        private const string MockDataFolderNameConfigEntryKey = "MockData:FolderName";
        #endregion

        #region Fields

        /// <summary>
        /// The injected admin repository.
        /// </summary>
        private readonly IAdminRepository _adminRepository;

        /// <summary>
        /// The injected car category repository.
        /// </summary>
        private readonly ICarCategoryRepository _carCategoryRepository;

        /// <summary>
        /// The injected car repository.
        /// </summary>
        private readonly ICarRepository _carRepository;

        /// <summary>
        /// The injected application configuration.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// The injected customer repository.
        /// </summary>
        private readonly ICustomerRepository _customerRepository;

        /// <summary>
        /// The injected user manager.
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="adminRepository">The injected admin repository.</param>
        /// <param name="carCategoryRepository">The injected car category repository.</param>
        /// <param name="carRepository">The injected car repository.</param>
        /// <param name="customerRepository">The injected customer repository.</param>
        /// <param name="userManager">The injected user manager.</param>
        /// <param name="configuration">The injected application configuration.</param>
        public MockDataSeeder(IAdminRepository adminRepository, ICarCategoryRepository carCategoryRepository, ICarRepository carRepository,
            ICustomerRepository customerRepository, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _adminRepository = adminRepository;
            _carCategoryRepository = carCategoryRepository;
            _carRepository = carRepository;
            _customerRepository = customerRepository;
            _userManager = userManager;
            _configuration = configuration;
        }

        #endregion

        #region MockDataMethods

        /// <summary>
        /// Returns the default admin users.
        /// </summary>
        /// <returns>A collection of <see cref="ApplicationUser"/>.</returns>
        public List<ApplicationUser> GetDefaultAdminUsers()
        {
            List<ApplicationUser> users = new()
            {
                new ApplicationUser("Adam", "Friberg", "admin@rental.com", "admin@rental.com", "070-123456789", emailConfirmed: true)
            };

            return users;
        }

        /// <summary>
        /// Returns the default cars and car categories.
        /// </summary>
        /// <returns>A tuple containing collections of <see cref="CarEntity"/> and <see cref="CarCategoryEntity"/>.</returns>
        public (List<CarCategoryEntity> CarCategories, List<CarEntity> Cars) GetDefaultCarsAndCategories()
        {
            List<CarCategoryEntity> carCategories = new()
            {
                new CarCategoryEntity("Sedan"),
                new CarCategoryEntity("SUV"),
                new CarCategoryEntity("Truck"),
            };

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

            return new(carCategories, cars);
        }

        /// <summary>
        /// Returns the default customer users.
        /// </summary>
        /// <returns>A collection of <see cref="ApplicationUser"/>.</returns>
        public List<ApplicationUser> GetDefaultCustomerUsers()
        {
            List<ApplicationUser> users = new()
            {
                new ApplicationUser("Kalle", "Anka", "kalle@ankeborg.com", "kalle@ankeborg.com", "070-123456789", emailConfirmed: true),
                new ApplicationUser("Kajsa", "Anka", "kajsa@ankeborg.com", "kajsa@ankeborg.com", "070-123456789", emailConfirmed: true),
            };

            return users;
        }

        #endregion

        #region SeedMethods

        /// <summary>
        /// Seeds admins into the database.
        /// </summary>
        /// <param name="adminUsers">A collection of application users for the admins to be created.</param>
        /// <param name="overridePassword">The password to override the default password for the new admins.</param>
        /// <returns>A <see cref="Task>"/> representing an asynchronous operation.</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task SeedAdmins(List<ApplicationUser> adminUsers, string? overridePassword = null)
        {
            #region Checks

            if (adminUsers == null || adminUsers.Count == 0)
            {
                throw new ArgumentException("The list of admin users can't be empty", nameof(adminUsers));
            }

            if (overridePassword != null && overridePassword == "")
            {
                throw new ArgumentException("The password can't be an empty string.", nameof(overridePassword));
            }

            #endregion

            if (await _adminRepository.AnyAsync() == false)
            {
                string password = overridePassword != null ? overridePassword : _configuration[DefaultUserPasswordConfigEntryKey]!;

                foreach (var user in adminUsers)
                {
                    var createUserResult = await _userManager.CreateAsync(user, password);
                    IdentityResult? addRoleResult = null;

                    if (createUserResult.Succeeded)
                    {
                        addRoleResult = await _userManager.AddToRoleAsync(user, ApplicationUserRoles.Admin);

                        if (addRoleResult.Succeeded)
                        {
                            var admin = new AdminEntity(user!);
                            await _adminRepository.AddAsync(admin);

                            return;
                        }
                    }

                    throw new Exception("Failed to seed admins");
                }               
            }
        }

        /// <summary>
        /// Seeds cars and car categories into the database.
        /// </summary>
        /// <param name="input">A tuple containing collections of <see cref="CarEntity"/> and <see cref="CarCategoryEntity"/>.</param>
        /// <param name="overrideDefaultMockDataImageFolderPath">The folder path to override the default path for the folder where the mock car images can be found.</param>
        /// <returns>A <see cref="Task>"/> representing an asynchronous operation.</returns>
        /// <exception cref="ArgumentException"></exception>
        public Task SeedCarsAndCategories((List<CarCategoryEntity> CarCategories, List<CarEntity> Cars) input, string? overrideDefaultMockDataImageFolderPath = null)
        {
            return SeedCarsAndCategories(input.CarCategories, input.Cars, overrideDefaultMockDataImageFolderPath);
        }

        /// <summary>
        /// Seeds cars and car categories into the database.
        /// </summary>
        /// <param name="carCategories">A collection of car categories to add.</param>
        /// <param name="cars">A collection of cars to add.</param>
        /// <param name="overrideDefaultMockDataImageFolderPath">The folder path to override the default path for the folder where the mock car images can be found.</param>
        /// <returns>A <see cref="Task>"/> representing an asynchronous operation.</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task SeedCarsAndCategories(List<CarCategoryEntity> carCategories, List<CarEntity> cars, string? overrideDefaultMockDataImageFolderPath = null)
        {
            #region Checks

            if (carCategories == null || carCategories.Count == 0)
            {
                throw new ArgumentException("The list of car categories can't be empty", nameof(carCategories));
            }

            if (cars == null || cars.Count == 0)
            {
                throw new ArgumentException("The list of cars can't be empty", nameof(cars));
            }

            if (overrideDefaultMockDataImageFolderPath != null && overrideDefaultMockDataImageFolderPath == "")
            {
                throw new ArgumentException("The folder path for the mock car images can't be an empty string.", nameof(overrideDefaultMockDataImageFolderPath));
            }

            #endregion

            if (await _carCategoryRepository.AnyAsync() == false && await _carRepository.AnyAsync() == false)
            {
                //=====================================================
                // Setup
                //=====================================================
                string mockDataImageFolderPath = overrideDefaultMockDataImageFolderPath != null ? overrideDefaultMockDataImageFolderPath : 
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, 
                        _configuration[MockDataFolderNameConfigEntryKey]!, 
                        _configuration[CarImagesFolderNameConfigEntryKey]!);

                //=====================================================
                // Cleanup
                //=====================================================
                ImageHelper.ClearAllImagesFromDisk();

                //=====================================================
                // Categories
                //=====================================================
                foreach (var carCategory in carCategories)
                {
                    await _carCategoryRepository.AddAsync(carCategory);
                }

                //=====================================================
                // Cars
                //=====================================================
                foreach (var car in cars)
                {
                    List<IFormFile> formFiles = new();

                    foreach (var image in car.Images)
                    {
                        FileStream fileStream = File.OpenRead(Path.Combine(mockDataImageFolderPath, image.FileName));
                        formFiles.Add(new FormFile(fileStream, 0, fileStream.Length, "FormFile", image.FileName));
                    }

                    if (formFiles.Count > 0)
                    {
                        var imageNames = await ImageHelper.SaveUploadedImagesToDisk(formFiles);
                        car.Images = imageNames.Select(x => new ImageEntity(x)).ToList();
                    }

                    await _carRepository.AddAsync(car);
                }
            }
        }

        /// <summary>
        /// Seeds customers into the database.
        /// </summary>
        /// <param name="customerUsers">A collection of application users for the customers to be created.</param>
        /// <param name="overridePassword">The password to override the default password for the new customers.</param>
        /// <returns>A <see cref="Task>"/> representing an asynchronous operation.</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task SeedCustomers(List<ApplicationUser> customerUsers, string? overridePassword = null)
        {
            #region Checks

            if (customerUsers == null || customerUsers.Count == 0)
            {
                throw new ArgumentException("The list of customer users can't be empty", nameof(customerUsers));
            }

            if (overridePassword != null && overridePassword == "")
            {
                throw new ArgumentException("The password can't be an empty string.", nameof(overridePassword));
            }

            #endregion

            if (await _customerRepository.AnyAsync() == false)
            {
                string password = overridePassword != null ? overridePassword : _configuration[DefaultUserPasswordConfigEntryKey]!;

                foreach (var user in customerUsers)
                {
                    var createUserResult = await _userManager.CreateAsync(user, password);
                    IdentityResult? addRoleResult = null;

                    if (createUserResult.Succeeded)
                    {
                        addRoleResult = await _userManager.AddToRoleAsync(user, ApplicationUserRoles.Customer);

                        if (addRoleResult.Succeeded)
                        {
                            var admin = new CustomerEntity(user!);
                            await _customerRepository.AddAsync(admin);

                            return;
                        }
                    }
                }
            }
        }

        #endregion
    }
}
