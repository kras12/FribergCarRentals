using FribergCarRentals.Data.EntityClasses;

namespace FribergCarRentalsApi.Services
{
	/// <summary>
	/// Interface for a class that creates JWT tokens for user classes.
	/// </summary>
	public interface ITokenService
    {
        /// <summary>
        /// Creates a JWT token for an admin.
        /// </summary>
        /// <param name="admin">The admin to create the token for.</param>
        /// <returns>The created token as a <see cref="string"/>.</returns>
        public Task<string> CreateToken(AdminEntity admin);

        /// <summary>
        /// Creates a JWT token for a customer.
        /// </summary>
        /// <param name="customer">The customer to create the token for.</param>
        /// <returns>The created token as a <see cref="string"/>.</returns>
        public Task<string> CreateToken(CustomerEntity customer);
    }
}
