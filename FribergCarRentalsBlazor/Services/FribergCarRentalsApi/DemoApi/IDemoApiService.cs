using FribergCarRentals.Shared.Models.Dto.Api;

namespace FribergCarRentalsBlazor.Services.FribergCarRentalsApi.DemoApi
{
    /// <summary>
    /// An interface for a service for managing Friberg Car Rentals Demo API endpoints.
    /// </summary>
    public interface IDemoApiService
    {
        /// <summary>
        /// Resets the demo data. 
        /// </summary>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the results of the operation.</returns>
        public Task<ApiResponseDto> ResetDemo();
    }
}