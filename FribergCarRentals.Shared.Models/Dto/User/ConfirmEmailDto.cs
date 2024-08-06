namespace FribergCarRentals.Shared.Models.Dto.User
{
    /// <summary>
    /// A DTO class for email confirmation.
    /// </summary>
    public class ConfirmEmailDto
    {
        #region Properties

        /// <summary>
        /// The confirmation code sent to the client.
        /// </summary>
        public string Code { get; set; } = "";

        /// <summary>
        /// The email of the user.
        /// </summary>
        public string Email { get; set; } = "";

        #endregion
    }
}
