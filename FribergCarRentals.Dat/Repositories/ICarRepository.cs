using FribergCarRentals.Models;
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
        /// Deletes an entity from the database.
        /// </summary>
        /// <param name="id">The ID of the entity to delete.</param>
        /// <returns>A <see cref="Task"/> object.</returns>
        public Task Delete(int id);

        /// <summary>
        /// Returns all the cars that are availble to be rented out. 
        /// </summary>
        /// <returns>A collection of cars.</returns>
        public Task<List<CarEntity>> GetRentableCars();

        #endregion
    }
}
