using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentals.Data.Repositories
{
    /// <summary>
    /// An interface for a generic repository
    /// </summary>
    public interface IRepositoryBase<T>
    {
        public ValueTask<T?> GetById(int id);

        public Task<IEnumerable<T>> GetAll();

        public Task<T> Update(T entity);


        // Since there's no convenient way of deleting entities by ID in a generic way, 
        // we can only support a T parameter here. 

        public Task<bool> Delete(T entity);

        public Task<T> Add(T entity);

        public Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
    }
}
