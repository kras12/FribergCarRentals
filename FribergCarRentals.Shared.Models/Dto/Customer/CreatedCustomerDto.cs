using FribergCarRentals.Shared.Models.Dto.User;

namespace FribergCarRentals.Shared.Models.Dto.Customer
{
    /// <summary>
    /// A DTO class created customers.
    /// </summary>
    public class CreatedCustomerDto : UserDto
    {
        #region Properties

        /// <summary>
        /// Contains the information needed to confirm the email account.
        /// </summary>
        public ConfirmEmailDto? ConfirmEmailData { get; set; } = null;

        /// <summary>
        /// The JWT token.
        /// </summary>
        public string? Token { get; set; } = null;

        #endregion
    }
}
