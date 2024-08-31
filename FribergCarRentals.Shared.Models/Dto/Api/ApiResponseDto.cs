using FribergCarRentals.Shared.Enums;
using System.Text;
using System.Text.Json.Serialization;

namespace FribergCarRentals.Shared.Models.Dto.Api
{
    /// <summary>
    /// Basic API response class. 
    /// </summary>
    public class ApiResponseDto
    {
        #region Constructors

        /// <summary>
        /// A constructor intended for JSON deserializers.
        /// </summary>
        [JsonConstructor]
        public ApiResponseDto()
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="success">True if the operation was successful.</param>
        protected ApiResponseDto(bool success)
        {
            Success = success;
        }

        /// <summary>
        /// Constructor for an error response.
        /// </summary>
        /// <param name="errors">A collection of errors.</param>
        protected ApiResponseDto(List<ApiErrorDto> errors)
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
            : this(new List<ApiErrorDto>() { new ApiErrorDto(errorType, errorMessage) })
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// A collection of errors. 
        /// </summary>
        [JsonInclude]
        public List<ApiErrorDto> Errors { get; private set; } = new();

        /// <summary>
        /// True if operation was successful.
        /// </summary>
        [JsonInclude]
        public bool Success { get; private set; }

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
            return CreateErrorResponse(new List<ApiErrorDto> { new ApiErrorDto(errorType, errorMessage) });
        }

        /// <summary>
        /// Creates an error response.
        /// </summary>
        /// <param name="errors">A collection of errors having labels and descriptions.</param>
        /// <returns><see cref="ApiResponseDto"/> containing the supplied errors.</returns>
        public static ApiResponseDto CreateErrorResponse(List<ApiErrorDto> errors)
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
            return Errors.Select(x => x.ErrorMessage).ToList();
        }

        /// <summary>
        /// Formats the error collection as a string.
        /// </summary>
        /// <returns>A <see cref="string"/>.</returns>
        public string GetErrorsAsString()
        {
            var stringBuilder = new StringBuilder();
            Errors.ForEach(x => stringBuilder.AppendLine($"{x.ErrorType}: {x.ErrorMessage}"));
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Formats the error collection as a list.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of <see cref="string"/>.</returns>
        public List<string> GetErrorsAsList()
        {
            return Errors.Select(x => $"{x.ErrorType}: {x.ErrorMessage}").ToList();
        }

        #endregion
    }
}
