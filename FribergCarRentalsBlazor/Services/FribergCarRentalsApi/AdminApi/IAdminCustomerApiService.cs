using FribergCarRentals.Shared.Models.Dto.Api;
using FribergCarRentals.Shared.Models.Dto.Customer;
using FribergCarRentals.Shared.Models.Dto.User;

namespace FribergCarRentalsBlazor.Services.FribergCarRentalsApi.AdminApi
{
	/// <summary>
	/// An interface for a service for managing customers for Friberg Car Rentals Admin API endpoints.
	/// </summary>
	public interface IAdminCustomerApiService
	{
		/// <summary>
		/// Confirms an email address.
		/// </summary>
		/// <param name="confirmEmailData">The confirm email input. </param>
		/// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
		public Task<ApiResponseDto> ConfirmEmailAsync(ConfirmEmailDto confirmEmailData);

        /// <summary>
        /// Creates a customer.
        /// </summary>
        /// <param name="createCustomerDto">The customer input. </param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="CustomerDto"/> object if successful.</returns>
        public Task<ApiValueResponseDto<CustomerDto>> CreateCustomerAsync(CreateCustomerDto createCustomerDto);

		/// <summary>
		/// Deletes a customer.
		/// </summary>
		/// <param name="customerId">The ID of the customer to delete.</param>
		/// <returns>An <see cref="ApiResponseDto"/>.</returns>
		public Task<ApiResponseDto> DeleteCustomerAsync(int customerId);

		/// <summary>
		/// Edits a customer.
		/// </summary>
		/// <param name="customerId">The ID of the customer.</param>
		/// <param name="customer">The new data for the customer.</param>
		/// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="CustomerDto"/> object if successful.</returns>
		public Task<ApiValueResponseDto<CustomerDto>> EditCustomerAsync(int customerId, EditCustomerDto customer);

		/// <summary>
		/// Gets the code for confirming an email.
		/// </summary>
		/// <param name="customerId">The ID of the customer the email belongs to.</param>
		/// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="ConfirmEmailDto"/> object if successful.</returns>
		public Task<ApiValueResponseDto<ConfirmEmailDto>> GetConfirmEmailCodeAsync(int customerId);

        /// <summary>
        /// Gets a customer by ID.
        /// </summary>
        /// <param name="customerId">The ID of the customer..</param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="CustomerDto"/> object if successful.</returns>
        public Task<ApiValueResponseDto<CustomerDto>> GetCustomerByIdAsync(int customerId);

		/// <summary>
		/// Gets all customers.
		/// </summary>
		/// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a collection of <see cref="CustomerDto"/> object if successful.</returns>
		public Task<ApiValueResponseDto<List<CustomerDto>>> GetCustomersAsync();
	}
}