using FribergCarRentals.Shared.Models.Dto.Api;
using FribergCarRentals.Shared.Models.Dto.CarCategory;
using FribergCarRentals.Shared.Models.Dto.Order;
using System.Net.Http.Json;

namespace FribergCarRentalsBlazor.Services.FribergCarRentalsApi
{
	/// <summary>
	/// A service service for managing customer order data for Friberg Car Rentals Customer API endpoints.
	/// </summary>
	public class CustomerOrderApiService : ApiServiceBase, ICustomerOrderApiService
	{
		#region CustomerConstants

		/// <summary>
		/// The car categories API endoint address.
		/// </summary>
		private const string CarCategoriesApiEndpoint = $"{ApiBase}/car-categories";

        /// <summary>
        /// The search cars API endpoint address.
        /// </summary>
        private const string SearchCarsApiEndPoint = $"{ApiBase}/search-cars";

		#endregion

		#region Constants

		/// <summary>
		/// The customer API base address.
		/// </summary>
		private const string ApiBase = "customer-api/customer/order";

		/// <summary>
		/// The ID placeholder used in API endpoint addresses.
		/// </summary>
		private const string IdPlaceHolder = "{id}";

		#endregion

		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="httpClient">The injected HTTP client.</param>
		public CustomerOrderApiService(HttpClient httpClient) : base(httpClient)
		{

		}

        #endregion

        #region Methods		

        /// <summary>
        /// Gets all car categories. 
        /// </summary>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a collection of <see cref="CarCategoryDto"/> object if successful.</returns>
        public async Task<ApiValueResponseDto<List<CarCategoryDto>>> GetCarCategories()
		{
            var result = await _httpClient.GetFromJsonAsync<ApiValueResponseDto<List<CarCategoryDto>>>(CarCategoriesApiEndpoint);
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        /// <summary>
        /// Searches for rentable cars.
        /// </summary>
        /// <param name="carRentalSearchDto">The input data.</param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="CarRentalSearchResultDto"/> object if successful.</returns>
        public async Task<ApiValueResponseDto<CarRentalSearchResultDto>> SearchRentableCars(CarRentalSearchDto carRentalSearchDto)
		{
			var response = await _httpClient.PostAsJsonAsync(SearchCarsApiEndPoint, carRentalSearchDto);
			var result = await response.Content.ReadFromJsonAsync<ApiValueResponseDto<CarRentalSearchResultDto>>();
			return EnsureNotNull(result, "Failed to serialize the API response.");
		}

		#endregion
	}
}
