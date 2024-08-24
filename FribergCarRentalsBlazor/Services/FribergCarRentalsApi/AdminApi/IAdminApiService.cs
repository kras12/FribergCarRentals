using FribergCarRentals.Shared.Models.Dto.Admin;
using FribergCarRentals.Shared.Models.Dto.Api;
using FribergCarRentals.Shared.Models.Dto.User;

namespace FribergCarRentalsBlazor.Services.FribergCarRentalsApi.AdminApi
{
    /// <summary>
    /// An interface for a service for managing admin data for Friberg Car Rentals Admin API endpoints.
    /// </summary>
    public interface IAdminApiService
    {
        /// <summary>
        /// Fetches an admin by ID.
        /// </summary>
        /// <param name="id">The ID of the admin.</param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="AdminDto"/> object if successful.</returns>
        public Task<ApiValueResponseDto<AdminDto>> GetAdminById(int id);

        /// <summary>
        /// Attempts to login an admin.
        /// </summary>
        /// <param name="loginAdminData">The input data.</param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="LoginUserResponseDto"/> object if successful.</returns>
        public Task<ApiValueResponseDto<LoginUserResponseDto>> LoginAdmin(LoginAdminDto loginAdminData);
    }
}