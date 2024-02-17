using FribergCarRentals.DataAccess.EntityClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentals.DataAccess.Repositories
{
    /// <summary>
    /// An interface for a car repository.
    /// </summary>
    public interface ICarRepository : IGenericRepository<CarEntity>
    {
        #region Methods

        /// <summary>
        /// Deletes a car from the database.
        /// </summary>
        /// <param name="id">The ID of the car to delete.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public Task DeleteAsync(int id);

        /// <summary>
        /// Attempts to fetch all cars with a specific rental status.
        /// </summary>
        /// <remarks>Returned entities will not be tracked by EF Core.</remarks>
        /// <param name="rentalStatus">The rental status of the cars.</param>
        /// <returns>A <see cref="IEnumerable{T}"/> that contains matched cars.</returns>
        public Task<IEnumerable<CarEntity>> GetAllAsync(CarRentalStatusEntity rentalStatus);

        /// <summary>
        /// Attempts to fetch a car with a specific ID and rental status.
        /// </summary>
        /// <remarks>Returned entities will not be tracked by EF Core.</remarks>
        /// <param name="id">The ID of the car.</param>
        /// <param name="rentalStatus">The rental status of the car.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the car entity if found, or null if not found.</returns>
        public Task<CarEntity?> GetByIdAsync(int id, CarRentalStatusEntity rentalStatus);

        /// <summary>
        /// Attempts to fetch all images for a car with a specific ID. 
        /// </summary>
        /// <remarks>Returned entities will not be tracked by EF Core.</remarks>
        /// <param name="id">The ID of the car.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing a collection containing the images found.</returns>
        public Task<IEnumerable<ImageEntity>> GetCarImagesAsync(int id);

        /// <summary>
        /// Returns all the cars that are available to be rented out. 
        /// </summary>
        /// <remarks>Returned entities will not be tracked by EF Core.</remarks>
        /// <returns>A <see cref="Task{TResult}"/> containing a collection of matching cars.</returns>
        public Task<IEnumerable<CarEntity>> GetRentableCarsAsync();

        #endregion
    }
}
