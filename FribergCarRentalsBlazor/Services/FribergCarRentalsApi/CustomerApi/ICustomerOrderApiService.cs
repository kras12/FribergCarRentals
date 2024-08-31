using FribergCarRentals.Shared.Models.Dto.Api;
using FribergCarRentals.Shared.Models.Dto.Car;
using FribergCarRentals.Shared.Models.Dto.CarCategory;
using FribergCarRentals.Shared.Models.Dto.Order;

namespace FribergCarRentalsBlazor.Services.FribergCarRentalsApi.CustomerApi
{
    // <summary>
    /// An interface for a service for managing customer order data from Friberg Car Rentals Customer API endpoints.
    /// </summary>
    public interface ICustomerOrderApiService
    {
        /// <summary>
        /// Cancels a car order.
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public Task<ApiValueResponseDto<CarOrderDto>> CancelOrderAsync(int orderId);

        /// <summary>
        /// Creates a car order.
        /// </summary>
        /// <param name="createCarOrderDto">The order input. </param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="CarOrderDto"/> object if successful.</returns>
        public Task<ApiValueResponseDto<CarOrderDto>> CreateOrderAsync(CreateCarOrderDto createCarOrderDto);

        /// <summary>
        /// Gets all car categories. 
        /// </summary>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a collection of <see cref="CarCategoryDto"/> object if successful.</returns>
        public Task<ApiValueResponseDto<List<CarCategoryDto>>> GetCarCategoriesAsync();

        /// <summary>
        /// Fetches the first car per category.
        /// </summary>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a collection of <see cref="CarDto"/> object if successful.</returns>
        public Task<ApiValueResponseDto<List<CarDto>>> GetFirstCarPerCategory();

        /// <summary>
        /// Gets an order. 
        /// </summary>
        /// <param name="orderId">The ID of the order.</param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="CarOrderDto"/> object if successful.</returns>
        public Task<ApiValueResponseDto<CarOrderDto>> GetOrderAsync(int orderId);

        /// <summary>
        /// Gets all orders.
        /// </summary>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a collection of <see cref="CarOrderDto"/> object if successful.</returns>
        public Task<ApiValueResponseDto<List<CarOrderDto>>> GetOrdersAsync();

        /// <summary>
        /// Searches for rentable cars.
        /// </summary>
        /// <param name="carRentalSearchDto">The input data.</param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="CarRentalSearchResultDto"/> object if successful.</returns>
        public Task<ApiValueResponseDto<CarRentalSearchResultDto>> SearchRentableCarsAsync(CarRentalSearchDto carRentalSearchDto);

    }
}