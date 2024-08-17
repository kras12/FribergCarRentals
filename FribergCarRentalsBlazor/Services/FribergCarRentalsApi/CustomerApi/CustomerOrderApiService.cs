using FribergCarRentals.Shared.Models.Dto.Api;
using FribergCarRentals.Shared.Models.Dto.CarCategory;
using FribergCarRentals.Shared.Models.Dto.Order;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace FribergCarRentalsBlazor.Services.FribergCarRentalsApi.CustomerApi
{
    /// <summary>
    /// A service service for managing customer order data for Friberg Car Rentals Customer API endpoints.
    /// </summary>
    public class CustomerOrderApiService : ApiServiceBase, ICustomerOrderApiService
    {
        #region CustomerConstants        

        /// <summary>
        /// The relative API base address.
        /// </summary>
        private const string ApiBaseAddress = "customer-api/order";

        /// <summary>
        /// The cancel order API endpoint address.
        /// </summary>
        private const string CancelOrderApiEndpoint = $"{ApiBaseAddress}/{IdPlaceHolder}/cancel";

        /// <summary>
        /// The car categories API endpoint address.
        /// </summary>
        private const string CarCategoriesApiEndpoint = $"{ApiBaseAddress}/car-categories";

        /// <summary>
        /// The create order API endpoint address.
        /// </summary>
        private const string OrderApiEndpoint = $"{ApiBaseAddress}";

        /// <summary>
        /// The create order API endpoint address.
        /// </summary>
        private const string OrderByIdApiEndpoint = $"{ApiBaseAddress}/{IdPlaceHolder}";

        /// <summary>
        /// The search cars API endpoint address.
        /// </summary>
        private const string SearchCarsApiEndPoint = $"{ApiBaseAddress}/search-cars";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="httpClient">The injected HTTP client.</param>
        /// <param name="authenticationStateProvider">The injected autenthication state provider.</param>
        public CustomerOrderApiService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider) 
            : base(httpClient, authenticationStateProvider)
        {

        }

        #endregion

        #region Methods		

        /// <summary>
        /// Cancels a car order.
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<ApiValueResponseDto<CarOrderDto>> CancelOrderAsync(int orderId)
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.PutAsJsonAsync<object>(CancelOrderApiEndpoint.Replace(IdPlaceHolder, orderId.ToString()), null!);
            var result = await response.Content.ReadFromJsonAsync<ApiValueResponseDto<CarOrderDto>>();
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        /// <summary>
        /// Creates a car order.
        /// </summary>
        /// <param name="createCarOrderDto">The order input. </param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="CarOrderDto"/> object if successful.</returns>
        public async Task<ApiValueResponseDto<CarOrderDto>> CreateOrderAsync(CreateCarOrderDto createCarOrderDto)
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.PostAsJsonAsync(OrderApiEndpoint, createCarOrderDto);
            var result = await response.Content.ReadFromJsonAsync<ApiValueResponseDto<CarOrderDto>>();
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        /// <summary>
        /// Gets all car categories. 
        /// </summary>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a collection of <see cref="CarCategoryDto"/> object if successful.</returns>
        public async Task<ApiValueResponseDto<List<CarCategoryDto>>> GetCarCategoriesAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<ApiValueResponseDto<List<CarCategoryDto>>>(CarCategoriesApiEndpoint);
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        /// <summary>
        /// Gets an order. 
        /// </summary>
        /// <param name="orderId">The ID of the order.</param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="CarOrderDto"/> object if successful.</returns>
        public async Task<ApiValueResponseDto<CarOrderDto>>GetOrderAsync(int orderId)
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
            var result = await _httpClient.GetFromJsonAsync<ApiValueResponseDto<List<CarOrderDto>>> (OrderApiEndpoint);
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        /// <summary>
        /// Searches for rentable cars.
        /// </summary>
        /// <param name="carRentalSearchDto">The input data.</param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="CarRentalSearchResultDto"/> object if successful.</returns>
        public async Task<ApiValueResponseDto<CarRentalSearchResultDto>> SearchRentableCarsAsync(CarRentalSearchDto carRentalSearchDto)
        {
            var response = await _httpClient.PostAsJsonAsync(SearchCarsApiEndPoint, carRentalSearchDto);
            var result = await response.Content.ReadFromJsonAsync<ApiValueResponseDto<CarRentalSearchResultDto>>();
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        #endregion
    }
}
