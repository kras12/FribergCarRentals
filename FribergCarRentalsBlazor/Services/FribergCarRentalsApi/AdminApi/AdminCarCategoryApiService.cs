using FribergCarRentals.Shared.Models.Dto.Api;
using FribergCarRentals.Shared.Models.Dto.CarCategory;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace FribergCarRentalsBlazor.Services.FribergCarRentalsApi.AdminApi
{
    /// <summary>
    /// A service for managing car categories for Friberg Car Rentals Admin API endpoints.
    /// </summary>
    public class AdminCarCategoryApiService : ApiServiceBase, IAdminCarCategoryApiService
    {
        #region Constants

        /// <summary>
        /// The relative API base address.
        /// </summary>
        private const string ApiBaseAddress = "admin-api/car/category";

        /// <summary>
        /// The car categories API endpoint address.
        /// </summary>
        private const string CarCategoriesApiEndpoint = $"{ApiBaseAddress}";

        /// <summary>
        /// The get car category by ID API endpoint address.
        /// </summary>
        private const string CarCategoryByIdApiEndpoint = $"{ApiBaseAddress}/{IdPlaceHolder}";

        /// <summary>
        /// The car category statistics API endpoint address.
        /// </summary>
        private const string CarCategoryStatisticsApiEndpoint = $"{ApiBaseAddress}/statistics";

        /// <summary>
        /// The create car category API endpoint address.
        /// </summary>
        private const string CreateCarCategoryApiEndpoint = $"{ApiBaseAddress}";

        /// <summary>
        /// The delete car category API endpoint address.
        /// </summary>
        private const string DeleteCarCategoryApiEndpoint = $"{ApiBaseAddress}/{IdPlaceHolder}";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="httpClient">The injected HTTP client.</param>
        /// <param name="authenticationStateProvider">The injected autenthication state provider.</param>
        public AdminCarCategoryApiService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
            : base(httpClient, authenticationStateProvider)
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a car category.
        /// </summary>
        /// <param name="createCarCategoryDto">The category input. </param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="CarCategoryDto"/> object if successful.</returns>
        public async Task<ApiValueResponseDto<CarCategoryDto>> CreateCarCategoryAsync(CreateCarCategoryDto createCarCategoryDto)
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.PostAsJsonAsync(CreateCarCategoryApiEndpoint, createCarCategoryDto);
            var result = await response.Content.ReadFromJsonAsync<ApiValueResponseDto<CarCategoryDto>>();
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        /// <summary>
        /// Deletes a car category. 
        /// </summary>
        /// <param name="carCategoryId">The ID of the category to delete.</param>
        /// <returns>An <see cref="ApiResponseDto"/>.</returns>
        public async Task<ApiResponseDto> DeleteCarCategoryAsync(int carCategoryId)
        {
            await SetAuthorizationHeaderAsync();
            var result = await _httpClient.DeleteFromJsonAsync<ApiResponseDto>(DeleteCarCategoryApiEndpoint.Replace(IdPlaceHolder, carCategoryId.ToString()));
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        /// <summary>
        /// Gets all car categories. 
        /// </summary>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a collection of <see cref="CarCategoryDto"/> object if successful.</returns>
        public async Task<ApiValueResponseDto<List<CarCategoryDto>>> GetCarCategoriesAsync()
        {
            await SetAuthorizationHeaderAsync();
            var result = await _httpClient.GetFromJsonAsync<ApiValueResponseDto<List<CarCategoryDto>>>(CarCategoriesApiEndpoint);
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        /// <summary>
        /// Gets a car category. 
        /// </summary>
        /// <param name="categoryId">The ID of the car category.</param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="CarCategoryDto"/> object if successful.</returns>
        public async Task<ApiValueResponseDto<CarCategoryDto>> GetCarCategoryAsync(int categoryId)
        {
            await SetAuthorizationHeaderAsync();
            var result = await _httpClient.GetFromJsonAsync<ApiValueResponseDto<CarCategoryDto>>(CarCategoryByIdApiEndpoint.Replace(IdPlaceHolder, categoryId.ToString()));
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        /// <summary>
        /// Gets statistics for all car categories. 
        /// </summary>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a collection of <see cref="CarCategoryStatisticsDto"/> object if successful.</returns>
        public async Task<ApiValueResponseDto<List<CarCategoryStatisticsDto>>> GetCarCategoryStatisticsAsync()
        {
            await SetAuthorizationHeaderAsync();
            var result = await _httpClient.GetFromJsonAsync<ApiValueResponseDto<List<CarCategoryStatisticsDto>>>(CarCategoryStatisticsApiEndpoint);
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        #endregion
    }
}
