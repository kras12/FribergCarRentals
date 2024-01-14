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

        #endregion
    }
}
