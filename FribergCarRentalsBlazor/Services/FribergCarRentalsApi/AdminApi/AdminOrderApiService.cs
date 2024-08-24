using FribergCarRentals.Shared.Models.Dto.Api;
using FribergCarRentals.Shared.Models.Dto.Order;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace FribergCarRentalsBlazor.Services.FribergCarRentalsApi.AdminApi
{
    /// <summary>
    /// A service for managing orders for Friberg Car Rentals Admin API endpoints.
    /// </summary>
    public class AdminOrderApiService : ApiServiceBase, IAdminOrderApiService
    {
        #region Constants

        /// <summary>
        /// The relative API base address.
        /// </summary>
        private const string ApiBaseAddress = "admin-api/order";

        /// <summary>
        /// The complete order API endpoint address.
        /// </summary>
        private const string CompleteOrderApiEndpoint = $"{ApiBaseAddress}/{IdPlaceHolder}/complete";

        /// <summary>
        /// The delete order API endpoint address.
        /// </summary>
        private const string DeleteOrderApiEndpoint = $"{ApiBaseAddress}/{IdPlaceHolder}";

        /// <summary>
        /// The order API endpoint address.
        /// </summary>
        private const string OrderApiEndpoint = $"{ApiBaseAddress}";

        /// <summary>
        /// The get order by ID API endpoint address.
        /// </summary>
        private const string OrderByIdApiEndpoint = $"{ApiBaseAddress}/{IdPlaceHolder}";
        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="httpClient">The injected HTTP client.</param>
        /// <param name="authenticationStateProvider">The injected autenthication state provider.</param>
        public AdminOrderApiService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
            : base(httpClient, authenticationStateProvider)
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Completes an order.
        /// </summary>
        /// <param name="orderId">The ID of the order to complete.</param>
        /// <returns>An <see cref="ApiResponseDto"/> that contains the result of the operation.</returns>
        public async Task<ApiValueResponseDto<CarOrderDto>> CompleteOrderAsync(int orderId) 
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.PutAsJsonAsync<object>(CompleteOrderApiEndpoint.Replace(IdPlaceHolder, orderId.ToString()), null!);
            var result = await response.Content.ReadFromJsonAsync<ApiValueResponseDto<CarOrderDto>>();
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        /// <summary>
        /// Deletes an order.
        /// </summary>
        /// <param name="orderId">The ID of the order to delete.</param>
        /// <returns>An <see cref="ApiResponseDto"/> that contains the result of the operation.</returns>
        public async Task<ApiResponseDto> DeleteOrderAsync(int orderId)
        {
            await SetAuthorizationHeaderAsync();
            var result = await _httpClient.DeleteFromJsonAsync<ApiResponseDto>(DeleteOrderApiEndpoint.Replace(IdPlaceHolder, orderId.ToString()));
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }
        /// <summary>
        /// Gets an order by ID.
        /// </summary>
        /// <param name="orderId">The ID of the order.</param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="CarOrderDto"/> object if successful.</returns>
        public async Task<ApiValueResponseDto<CarOrderDto>> GetOrderByIdAsync(int orderId)
        {
            await SetAuthorizationHeaderAsync();
            var result = await _httpClient.GetFromJsonAsync<ApiValueResponseDto<CarOrderDto>>(OrderByIdApiEndpoint.Replace(IdPlaceHolder, orderId.ToString()));
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        /// <summary>
        /// Gets all orders.
        /// </summary>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a collection of <see cref="CarOrderDto"/> object if successful.</returns>
        public async Task<ApiValueResponseDto<List<CarOrderDto>>> GetOrdersAsync()
        {
            await SetAuthorizationHeaderAsync();
            var result = await _httpClient.GetFromJsonAsync<ApiValueResponseDto<List<CarOrderDto>>>(OrderApiEndpoint);
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        #endregion
    }
}
