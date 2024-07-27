using FribergCarRentals.Data.EntityClasses;

namespace FribergCarRentals.Data.Repositories
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

        #endregion
    }
}
