using FribergCarRentals.Shared.Models.Dto.Api;
using FribergCarRentals.Shared.Models.Dto.CarCategory;
using FribergCarRentals.Shared.Models.Dto.Order;

namespace FribergCarRentalsBlazor.Services.FribergCarRentalsApi
{
    // <summary>
    /// An interface for a service for managing customer order data from Friberg Car Rentals Customer API endpoints.
    /// </summary>
    public interface ICustomerOrderApiService
    {
        /// <summary>
        /// Gets all car categories. 
        /// </summary>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a collection of <see cref="CarCategoryDto"/> object if successful.</returns>
        public Task<ApiValueResponseDto<List<CarCategoryDto>>> GetCarCategories();

        /// <summary>
        /// Searches for rentable cars.
        /// </summary>
        /// <param name="carRentalSearchDto">The input data.</param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="CarRentalSearchResultDto"/> object if successful.</returns>
        public Task<ApiValueResponseDto<CarRentalSearchResultDto>> SearchRentableCars(CarRentalSearchDto carRentalSearchDto);

    }
}