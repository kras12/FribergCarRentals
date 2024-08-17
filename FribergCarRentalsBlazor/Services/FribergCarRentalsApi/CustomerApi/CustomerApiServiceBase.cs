using Microsoft.AspNetCore.Components.Authorization;

namespace FribergCarRentalsBlazor.Services.FribergCarRentalsApi
{
	/// <summary>
	/// A service base class for managing customer data from Friberg Car Rentals Customer API endpoints.
	/// </summary>
	public abstract class CustomerApiServiceBase : ApiServiceBase
    {
		#region Constants

		/// <summary>
		/// The customer API base address.
		/// </summary>
		protected const string ApiBase = "customer-api";

        /// <summary>
        /// The ID placeholder used in API endpoint addresses.
        /// </summary>
        protected const string IdPlaceHolder = "{id}";

        #endregion

        #region Fields

        /// <summary>
        /// The injected autenthication state provider.
        /// </summary>
        protected readonly AuthenticationStateProvider _authenticationStateProvider;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="httpClient">The injected HTTP client.</param>
		/// <param name="authenticationStateProvider">The injected autenthication state provider.</param>
        protected CustomerApiServiceBase(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider) : base(httpClient)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        #endregion

        #region JwtMethods

        /// <summary>
        /// Sets the authorization header data for logged in users. 
        /// </summary>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        protected async Task SetAuthorizationHeaderAsync()
        {
            var token = await ((ApiUserAuthenticationStateProvider)_authenticationStateProvider).GetTokenAsync();

            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        #endregion
	}
}
