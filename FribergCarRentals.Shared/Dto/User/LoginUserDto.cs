using FribergCarRentals.Shared.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
        [Required(AllowEmptyStrings = false)]
        [StringLength(maximumLength: ValidationRules.DefaultMaxCharacterInput, ErrorMessage = ValidationMessages.InputTooLongValidationMessage)]
        [EmailAddress]
        [RegularExpression(ValidationRules.EmailRegexPattern, ErrorMessage = ValidationMessages.EmailInputValidationMessage)]
        [DefaultValue("kalle@ankeborg.com")]
        public string Email { get; set; } = "";

        /// <summary>
        /// The password for the user.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [StringLength(maximumLength: ValidationRules.PasswordLengthMaximum, MinimumLength = ValidationRules.PasswordLengthMinium, ErrorMessage = ValidationMessages.PasswordLengthValidationMessage)]
        [DefaultValue("Aa1!123456789")]
        public string Password { get; set; } = "";

        #endregion
    }
}
