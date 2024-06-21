using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.DataAccess.DatabaseContexts;
using FribergCarRentals.DataAccess.Types;
using static System.Net.Mime.MediaTypeNames;
using System.Threading.Channels;
using System.Runtime.ConstrainedExecution;
using FribergCarRentals.DataAccess.DataTransferObjects;
using Microsoft.Identity.Client;

namespace FribergCarRentals.DataAccess.Repositories
{
    /// <summary>
    /// A repository class that handles the car category entity.
    /// </summary>
    /// <remarks>This repository class works on detached entities. All fetched entities will not be tracked by EF Core.</remarks>
    public class CarCategoryRepository : GenericRepository<CarCategoryEntity>, ICarCategoryRepository
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="databaseContext">The database context to use.</param>
        public CarCategoryRepository(ApplicationDbContext databaseContext) : base(databaseContext)
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a car category from the database.
        /// </summary>
        /// <param name="id">The ID of the car category to delete.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public Task DeleteAsync(int id)
        {
            var category = new CarCategoryEntity() { CarCategoryId = id };
            _databaseContext.CarCategories.Remove(category);
            return _databaseContext.SaveChangesAsync();
        }

        /// <summary>
        /// Attempts to fetch a car category with a specific ID.
        /// </summary>
        /// <remarks>Returned entities will not be tracked by EF Core.</remarks>
        /// <param name="id">The ID of the car category.</param>
        /// <remarks>This override is needed to disable tracking since the base class lacks this ability.</remarks>
        /// <returns>a <see cref="Task{TResult}"/> containing the car if found, or null if not found.</returns>
        public override Task<CarCategoryEntity?> GetByIdAsync(int id)
        {
            return _databaseContext.CarCategories.AsNoTracking().SingleOrDefaultAsync(x => x.CarCategoryId == id);
        }

        /// <summary>
        /// Returns the statistics for all car categories.
        /// </summary>
        /// <returns>A <see cref="Task"/> containing a collection of category statistics.</returns>
        public async Task<List<CarCategoryStatisticsDto>> GetCategoryStatistics()
        {
            return await _databaseContext.CarCategories.AsNoTracking()
               .GroupJoin(
                   _databaseContext.Cars.AsNoTracking(),
                   category => category.CarCategoryId,
                   car => car.Category!.CarCategoryId,
                   (categoryEntity, carList) => new
                   {
                       category = categoryEntity,
                       carCount = carList.Count()
                   })
               .Select(categoryGroup => new CarCategoryStatisticsDto(categoryGroup.category, categoryGroup.carCount))
               .ToListAsync();
        }

        /// <summary>
        /// Returns the statistics for the category that matches the ID. 
        /// </summary>
        /// <param name="id">The ID of the category to fetch statistics for.</param>
        /// <returns>A <see cref="Task"/> containing the statistics for the category.</returns>
        public async Task<CarCategoryStatisticsDto> GetCategoryStatisticsById(int id)
        {
            return await _databaseContext.CarCategories.AsNoTracking().Where(x => x.CarCategoryId == id)
                .GroupJoin(
                    _databaseContext.Cars.AsNoTracking(),
                    category => category.CarCategoryId,
                    car => car.Category!.CarCategoryId,
                    (categoryEntity, carList) => new
                    {
                        category = categoryEntity,
                        carCount = carList.Count()
                    })
                .Select(categoryGroup => new CarCategoryStatisticsDto(categoryGroup.category, categoryGroup.carCount))
                .FirstAsync();
        }

        #endregion

    }
}
