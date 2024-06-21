using FribergCarRentals.DataAccess.EntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentals.DataAccess.Repositories
{
    /// <summary>
    /// An interface for a customer repository.
    /// </summary>
    public interface ICustomerRepository : IGenericRepository<CustomerEntity>
    {
        #region Methods

        /// <summary>
        /// Checks whether a customer with the specified email exists. 
        /// </summary>
        /// <param name="email">The email for the customer.</param>
        /// <returns>A <see cref="Task"/> object containing true if there was a matching customer.</returns>
        public Task<bool> CustomerExists(string email);

        /// <summary>
        /// Deletes a customer from the database.
        /// </summary>
        /// <param name="id">The ID of the customer to delete.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public Task DeleteAsync(int id);

        /// <summary>
        /// Attempts to fetch a customer with matching email and password.
        /// </summary>
        /// <remarks>Returned entities will not be tracked by EF Core.</remarks>
        /// <param name="email">The email for the customer.</param>
        /// <param name="password">The password for the customer.</param>
        /// <returns>A <see cref="Task"/> object containing the customer if found or null if not found.</returns>
        public Task<CustomerEntity?> GetMatchingCustomerAsync(string email, string password);

        /// <summary>
        /// Updates a customer and ignores the password field.
        /// </summary>
        /// <param name="entity">The customer to update</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public Task UpdateExcludePasswordAsync(CustomerEntity entity);

        #endregion
    }
}
