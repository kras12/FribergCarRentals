using FribergCarRentals.Shared.Dto.Api;

namespace FribergCarRentalsApi.Dto
{
    /// <summary>
    /// API empty response class for Friberg Fastigheter APIs.
    /// </summary>
    public class MvcApiEmptyResponseDto : ApiResponseDto<object>
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public MvcApiEmptyResponseDto() : base(value: null)
        {

        }

        #endregion
    }
}
