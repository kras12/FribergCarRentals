using FribergCarRentals.Shared.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Shared.Models.Dto.User
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
        [DataType(DataType.Password)]
        [StringLength(maximumLength: ValidationRules.PasswordLengthMaximum, MinimumLength = ValidationRules.PasswordLengthMinium, ErrorMessage = ValidationMessages.PasswordLengthValidationMessage)]
        [DefaultValue("Aa1!123456789")]
        public string Password { get; set; } = "";

        #endregion
    }
}
