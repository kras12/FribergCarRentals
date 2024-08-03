namespace FribergCarRentals.Shared.Dto.User
{
    /// <summary>
    /// A DTO base class for creating users. 
    /// </summary>
    public abstract class CreateUserDto : UserDto
    {
        #region Properties

        /// <summary>
        /// The password for the user.
        /// </summary>
        public string NewPassword { get; set; } = "";

        #endregion
    }
}
