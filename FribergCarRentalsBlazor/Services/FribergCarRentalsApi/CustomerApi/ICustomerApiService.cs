using FribergCarRentals.Shared.Models.Dto.Api;
using FribergCarRentals.Shared.Models.Dto.Customer;
using FribergCarRentals.Shared.Models.Dto.User;

namespace FribergCarRentalsBlazor.Services.FribergCarRentalsApi.CustomerApi
{
    /// <summary>
    /// An interface for a service for managing customer data for Friberg Car Rentals Customer API endpoints.
    /// </summary>
    public interface ICustomerApiService
    {
        /// <summary>
        /// Confirms the email of a customer.
        /// </summary>
        /// <param name="confirmEmailDto">The input data.</param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="LoginUserResponseDto"/> object if successful.</returns>
        public Task<ApiValueResponseDto<LoginUserResponseDto>> ConfirmEmail(ConfirmEmailDto confirmEmailDto);

        /// <summary>
        /// Creates a new customer.
        /// </summary>
        /// <param name="createCustomerDto">The input data.</param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="CreatedCustomerDto"/> object if successful.</returns>
        public Task<ApiValueResponseDto<CreatedCustomerDto>> CreateCustomer(CreateCustomerDto createCustomerDto);

        /// <summary>
        /// Fetches a customer by ID.
        /// </summary>
        /// <param name="id">The ID of the customer.</param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="CustomerDto"/> object if successful.</returns>
        public Task<ApiValueResponseDto<CustomerDto>> GetCustomerById(int id);

        /// <summary>
        /// Attempts to login a customer.
        /// </summary>
        /// <param name="loginCustomerDto">The input data.</param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="LoginUserResponseDto"/> object if successful.</returns>
        public Task<ApiValueResponseDto<LoginUserResponseDto>> LoginCustomer(LoginCustomerDto loginCustomerDto);
    }
}