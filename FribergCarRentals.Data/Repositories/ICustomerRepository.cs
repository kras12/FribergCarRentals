using FribergCarRentals.Data.Entities;
using FribergCarRentals.Data.EntityClasses;
using Microsoft.AspNetCore.Identity;

namespace FribergCarRentals.Data.Repositories
{
    /// <summary>
    /// An interface for a customer repository.
    /// </summary>
    public interface ICustomerRepository : IGenericRepository<CustomerEntity>
    {
        #region Methods

        /// <summary>
        /// Confirms an email account for the customer.
        /// </summary>
        /// <param name="id">The ID of the customer.</param>
        /// <param name="token">The token to use.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public Task<IdentityResult> ConfirmEmailAsync(int id, string token);

        /// <summary>
        /// Checks whether a customer with the specified email exists. 
        /// </summary>
        /// <param name="email">The email for the customer.</param>
        /// <returns>A <see cref="Task"/> object containing true if there was a matching customer.</returns>
        public Task<bool> CustomerExists(string email);

        /// <summary>
        /// Checks whether a customer with the specified ID exists. 
        /// </summary>
        /// <param name="id">The ID for the customer.</param>
        /// <returns>A <see cref="Task"/> object containing true if there was a matching customer.</returns>
        public Task<bool> CustomerExists(int id);

        /// <summary>
        /// Deletes a customer from the database.
        /// </summary>
        /// <param name="id">The ID of the customer to delete.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public Task DeleteAsync(int id);

        /// <summary>
        /// Generates an email confirmation token for the customer.
        /// </summary>
        /// <param name="customerId">The ID of the customer.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public Task<string> GenerateEmailConfirmationTokenAsync(int customerId);

        /// <summary>
        /// Gets a customer by email.
        /// </summary>
        /// <param name="email">The email of the customer.</param>
        /// <returns>A <see cref="Task"/> object containg the customer.</returns>
        public Task<CustomerEntity?> GetByEmailAsync(string email);

        /// <summary>
        /// Attempts to fetch a customer by user ID.
        /// </summary>
        /// <remarks>Returned entities will not be tracked by EF Core.</remarks>
        /// <param name="id">The ID of the admin.</param>
        /// <returns>A <see cref="Task"/> object containing the admin if found or null if not found.</returns>
        public Task<CustomerEntity?> GetByUserIdAsync(string userId);

        /// <summary>
        /// Attempts to fetch the user ID for the customer.
        /// </summary>
        /// <param name="id">The ID of the customer.</param>
        /// <returns>A <see cref="Task"/> object containing the user ID if found or null if not found.</returns>
        public Task<string?> GetUserId(int id);

        /// <summary>
        /// Checks whether the customer's email address is confirmed.
        /// </summary>
        /// <param name="customer">The customer.</param>
        /// <returns>A <see cref="Task"/> object containing true if the email is confirmed.</returns>
        public Task<bool> IsEmailConfirmedAsync(CustomerEntity customer);

        /// <summary>
        /// Updates the password of a customer.
        /// </summary>
        /// <param name="customerId">The ID of the customer.</param>
        /// <param name="password">The new password.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public Task UpdatePasswordAsync(int customerId, string password);

        #endregion
    }
}
