using System.Text;

namespace FribergCarRentalsBlazor.Services.FribergCarRentalsApi
{
	/// <summary>
	/// Base class for all Friberg Car Rentals API services.
	/// </summary>
	public abstract class ApiServiceBase
	{
        #region Constants

        /// <summary>
        /// The ID placeholder used in API endpoint addresses.
        /// </summary>
        protected const string IdPlaceHolder = "{id}";

        #endregion

        #region Fields

        /// <summary>
        /// The injected authentication state provider.
        /// </summary>
        protected readonly IApiUserAuthenticationStateProvider _authenticationStateProvider;

        /// <summary>
        /// The injected HTTP client.
        /// </summary>
        protected readonly HttpClient _httpClient;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="httpClient">The injected HTTP client.</param>
		/// <param name="authenticationStateProvider">The injected authentication state provider.</param>
        protected ApiServiceBase(HttpClient httpClient, IApiUserAuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Builds the query string to send with a request. 
        /// </summary>
        /// <param name="queries">A collection of key value pairs for the queries to include.</param>
        /// <returns>A formatted query <see cref="string"/>, or an empty string if the collection is empty.</returns>
        /// <exception cref="ArgumentException"></exception>
        protected string BuildQueryString(List<KeyValuePair<string, string>> queries)
		{
			#region Checks

			if (queries.Any(x => string.IsNullOrEmpty(x.Key) || string.IsNullOrEmpty(x.Value)))
			{
				throw new ArgumentException("The query collection contains invalid parameters.", nameof(queries));
			}

			#endregion

			StringBuilder stringBuilder = new();

			foreach (var query in queries)
			{
				if (stringBuilder.Length == 0)
				{
					stringBuilder.Append("?");
				}
				else
				{
					stringBuilder.Append("&");
				}

				stringBuilder.Append($"{query.Key}={query.Value}");
			}

			return stringBuilder.ToString();
		}

		/// <summary>
		/// Builds the query string to send with a request.
		/// </summary>
		/// <param name="key">The name of the parameter.</param>
		/// <param name="value">The value of the parameter.</param>
		/// <returns>A <see cref="string"/>.</returns>
		/// <exception cref="ArgumentException"></exception>
		protected string BuildQueryString(string key, string value)
		{
			return BuildQueryString(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>(key, value) });
		}

		/// <summary>
		/// Checks the value of an object reference and throws an exception if it's null. 
		/// If it has a value the object is returned. 
		/// </summary>
		/// <param name="targetObject">The object to check.</param>
		/// <param name="exceptionMessage">An optional message to use for the exception.</param>
		/// <returns>The object if it's not null.</returns>
		/// <exception cref="Exception"></exception>
		protected T EnsureNotNull<T>(T? targetObject, string? exceptionMessage = null) where T : class
		{
			if (targetObject == null)
			{
				throw new Exception(exceptionMessage);
			}

			return targetObject;
		}

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
