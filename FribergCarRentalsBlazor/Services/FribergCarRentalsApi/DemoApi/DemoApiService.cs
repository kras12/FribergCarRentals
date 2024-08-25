using FribergCarRentals.Shared.Models.Dto.Api;
using System.Net.Http.Json;

namespace FribergCarRentalsBlazor.Services.FribergCarRentalsApi.DemoApi
{
    /// <summary>
    /// A service for managing Friberg Car Rentals Demo API endpoints.
    /// </summary>
    public class DemoApiService : ApiServiceBase, IDemoApiService
    {
        #region Constants

        /// <summary>
        /// The relative API base address.
        /// </summary>
        private const string ApiBaseAddress = "demo-api";

        /// <summary>
        /// The reset demo API endpoint address.
        /// </summary>
        private const string ResetDemoApiEndpoint = $"{ApiBaseAddress}/reset";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="httpClient">The injected HTTP client.</param>
		/// <param name="authenticationStateProvider">The injected autenthication state provider.</param>
        public DemoApiService(HttpClient httpClient, IApiUserAuthenticationStateProvider authenticationStateProvider)
            : base(httpClient, authenticationStateProvider)
        {

        }

        #endregion

        #region ApiEndpointMethods

        /// <summary>
        /// Resets the demo data. 
        /// </summary>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the results of the operation.</returns>
        public async Task<ApiResponseDto> ResetDemo()
        {
            await SetAuthorizationHeaderAsync();
            var result = await _httpClient.GetFromJsonAsync<ApiResponseDto>(ResetDemoApiEndpoint);
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        #endregion
    }
}
