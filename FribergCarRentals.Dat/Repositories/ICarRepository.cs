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
    public interface ICarRepository : IRepositoryBase<CarEntity>
    {
        #region Methods

        /// <summary>
        /// Deletes a car from the database.
        /// </summary>
        /// <param name="id">The ID of the car to delete.</param>
        /// <returns>A <see cref="Task{TResult}"/> object containing true if the car was deleted. False if not.</returns>
        public Task<bool> Delete(int id);

        /// <summary>
        /// Attempts to fetch all cars with a specific rental status.
        /// </summary>
        /// <param name="rentalStatus">The rental status of the cars.</param>
        /// <returns>A <see cref="IEnumerable{T}"/> that contains matched cars.</returns>
        public Task<IEnumerable<CarEntity>> GetAll(CarRentalStatusEntity rentalStatus);

        /// <summary>
        /// Attempts to fetch a car with a specific ID and rental status.
        /// </summary>
        /// <param name="id">The ID of the car..</param>
        /// <param name="rentalStatus">The rental status of the car.</param>
        /// <returns>a <see cref="CarEntity"/> if the car was found. Null if not.</returns>
        public Task<CarEntity?> GetById(int id, CarRentalStatusEntity rentalStatus);

        /// <summary>
        /// Returns all the cars that are availble to be rented out. 
        /// </summary>
        /// <returns>A collection of cars.</returns>
        public Task<List<CarEntity>> GetRentableCars();

        #endregion
    }
}
