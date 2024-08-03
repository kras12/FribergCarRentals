using FribergCarRentals.Data.DTO;
using FribergCarRentals.Data.EntityClasses;

namespace FribergCarRentals.Data.Repositories
{
    /// <summary>
    /// An interface for a car category repository.
    /// </summary>
    public interface ICarCategoryRepository : IGenericRepository<CarCategoryEntity>
    {
        #region Methods

        /// <summary>
        /// Checks the existence of a car category.
        /// </summary>
        /// <param name="carCategoryId">The ID of the category to look for.</param>
        /// <returns>A <see cref="Task"/> containing true if the category exists.</returns>
        public Task<bool> CategoryExists(int carCategoryId);

        /// <summary>
        /// Deletes a car category from the database.
        /// </summary>
        /// <param name="id">The ID of the car category to delete.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public Task DeleteAsync(int id);

        /// <summary>
        /// Returns the statistics for all car categories.
        /// </summary>
        /// <returns>A <see cref="Task"/> containing a collection of category statistics.</returns>
        public Task<List<CarCategoryCountDto>> GetCategoryStatistics();

        /// <summary>
        /// Returns the statistics for the category that matches the ID. 
        /// </summary>
        /// <param name="id">The ID of the category to fetch statistics for.</param>
        /// <returns>A <see cref="Task"/> containing the statistics for the category.</returns>
        public Task<CarCategoryCountDto> GetCategoryStatisticsById(int id);

        #endregion
    }
}
