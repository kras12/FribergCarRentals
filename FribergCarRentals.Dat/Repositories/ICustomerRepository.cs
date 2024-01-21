using FribergCarRentals.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentals.Data.Repositories
{
    public interface ICustomerRepository : IRepositoryBase<CustomerEntity>
    {
        #region Methods

        /// <summary>
        /// Deletes an entity from the database.
        /// </summary>
        /// <param name="id">The ID of the entity to delete.</param>
        /// <returns>A <see cref="Task"/> object containing true if the customer was deleted.</returns>
        public Task<bool> Delete(int id);

        /// <summary>
        /// Attempts to fetch a customer with matching email and password.
        /// </summary>
        /// <param name="email">The email for the customer.</param>
        /// <param name="password">The password for the customer.</param>
        /// <returns>A <see cref="Task"/> object containing the customer if found or null if not found.</returns>
        public Task<CustomerEntity?> GetMatchingCustomer(string email, string password);

        /// <summary>
        /// Updates a customer and ignore the password field.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>A <see cref="Task"/> object containing the customer.</returns>
        public Task<CustomerEntity> UpdateExcludePassword(CustomerEntity entity);

        #endregion
    }
}
