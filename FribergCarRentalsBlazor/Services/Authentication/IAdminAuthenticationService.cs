using FribergCarRentals.Shared.Models.Dto.Admin;
using FribergCarRentals.Shared.Models.Dto.Api;
using FribergCarRentals.Shared.Models.Dto.User;

namespace FribergCarRentalsBlazor.Services.Authentication
{
	/// <summary>
	/// Interface for an authentication service for admins.
	/// </summary>
	public interface IAdminAuthenticationService
    {
        /// <summary>
        /// Attempts to login an admin.
        /// </summary>
        /// <param name="token">The JWT token.</param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="LoginUserResponseDto"/> object if successful.</returns>
        public Task LoginAdmin(string token);

        /// <summary>
        /// Attempts to login an admin.
        /// </summary>
        /// <param name="loginDto">The login credentials.</param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="LoginUserResponseDto"/> object if successful.</returns>
        public Task<ApiValueResponseDto<LoginUserResponseDto>> LoginAdmin(LoginAdminDto loginDto);

        /// <summary>
        /// Logs out the admin.
        /// </summary>
        /// <returns>A <see cref="Task"/>.</returns>
        public Task Logout();
    }
}