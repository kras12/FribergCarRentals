using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FribergCarRentals.Shared.Constants;
using FribergCarRentals.Shared.Attributes;
using FribergCarRentals.Shared.Models.ViewModels.Other;

namespace FribergCarRentals.Shared.Models.ViewModels.User
{
    /// <summary>
    /// A viewmodel base class that handles data related to users. 
    /// </summary>
    /// <remarks>This class acts like a base class for view models of all types as it supports model binding on its properties.</remarks>
    public abstract class UserViewModelBase : ViewModelBase
    {
        #region Properties

        /// <summary>
        /// The email address for the user.
        /// </summary>
        [DisplayName("Email")]
        [EmailAddress]
        [Required(AllowEmptyStrings = false, ErrorMessage = ValidationMessages.MandatoryFieldValidationMessage)]
        [StringLength(maximumLength: ValidationRules.DefaultMaxCharacterInput, ErrorMessage = ValidationMessages.InputTooLongValidationMessage)]
        [ServerSideRegularExpression(ValidationRules.EmailRegexPattern, ErrorMessage = ValidationMessages.EmailInputValidationMessage)]
        public virtual string Email { get; set; } = "";

        /// <summary>
        /// The first name for the user.
        /// </summary>
        [DisplayName("First Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = ValidationMessages.MandatoryFieldValidationMessage)]
        [StringLength(maximumLength: ValidationRules.DefaultMaxCharacterInput, ErrorMessage = ValidationMessages.InputTooLongValidationMessage)]
        [ServerSideRegularExpression(ValidationRules.LettersAndSpacesRegexPattern, ErrorMessage = ValidationMessages.OnlyLettersAndSpacesValidationMessage)]
        public virtual string FirstName { get; set; } = "";

        /// <summary>
        /// The full name for the user.
        /// </summary>
        [DisplayName("Full Name")]
        public virtual string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        /// <summary>
        /// The last name for the user.
        /// </summary>
        [DisplayName("Last Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = ValidationMessages.MandatoryFieldValidationMessage)]
        [StringLength(maximumLength: ValidationRules.DefaultMaxCharacterInput, ErrorMessage = ValidationMessages.InputTooLongValidationMessage)]
        [ServerSideRegularExpression(ValidationRules.LettersAndSpacesRegexPattern, ErrorMessage = ValidationMessages.OnlyLettersAndSpacesValidationMessage)]
        public virtual string LastName { get; set; } = "";

        #endregion
    }
}
