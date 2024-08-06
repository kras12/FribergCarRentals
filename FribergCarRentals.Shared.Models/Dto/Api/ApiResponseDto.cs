using System.Text;

namespace FribergCarRentals.Shared.Models.Dto.Api
{
    /// <summary>
    /// Basic API response class. 
    /// </summary>
    public class ApiResponseDto
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="success">True if the operation was successful.</param>
        public ApiResponseDto(bool success = false)
        {
            Success = success;
        }

        /// <summary>
        /// Constructor for an error response.
        /// </summary>
        /// <param name="errors">A collection of errors.</param>
        protected ApiResponseDto(List<KeyValuePair<string, string>> errors)
        {
            Errors = errors;
            Success = false;
        }

        /// <summary>
        /// Constructor for an error response.
        /// </summary>
        /// <param name="errorType">The type of error.</param>
        /// <param name="errorMessage">The error message.</param>
        protected ApiResponseDto(string errorType, string errorMessage)
            : this(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>(errorType, errorMessage) })
        {

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

        #endregion

        #region FactoryMethods

        /// <summary>
        /// Creates an error response.
        /// </summary>
        /// <param name="errorType">The type of error.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns><see cref="ApiResponseDto"/> containing the supplied errors.</returns>
        public static ApiResponseDto CreateErrorResponse(ApiErrorMessageTypes errorType, string errorMessage)
        {
            return CreateErrorResponse(errorType.ToString(), errorMessage);
        }

        /// <summary>
        /// Creates an error response.
        /// </summary>
        /// <param name="errorType">The type of error.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns><see cref="ApiResponseDto"/> containing the supplied errors.</returns>
        public static ApiResponseDto CreateErrorResponse(string errorType, string errorMessage)
        {
            return CreateErrorResponse(new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>(errorType, errorMessage) });
        }

        /// <summary>
        /// Creates an error response.
        /// </summary>
        /// <param name="errors">A collection of errors having labels and descriptions.</param>
        /// <returns><see cref="ApiResponseDto"/> containing the supplied errors.</returns>
        public static ApiResponseDto CreateErrorResponse(List<KeyValuePair<string, string>> errors)
        {
            return new ApiResponseDto(errors);
        }

        /// <summary>
        /// Creates a successful response having a value of type <see cref="T"/>.
        /// </summary>
        /// <returns><see cref="ApiResponseDto"/> containing the supplied response value.</returns>
        public static ApiResponseDto CreateSuccessfulResponse()
        {
            return new ApiResponseDto(success: true);
        }

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
