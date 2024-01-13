using FribergCarRentals.Data;
using FribergCarRentals.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentals.DataAccess.Repositories
{
    public class GenericRepository<T> : IRepositoryBase<T> where T : class
    {
        protected readonly ApplicationDbContext _databaseContext;

        public GenericRepository(ApplicationDbContext databaseContext)
        {
            #region Checks

            if (databaseContext is null)
            {
                throw new ArgumentNullException(nameof(databaseContext), "The DBContext can't be null.");
            }

            _databaseContext = databaseContext;

            #endregion
        }

        public async virtual Task<T> Add(T entity)
        {
            await _databaseContext.Set<T>().AddAsync(entity);
            await _databaseContext.SaveChangesAsync();
            return entity;
        }

        public async virtual Task Delete(T entity)
        {
            _databaseContext.Set<T>().Remove(entity);
            await _databaseContext.SaveChangesAsync();
        }

        public async virtual Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return await _databaseContext.Set<T>().Where(predicate).AsNoTracking().ToListAsync();
        }

        public virtual ValueTask<T?> GetById(int id)
        {
            return _databaseContext.Set<T>().FindAsync(id);
        }

        public async virtual Task<IEnumerable<T>> GetAll()
        {
            return await _databaseContext.Set<T>().AsQueryable().AsNoTracking().ToListAsync();
        }

        public async virtual Task<T> Update(T entity)
        {
            _databaseContext.Update(entity);
            await _databaseContext.SaveChangesAsync();
            return entity;
        }
    }
}