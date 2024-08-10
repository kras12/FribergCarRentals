using FribergCarRentals.Shared.Models.Dto.Api;
using FribergCarRentals.Shared.Models.Dto.Customer;
using FribergCarRentals.Shared.Models.Dto.User;

namespace FribergCarRentalsBlazor.Services.Authentication
{
	/// <summary>
	/// Interface for an authentication service for customers.
	/// </summary>
	public interface ICustomerAuthenticationService
	{
        /// <summary>
        /// Attempts to login a customer.
        /// </summary>
        /// <param name="loginDto">The login credentials.</param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="LoginUserResponseDto"/> object if successful.</returns>
        public Task<ApiValueResponseDto<LoginUserResponseDto>> LoginCustomer(LoginCustomerDto loginDto);

		/// <summary>
		/// Logs out the customer.
		/// </summary>
		/// <returns>A <see cref="Task"/>.</returns>
		public Task Logout();
	}
}