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

        #endregion
    }
}
