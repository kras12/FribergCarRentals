using System.Text;

namespace FribergCarRentals.Shared.Dto.Api
{
    /// <summary>
    /// Generic API response class to support web APIs.
    /// </summary>
    public class ApiResponseDto<T> where T : class
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public ApiResponseDto()
        {
            
        }

        /// <summary>
        /// Constructor for a successful response.
        /// </summary>
        /// <param name="value">The value to send in the response body.</param>
        private ApiResponseDto(T? value = null)
        {
            Value = value;
            Success = true;
        }

        /// <summary>
        /// Constructor for an error response.
        /// </summary>
        /// <param name="errors">A collection of errors.</param>
        private ApiResponseDto(List<KeyValuePair<string, string>> errors)
        {
            Errors = errors;
            Success = false;
        }

        /// <summary>
        /// Constructor for an error response.
        /// </summary>
        /// <param name="errorType">The type of error.</param>
        /// <param name="errorMessage">The error message.</param>
        private ApiResponseDto(string errorType, string errorMessage)
            : this(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>(errorType, errorMessage) })
        {

        }

        #endregion

        #region FactoryMethods

        /// <summary>
        /// Creates an error response.
        /// </summary>
        /// <param name="errorType">The type of error.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns><see cref="ApiResponseDto"/> containing the supplied errors.</returns>
        public static ApiResponseDto<T> CreateErrorResponse(string errorType, string errorMessage)
        {
            return CreateErrorResponse(new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>(errorType, errorMessage) });
        }

        /// <summary>
        /// Creates an error response.
        /// </summary>
        /// <param name="errors">A collection of errors having labels and descriptions.</param>
        /// <returns><see cref="ApiResponseDto"/> containing the supplied errors.</returns>
        public static ApiResponseDto<T> CreateErrorResponse(List<KeyValuePair<string, string>> errors)
        {
            return new ApiResponseDto<T>(errors);
        }

        /// <summary>
        /// Creates a successful response.
        /// </summary>
        /// <typeparam name="T">The type of the response value</typeparam>
        /// <param name="value">The response value.</param>
        /// <returns><see cref="ApiResponseDto"/> containing the supplied response value.</returns>
        public static ApiResponseDto<T> CreateSuccessfulResponse(T value)
        {
            return new ApiResponseDto<T>(value);
        }

        #endregion

        #region Properties

        /// <summary>
        /// A collection of errors. 
        /// </summary>
        public List<KeyValuePair<string, string>> Errors { get; } = new();

        /// <summary>
        /// True if operation was successful.
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// The value for a successful response.
        /// </summary>
        public T? Value { get; }

        #endregion

        #region OtherMethods

        /// <summary>
        /// Formats the error descriptions as a list.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of <see cref="string"/>.</returns>
        public List<string> GetErrorDescriptionsAsList()
        {
            return Errors.Select(x => x.Value).ToList();
        }

        /// <summary>
        /// Formats the error collection as a string.
        /// </summary>
        /// <returns>A <see cref="string"/>.</returns>
        public string GetErrorsAsString()
        {
            var stringBuilder = new StringBuilder();
            Errors.ForEach(x => stringBuilder.AppendLine($"{x.Key}: {x.Value}"));
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Formats the error collection as a list.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of <see cref="string"/>.</returns>
        public List<string> GetErrorsAsList()
        {
            return Errors.Select(x => $"{x.Key}: {x.Value}").ToList();
        }

        #endregion
    }
}
