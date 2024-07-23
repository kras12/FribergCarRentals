namespace FribergCarRentals.Shared.Dto.Api
{
    /// <summary>
    /// Wraps a value type inside a class. Designed to be used as a value with the class <see cref="ApiResponseDto{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResponseValueTypeWrapperDto<T> where T : struct, IFormattable
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value">The value for a successful response.</param>
        public ApiResponseValueTypeWrapperDto(T value)
        {
            Value = value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The value for a successful response.
        /// </summary>
        public T Value { get; protected set; }

        #endregion
    }
}
