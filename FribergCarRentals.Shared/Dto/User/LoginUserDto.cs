namespace FribergCarRentals.Shared.Dto.User
{
    /// <summary>
    /// A DTO base class for logging in users.
    /// </summary>
    public abstract class LoginUserDto
    {
        #region Properties

        /// <summary>
        /// The email address for the user.
        /// </summary>
        public string Email { get; set; } = "";

        /// <summary>
        /// The password for the user.
        /// </summary>
        public string Password { get; set; } = "";

        #endregion
    }
}
