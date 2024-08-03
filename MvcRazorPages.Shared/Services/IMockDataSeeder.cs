using FribergCarRentals.Data.EntityClasses;

namespace MvcRazorPages.Shared.Services
{
    /// <summary>
    /// Interface for a service that seeds data to the database. 
    /// </summary>
    public interface IMockDataSeeder
    {
        #region MockDataMethods

        /// <summary>
        /// Returns the default admins.
        /// </summary>
        /// <returns>A collection of <see cref="AdminEntity"/>.</returns>
        public List<AdminEntity> GetDefaultAdmins();

        /// <summary>
        /// Returns the default cars and car categories.
        /// </summary>
        /// <returns>A tuple containing collections of <see cref="CarEntity"/> and <see cref="CarCategoryEntity"/>.</returns>
        public (List<CarCategoryEntity> CarCategories, List<CarEntity> Cars) GetDefaultCarsAndCategories();

        /// <summary>
        /// Returns the customers.
        /// </summary>
        /// <returns>A collection of <see cref="CustomerEntity"/>.</returns>
        public List<CustomerEntity> GetDefaultCustomers();

        #endregion

        #region SeedMethods

        /// <summary>
        /// Seeds admins into the database.
        /// </summary>
        /// <param name="admins">A collection of admins to be created.</param>
        /// <param name="overridePassword">The password to override the default password for the new admins.</param>
        /// <returns>A <see cref="Task>"/> representing an asynchronous operation.</returns>
        /// <exception cref="ArgumentException"></exception>
        public Task SeedAdmins(List<AdminEntity> admins, string? overridePassword = null);

        /// <summary>
        /// Seeds cars and car categories into the database.
        /// </summary>
        /// <param name="input">A tuple containing collections of <see cref="CarEntity"/> and <see cref="CarCategoryEntity"/>.</param>
        /// <param name="overrideDefaultMockDataImageFolderPath">The folder path to override the default path for the folder where the mock car images can be found.</param>
        /// <returns>A <see cref="Task>"/> representing an asynchronous operation.</returns>
        /// <exception cref="ArgumentException"></exception>
        public Task SeedCarsAndCategories((List<CarCategoryEntity> CarCategories, List<CarEntity> Cars) input, string? overrideDefaultMockDataImageFolderPath = null);

        /// <summary>
        /// Seeds cars and car categories into the database.
        /// </summary>
        /// <param name="carCategories">A collection of car categories to add.</param>
        /// <param name="cars">A collection of cars to add.</param>
        /// <param name="overrideDefaultMockDataImageFolderPath">The folder path to override the default path for the folder where the mock car images can be found.</param>
        /// <returns>A <see cref="Task>"/> representing an asynchronous operation.</returns>
        /// <exception cref="ArgumentException"></exception>
        public Task SeedCarsAndCategories(List<CarCategoryEntity> carCategories, List<CarEntity> cars, string? overrideDefaultMockDataImageFolderPath = null);

        /// <summary>
        /// Seeds customers into the database.
        /// </summary>
        /// <param name="customers">A collection of new customers to be created.</param>
        /// <param name="overridePassword">The password to override the default password for the new customers.</param>
        /// <returns>A <see cref="Task>"/> representing an asynchronous operation.</returns>
        /// <exception cref="ArgumentException"></exception>
        public Task SeedCustomers(List<CustomerEntity> customers, string? overridePassword = null);

        #endregion
    }
}