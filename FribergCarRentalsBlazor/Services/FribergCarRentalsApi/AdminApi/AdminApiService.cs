using FribergCarRentals.Shared.Models.Dto.Admin;
using FribergCarRentals.Shared.Models.Dto.Api;
using FribergCarRentals.Shared.Models.Dto.User;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace FribergCarRentalsBlazor.Services.FribergCarRentalsApi.CustomerApi
{
    /// <summary>
    /// A service for managing admin data from Friberg Car Rentals Admin API endpoints.
    /// </summary>
    public class AdminApiService : ApiServiceBase, IAdminApiService
    {
        #region CustomerConstants

        /// <summary>
        /// The relative API base address.
        /// </summary>
        private const string ApiBaseAddress = "admin-api/admin";

        /// <summary>
        /// The admin by ID API endpoint address.
        /// </summary>
        private const string AdminByIdApiEndpoint = $"{ApiBaseAddress}/{IdPlaceHolder}";

        /// <summary>
        /// The admin login API endoint address.
        /// </summary>
        private const string LoginAdminApiEndpoint = $"{ApiBaseAddress}/login";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="httpClient">The injected HTTP client.</param>
		/// <param name="authenticationStateProvider">The injected autenthication state provider.</param>
        public AdminApiService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider) 
            : base(httpClient, authenticationStateProvider)
        {

        }

        #endregion

        #region ApiEndpointMethods

        /// <summary>
        /// Fetches an admin by ID.
        /// </summary>
        /// <param name="id">The ID of the admin.</param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="AdminDto"/> object if successful.</returns>
        public async Task<ApiValueResponseDto<AdminDto>> GetAdminById(int id)
        {
            await SetAuthorizationHeaderAsync();
            var result = await _httpClient.GetFromJsonAsync<ApiValueResponseDto<AdminDto>>(AdminByIdApiEndpoint.Replace(IdPlaceHolder, id.ToString()));
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        /// <summary>
        /// Attempts to login an admin.
        /// </summary>
        /// <param name="loginAdminData">The input data.</param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="LoginUserResponseDto"/> object if successful.</returns>
        public async Task<ApiValueResponseDto<LoginUserResponseDto>> LoginAdmin(LoginAdminDto loginAdminData)
        {
            var response = await _httpClient.PostAsJsonAsync(LoginAdminApiEndpoint, loginAdminData);
            var result = await response.Content.ReadFromJsonAsync<ApiValueResponseDto<LoginUserResponseDto>>();
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        #endregion
    }
}
