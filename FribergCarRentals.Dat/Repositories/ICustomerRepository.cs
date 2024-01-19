using FribergCarRentals.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentals.DataAccess.Repositories
{
    public interface ICustomerRepository : IRepositoryBase<CustomerEntity>
    {
        #region Methods

        /// <summary>
        /// Deletes an entity from the database.
        /// </summary>
        /// <param name="id">The ID of the entity to delete.</param>
        /// <returns>A <see cref="Task"/> object.</returns>
        public Task Delete(int id);

        /// <summary>
        /// Updates a customer and ignore the password field.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<CustomerEntity> UpdateExcludePassword(CustomerEntity entity);

        #endregion
    }
}
