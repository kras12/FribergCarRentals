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
    /// An interface for a generic repository
    /// </summary>
    public interface IGenericRepository<T>
    {
        /// <summary>
        /// Adds an entity to the database.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A <see cref="Task"/> object.</returns>
        public Task AddAsync(T entity);

        /// <summary>
        /// Deletes an entity from the database.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>A <see cref="Task"/> object.</returns>
        public Task DeleteAsync(T entity);

        /// <summary>
        /// Performs a search in the database and returns found entities.
        /// </summary>
        /// <remarks>Returned entities will not be tracked by EF Core.</remarks>
        /// <param name="predicate">The predicate to use.</param>
        /// <returns>A <see cref="Task"/> object containing a collection of the resulting entities.</returns>
        public Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Gets all entities from the database.
        /// </summary>
        /// <remarks>Returned entities will not be tracked by EF Core.</remarks>
        /// <returns>A <see cref="Task"/> object containg a collection of all entities found.</returns>
        public Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Gets an entity by ID.
        /// </summary>
        /// <remarks>Returned entities will not be tracked by EF Core.</remarks>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>A <see cref="Task"/> object containg the entity.</returns>
        public Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Updates an entity in the database.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A <see cref="Task"/> object.</returns>
        public Task UpdateAsync(T entity);
    }
}
