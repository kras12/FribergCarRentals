using FribergCarRentals.Shared.Models.Dto.Api;
using FribergCarRentals.Shared.Models.Dto.Customer;
using FribergCarRentals.Shared.Models.Dto.User;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace FribergCarRentalsBlazor.Services.FribergCarRentalsApi.CustomerApi
{
    /// <summary>
    /// A service for managing customer data from Friberg Car Rentals Customer API endpoints.
    /// </summary>
    public class CustomerApiService : ApiServiceBase, ICustomerApiService
    {
        #region CustomerConstants

        /// <summary>
        /// The relative API base address.
        /// </summary>
        private const string ApiBaseAddress = "customer-api/customer";

        /// <summary>
        /// The confirm email API endpoint address
        /// </summary>
        private const string ConfirmEmailApiEndPoint = $"{ApiBaseAddress}/confirm-email";

        /// <summary>
        /// The create customer API endpoint address
        /// </summary>
        private const string CreateCustomerApiEndoint = $"{ApiBaseAddress}/create";

        /// <summary>
        /// The customer by ID API endpoint address.
        /// </summary>
        private const string CustomerByIdApiEndpoint = $"{ApiBaseAddress}/{IdPlaceHolder}";

        /// <summary>
        /// The customer login API endoint address.
        /// </summary>
        private const string LoginCustomerApiEndpoint = $"{ApiBaseAddress}/login";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="httpClient">The injected HTTP client.</param>
		/// <param name="authenticationStateProvider">The injected autenthication state provider.</param>
        public CustomerApiService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider) 
            : base(httpClient, authenticationStateProvider)
        {

        }

        #endregion

        #region ApiEndpointMethods		

        /// <summary>
        /// Confirms the email of a customer.
        /// </summary>
        /// <param name="confirmEmailDto">The input data.</param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="LoginUserResponseDto"/> object if successful.</returns>
        public async Task<ApiValueResponseDto<LoginUserResponseDto>> ConfirmEmail(ConfirmEmailDto confirmEmailDto)
        {
            var response = await _httpClient.PostAsJsonAsync(ConfirmEmailApiEndPoint, confirmEmailDto);
            var result = await response.Content.ReadFromJsonAsync<ApiValueResponseDto<LoginUserResponseDto>>();
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        /// <summary>
        /// Creates a new customer.
        /// </summary>
        /// <param name="createCustomerDto">The input data.</param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="CreatedCustomerDto"/> object if successful.</returns>
        public async Task<ApiValueResponseDto<CreatedCustomerDto>> CreateCustomer(CreateCustomerDto createCustomerDto)
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.PostAsJsonAsync(CreateCustomerApiEndoint, createCustomerDto);
            var result = await response.Content.ReadFromJsonAsync<ApiValueResponseDto<CreatedCustomerDto>>();
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        /// <summary>
        /// Fetches a customer by ID.
        /// </summary>
        /// <param name="id">The ID of the customer.</param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="CustomerDto"/> object if successful.</returns>
        public async Task<ApiValueResponseDto<CustomerDto>> GetCustomerById(int id)
        {
            await SetAuthorizationHeaderAsync();
            var result = await _httpClient.GetFromJsonAsync<ApiValueResponseDto<CustomerDto>>(CustomerByIdApiEndpoint.Replace(IdPlaceHolder, id.ToString()));
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        /// <summary>
        /// Attempts to login a customer.
        /// </summary>
        /// <param name="loginCustomerDto">The input data.</param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="LoginUserResponseDto"/> object if successful.</returns>
        public async Task<ApiValueResponseDto<LoginUserResponseDto>> LoginCustomer(LoginCustomerDto loginCustomerDto)
        {
            var response = await _httpClient.PostAsJsonAsync(LoginCustomerApiEndpoint, loginCustomerDto);
            var result = await response.Content.ReadFromJsonAsync<ApiValueResponseDto<LoginUserResponseDto>>();
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        #endregion
    }
}
