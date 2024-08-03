namespace FribergCarRentals.Shared.Dto.Api
{
    /// <summary>
    /// API response class having a value of type <see cref="T"/>.
    /// </summary>
    public class ApiValueResponseDto<T> : ApiResponseDto where T : class
    {
        #region Constructors

        /// <summary>
        /// Constructor for a successful response.
        /// </summary>
        /// <param name="value">The value to send in the response body.</param>
        private ApiValueResponseDto(T? value = null) : base(success: true)
        {
            Value = value;
        }

        /// <summary>
        /// Constructor for an error response.
        /// </summary>
        /// <param name="errors">A collection of errors.</param>
        private ApiValueResponseDto(List<KeyValuePair<string, string>> errors) : base(errors)
        {
            
        }

        /// <summary>
        /// Constructor for an error response.
        /// </summary>
        /// <param name="errorType">The type of error.</param>
        /// <param name="errorMessage">The error message.</param>
        private ApiValueResponseDto(string errorType, string errorMessage)
            : this(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>(errorType, errorMessage) })
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// The value for a successful response.
        /// </summary>
        public T? Value { get; }

        #endregion

        #region FactoryMethods

        /// <summary>
        /// Creates an error response.
        /// </summary>
        /// <param name="errorType">The type of error.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns><see cref="ApiResponseDto"/> containing the supplied errors.</returns>
        public static new ApiValueResponseDto<T> CreateErrorResponse(ApiErrorMessageTypes errorType, string errorMessage)
        {
            return CreateErrorResponse(errorType.ToString(), errorMessage);
        }

        /// <summary>
        /// Creates an error response.
        /// </summary>
        /// <param name="errorType">The type of error.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns><see cref="ApiResponseDto"/> containing the supplied errors.</returns>
        public static new ApiValueResponseDto<T> CreateErrorResponse(string errorType, string errorMessage)
        {
            return CreateErrorResponse(new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>(errorType, errorMessage) });
        }

        /// <summary>
        /// Creates an error response.
        /// </summary>
        /// <param name="errors">A collection of errors having labels and descriptions.</param>
        /// <returns><see cref="ApiResponseDto"/> containing the supplied errors.</returns>
        public static new ApiValueResponseDto<T> CreateErrorResponse(List<KeyValuePair<string, string>> errors)
        {
            return new ApiValueResponseDto<T>(errors);
        }

        /// <summary>
        /// Creates a successful response having a value of type <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the response value</typeparam>
        /// <param name="value">The response value.</param>
        /// <returns><see cref="ApiResponseDto"/> containing the supplied response value.</returns>
        public static ApiValueResponseDto<T> CreateSuccessfulResponse(T? value)
        {
            return new ApiValueResponseDto<T>(value);
        }

        #endregion
    }
}
