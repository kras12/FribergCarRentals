namespace FribergCarRentals.Shared.Dto.User
{
    /// <summary>
    /// A DTO class for user login responses.
    /// </summary>
    public class LoginUserResponseDto
    {
        #region Properties

        /// <summary>
        /// The email.
        /// </summary>
        public string Email { get; set; } = "";

        /// <summary>
        /// The JWT token.
        /// </summary>
        public string Token { get; set; } = "";

        #endregion
    }
}
