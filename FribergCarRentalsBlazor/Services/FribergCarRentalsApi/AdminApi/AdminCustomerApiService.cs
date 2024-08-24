using FribergCarRentals.Shared.Models.Dto.Api;
using FribergCarRentals.Shared.Models.Dto.Customer;
using FribergCarRentals.Shared.Models.Dto.User;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace FribergCarRentalsBlazor.Services.FribergCarRentalsApi.AdminApi
{
    /// <summary>
    /// A service for managing customers for Friberg Car Rentals Admin API endpoints.
    /// </summary>
    public class AdminCustomerApiService : ApiServiceBase, IAdminCustomerApiService
	{
        #region Constants

        /// <summary>
        /// The relative API base address.
        /// </summary>
        private const string ApiBaseAddress = "admin-api/customer";

		/// <summary>
		/// The create customer API endpoint address.
		/// </summary>
		private const string CreateCustomerApiEndpoint = $"{ApiBaseAddress}";

        /// <summary>
        /// The confirm email API endpoint address.
        /// </summary>
        private const string ConfirmEmailApiEndpoint = $"{ApiBaseAddress}/confirm-email";

        /// <summary>
        /// The customer API endpoint address.
        /// </summary>
        private const string CustomerApiEndpoint = $"{ApiBaseAddress}";

		/// <summary>
		/// The delete customer API endpoint address.
		/// </summary>
		private const string DeleteCustomerApiEndpoint = $"{ApiBaseAddress}/{IdPlaceHolder}";

        /// <summary>
        /// The get confirm email data API endpoint address.
        /// </summary>
		private const string GetConfirmEmailDataApiEndpoint = $"{ApiBaseAddress}/{IdPlaceHolder}/confirm-email-code";

        /// <summary>
        /// The edit customer API endpoint address.
        /// </summary>
        private const string EditCustomerApiEndpoint = $"{ApiBaseAddress}/{IdPlaceHolder}";

		/// <summary>
		/// The get customer by ID API endpoint address.
		/// </summary>
		private const string GetCustomerByIdApiEndpoint = $"{ApiBaseAddress}/{IdPlaceHolder}";
        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="httpClient">The injected HTTP client.</param>
        /// <param name="authenticationStateProvider">The injected autenthication state provider.</param>
        public AdminCustomerApiService(HttpClient httpClient, IApiUserAuthenticationStateProvider authenticationStateProvider)
            : base(httpClient, authenticationStateProvider)
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Confirms an email address.
        /// </summary>
        /// <param name="confirmEmailData">The confirm email input. </param>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        public async Task<ApiResponseDto> ConfirmEmailAsync(ConfirmEmailDto confirmEmailData)
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.PostAsJsonAsync(ConfirmEmailApiEndpoint, confirmEmailData);
            var result = await response.Content.ReadFromJsonAsync<ApiResponseDto>();
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        /// <summary>
        /// Creates a customer.
        /// </summary>
        /// <param name="createCustomerDto">The customer input. </param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="CustomerDto"/> object if successful.</returns>
        public async Task<ApiValueResponseDto<CustomerDto>> CreateCustomerAsync(CreateCustomerDto createCustomerDto)
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.PostAsJsonAsync(CreateCustomerApiEndpoint, createCustomerDto);
            var result = await response.Content.ReadFromJsonAsync<ApiValueResponseDto<CustomerDto>>();
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        /// <summary>
        /// Deletes a customer.
        /// </summary>
        /// <param name="customerId">The ID of the customer to delete.</param>
        /// <returns>An <see cref="ApiResponseDto"/>.</returns>
        public async Task<ApiResponseDto> DeleteCustomerAsync(int customerId)
        {
            await SetAuthorizationHeaderAsync();
            var result = await _httpClient.DeleteFromJsonAsync<ApiResponseDto>(DeleteCustomerApiEndpoint.Replace(IdPlaceHolder, customerId.ToString()));
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

		/// <summary>
		/// Edits a customer.
		/// </summary>
		/// <param name="customerId">The ID of the customer.</param>
		/// <param name="customer">The new data for the customer.</param>
		/// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="CustomerDto"/> object if successful.</returns>
		public async Task<ApiValueResponseDto<CustomerDto>> EditCustomerAsync(int customerId, EditCustomerDto customer)
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.PutAsJsonAsync(EditCustomerApiEndpoint.Replace(IdPlaceHolder, customerId.ToString()), customer);
            var result = await response.Content.ReadFromJsonAsync<ApiValueResponseDto<CustomerDto>>();
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        /// <summary>
        /// Gets the code for confirming an email.
        /// </summary>
        /// <param name="customerId">The ID of the customer the email belongs to.</param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="ConfirmEmailDto"/> object if successful.</returns>
        public async Task<ApiValueResponseDto<ConfirmEmailDto>> GetConfirmEmailCodeAsync(int customerId)
        {
            await SetAuthorizationHeaderAsync();
            var result = await _httpClient.GetFromJsonAsync<ApiValueResponseDto<ConfirmEmailDto>>(GetConfirmEmailDataApiEndpoint.Replace(IdPlaceHolder, customerId.ToString()));
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        /// <summary>
        /// Gets a customer by ID.
        /// </summary>
        /// <param name="customerId">The ID of the customer..</param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="CustomerDto"/> object if successful.</returns>
        public async Task<ApiValueResponseDto<CustomerDto>> GetCustomerByIdAsync(int customerId)
		{
			await SetAuthorizationHeaderAsync();
			var result = await _httpClient.GetFromJsonAsync<ApiValueResponseDto<CustomerDto>>(GetCustomerByIdApiEndpoint.Replace(IdPlaceHolder, customerId.ToString()));
			return EnsureNotNull(result, "Failed to serialize the API response.");
		}

		/// <summary>
		/// Gets all customers.
		/// </summary>
		/// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a collection of <see cref="CustomerDto"/> object if successful.</returns>
		public async Task<ApiValueResponseDto<List<CustomerDto>>> GetCustomersAsync()
        {
            await SetAuthorizationHeaderAsync();
            var result = await _httpClient.GetFromJsonAsync<ApiValueResponseDto<List<CustomerDto>>>(CustomerApiEndpoint);
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        #endregion
    }
}
