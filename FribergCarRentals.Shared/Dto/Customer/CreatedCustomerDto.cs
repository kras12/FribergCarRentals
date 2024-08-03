using FribergCarRentals.Shared.Dto.User;

namespace FribergCarRentals.Shared.Dto.Customer
{
    /// <summary>
    /// A DTO class created customers.
    /// </summary>
    public class CreatedCustomerDto
    {
        #region Properties

        /// <summary>
        /// The link to confirm the email account.
        /// </summary>
        public string? ConfirmEmailLink { get; set; } = null;

        /// <summary>
		/// The user associated with the admin.
		/// </summary>
        public required UserDto User { get; set; }

        /// <summary>
        /// The JWT token.
        /// </summary>
        public string? Token { get; set; } = null;

        #endregion
    }
}
