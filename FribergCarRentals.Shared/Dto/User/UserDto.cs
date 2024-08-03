namespace FribergCarRentals.Shared.Dto.User
{
    /// <summary>
    /// A DTO class for application users.
    /// </summary>
    public class UserDto
    {
        #region Properties

        /// <summary>
        /// The email of the user.
        /// </summary>
        public string Email { get; set; } = "";

        /// <summary>
        /// The first name of the user.
        /// </summary>
        public string FirstName { get; set; } = "";        

        /// <summary>
        /// The last name of the user.
        /// </summary>
        public string LastName { get; set; } = "";

        #endregion

    }
}
