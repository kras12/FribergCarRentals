using FribergCarRentals.Shared.Dto.Api;

namespace FribergCarRentalsApi.Dto
{
    /// <summary>
    /// API value response class for Friberg Fastigheter APIs.
    /// </summary>
    public class MvcApiValueResponseDto<T> : ApiResponseDto<T> where T : class
    {
        #region Constructors

        /// <summary>
        /// Constructor for a successful response.
        /// </summary>
        /// <param name="value">The value to send in the response body.</param>
        public MvcApiValueResponseDto(T value = null!)
        {
            Value = value;
            Success = true;
        }

        #endregion
    }
}
