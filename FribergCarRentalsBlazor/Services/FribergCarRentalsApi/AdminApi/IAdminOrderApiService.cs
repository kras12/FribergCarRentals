using FribergCarRentals.Shared.Models.Dto.Api;
using FribergCarRentals.Shared.Models.Dto.Order;

namespace FribergCarRentalsBlazor.Services.FribergCarRentalsApi.AdminApi
{
    /// <summary>
    /// An interface for a service for managing orders for Friberg Car Rentals Admin API endpoints.
    /// </summary>
    public interface IAdminOrderApiService
    {
        /// <summary>
        /// Completes an order.
        /// </summary>
        /// <param name="orderId">The ID of the order to complete.</param>
        /// <returns>An <see cref="ApiResponseDto"/> that contains the result of the operation.</returns>
        public Task<ApiValueResponseDto<CarOrderDto>> CompleteOrderAsync(int orderId);

        /// <summary>
        /// Deletes an order.
        /// </summary>
        /// <param name="orderId">The ID of the order to delete.</param>
        /// <returns>An <see cref="ApiResponseDto"/> that contains the result of the operation.</returns>
        public Task<ApiResponseDto> DeleteOrderAsync(int orderId);

        /// <summary>
        /// Gets an order by ID.
        /// </summary>
        /// <param name="orderId">The ID of the order.</param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="CarOrderDto"/> object if successful.</returns>
        public Task<ApiValueResponseDto<CarOrderDto>> GetOrderByIdAsync(int orderId);

        /// <summary>
        /// Gets all orders.
        /// </summary>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a collection of <see cref="CarOrderDto"/> object if successful.</returns>
        public Task<ApiValueResponseDto<List<CarOrderDto>>> GetOrdersAsync();
    }
}