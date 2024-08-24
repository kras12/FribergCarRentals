using FribergCarRentals.Shared.Models.Dto.Admin;
using FribergCarRentals.Shared.Models.Dto.Api;
using FribergCarRentals.Shared.Models.Dto.User;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.AdminApi;
using Microsoft.AspNetCore.Components.Authorization;

namespace FribergCarRentalsBlazor.Services.Authentication
{
    /// <summary>
    /// Authentication service for admins.
    /// </summary>
    public class AdminAuthenticationService : IAdminAuthenticationService
    {
		#region Fields

		/// <summary>
		/// The injected authentication state provider.
		/// </summary>
		private readonly AuthenticationStateProvider _authenticationStateProvider;

		/// <summary>
		/// The injected admin API service.
		/// </summary>
		private readonly IAdminApiService _adminApiService;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="authenticationStateProvider">The injected authentication state provider.</param>
        /// <param name="_adminApiService">The injected admin API service.</param>
        public AdminAuthenticationService(AuthenticationStateProvider authenticationStateProvider, IAdminApiService adminApiService)
		{
			_authenticationStateProvider = authenticationStateProvider;
			_adminApiService = adminApiService;
		}

        #endregion

        #region Methods

        /// <summary>
        /// Attempts to login an admin.
        /// </summary>
        /// <param name="token">The JWT token.</param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="LoginUserResponseDto"/> object if successful.</returns>
        public async Task LoginAdmin(string token)
		{
            await ((ApiUserAuthenticationStateProvider)_authenticationStateProvider).SetTokenAsync(token);
        }

        /// <summary>
        /// Attempts to login an admin.
        /// </summary>
        /// <param name="loginDto">The login credentials.</param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="LoginUserResponseDto"/> object if successful.</returns>
        public async Task<ApiValueResponseDto<LoginUserResponseDto>> LoginAdmin(LoginAdminDto loginDto)
		{
			var response = await _adminApiService.LoginAdmin(loginDto);

			if (response.Success)
			{
				await ((ApiUserAuthenticationStateProvider)_authenticationStateProvider).SetTokenAsync(response.Value!.Token);
			}

			return response;
		}

		/// <summary>
		/// Logs out the admin.
		/// </summary>
		/// <returns>A <see cref="Task"/>.</returns>
		public async Task Logout()
		{
			await ((ApiUserAuthenticationStateProvider)_authenticationStateProvider).RemoveTokenAsync();
		}

		#endregion
	}
}
