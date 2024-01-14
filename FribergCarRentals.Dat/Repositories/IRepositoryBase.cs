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
    public interface IRepositoryBase<T>
    {
        public ValueTask<T?> GetById(int id);

        public Task<IEnumerable<T>> GetAll();

        public Task<T> Update(T entity);

        public Task Delete(T entity);

        public Task<T> Add(T entity);

        public Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
    }
}
