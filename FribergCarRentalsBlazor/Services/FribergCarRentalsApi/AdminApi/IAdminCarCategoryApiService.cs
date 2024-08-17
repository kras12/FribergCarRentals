using FribergCarRentals.Shared.Models.Dto.Api;
using FribergCarRentals.Shared.Models.Dto.CarCategory;

namespace FribergCarRentalsBlazor.Services.FribergCarRentalsApi.AdminApi
{
    /// <summary>
    /// An interface for a service for managing car categories for Friberg Car Rentals Admin API endpoints.
    /// </summary>
    public interface IAdminCarCategoryApiService
    {
        /// <summary>
        /// Creates a car category.
        /// </summary>
        /// <param name="createCarCategoryDto">The category input. </param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="CarCategoryDto"/> object if successful.</returns>
        public Task<ApiValueResponseDto<CarCategoryDto>> CreateCarCategoryAsync(CreateCarCategoryDto createCarCategoryDto);

        /// <summary>
        /// Deletes a car category. 
        /// </summary>
        /// <param name="carCategoryId">The ID of the category to delete.</param>
        /// <returns>An <see cref="ApiResponseDto"/>.</returns>
        public Task<ApiResponseDto> DeleteCarCategoryAsync(int carCategoryId);

        /// <summary>
        /// Gets all car categories. 
        /// </summary>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a collection of <see cref="CarCategoryDto"/> object if successful.</returns>
        public Task<ApiValueResponseDto<List<CarCategoryDto>>> GetCarCategoriesAsync();

        /// <summary>
        /// Gets a car category. 
        /// </summary>
        /// <param name="categoryId">The ID of the car category.</param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="CarCategoryDto"/> object if successful.</returns>
        public Task<ApiValueResponseDto<CarCategoryDto>> GetCarCategoryAsync(int categoryId);

        /// <summary>
        /// Gets statistics for all car categories. 
        /// </summary>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a collection of <see cref="CarCategoryStatisticsDto"/> object if successful.</returns>
        public Task<ApiValueResponseDto<List<CarCategoryStatisticsDto>>> GetCarCategoryStatisticsAsync();
    }
}