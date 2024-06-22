using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FribergCarRentals.DataAccess.DatabaseContexts;

namespace FribergCarRentals.DataAccess.Repositories
{
    /// <summary>
    /// A generic repository class.
    /// </summary>
    /// <remarks>This repository class works on detached entities. All fetched entities will not be tracked by EF Core.</remarks>
    /// <typeparam name="T">The entity type.</typeparam>
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        #region Fields

        /// <summary>
        /// The database context.
        /// </summary>
        protected readonly ApplicationDbContext _databaseContext;

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="databaseContext">The database context to use.</param>
        public GenericRepository(ApplicationDbContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds an entity to the database.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A <see cref="Task"/> object.</returns>
        public async virtual Task AddAsync(T entity)
        {
            await _databaseContext.Set<T>().AddAsync(entity);
            await _databaseContext.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes an entity from the database.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>A <see cref="Task"/> object.</returns>
        public virtual Task DeleteAsync(T entity)
        {
            _databaseContext.Set<T>().Remove(entity);
            return _databaseContext.SaveChangesAsync();
        }

        /// <summary>
        /// Performs a search in the database and returns found entities.
        /// </summary>
        /// <remarks>Returned entities will not be tracked by EF Core.</remarks>
        /// <param name="predicate">The predicate to use.</param>
        /// <returns>A <see cref="Task"/> object containing a collection of the resulting entities.</returns>
        public async virtual Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _databaseContext.Set<T>().Where(predicate).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Gets all entities from the database.
        /// </summary>
        /// <remarks>Returned entities will not be tracked by EF Core.</remarks>
        /// <returns>A <see cref="Task"/> object containg a collection of all entities found.</returns>
        public async virtual Task<IEnumerable<T>> GetAllAsync()
        {
            return await _databaseContext.Set<T>().AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Gets an entity by ID.
        /// </summary>
        /// <remarks>The resulting entity is not tracked by EF Core.</remarks>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>A <see cref="Task"/> object containg the entity.</returns>
        public abstract Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Updates an entity in the database.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A <see cref="Task"/> object.</returns>
        public virtual Task UpdateAsync(T entity)
        {
            _databaseContext.Update(entity);
            return _databaseContext.SaveChangesAsync();
        }

        #endregion
    }
}