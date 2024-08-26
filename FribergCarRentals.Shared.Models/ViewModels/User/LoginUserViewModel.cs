using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FribergCarRentals.Shared.Attributes;
using FribergCarRentals.Shared.Constants;
using FribergCarRentals.Shared.Models.ViewModels.Other;

namespace FribergCarRentals.Shared.Models.ViewModels.User
{
    /// <summary>
    /// A viewmodel base class that handles data related to user login. 
    /// </summary>
    public abstract class LoginUserViewModel : ViewModelBase
    {
        #region Properties

        /// <summary>
        /// The email address for the user.
        /// </summary>
        [DisplayName("Email")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(maximumLength: ValidationRules.DefaultMaxCharacterInput, ErrorMessage = ValidationMessages.InputTooLongValidationMessage)]
        [EmailAddress]
        [ServerSideRegularExpression(ValidationRules.EmailRegexPattern, ErrorMessage = ValidationMessages.EmailInputValidationMessage)]
        public string Email { get; set; } = "";

        /// <summary>
        /// The password for the user.
        /// </summary>
        [DisplayName("Password")]
        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [StringLength(maximumLength: ValidationRules.PasswordLengthMaximum, MinimumLength = ValidationRules.PasswordLengthMinium, ErrorMessage = ValidationMessages.PasswordLengthValidationMessage)]
        public string Password { get; set; } = "";

        #endregion
    }
}
