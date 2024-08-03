using FribergCarRentals.Shared.Constants;
using System.ComponentModel.DataAnnotations;

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
        [EmailAddress]
        [Required(AllowEmptyStrings = false, ErrorMessage = ValidationMessages.MandatoryFieldValidationMessage)]
        [StringLength(maximumLength: ValidationRules.DefaultMaxCharacterInput, ErrorMessage = ValidationMessages.InputTooLongValidationMessage)]
        [RegularExpression(ValidationRules.EmailRegexPattern, ErrorMessage = ValidationMessages.EmailInputValidationMessage)]
        public string Email { get; set; } = "";

        /// <summary>
        /// The first name of the user.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = ValidationMessages.MandatoryFieldValidationMessage)]
        [StringLength(maximumLength: ValidationRules.DefaultMaxCharacterInput, ErrorMessage = ValidationMessages.InputTooLongValidationMessage)]
        [RegularExpression(ValidationRules.LettersAndSpacesRegexPattern, ErrorMessage = ValidationMessages.OnlyLettersAndSpacesValidationMessage)]
        public string FirstName { get; set; } = "";

        /// <summary>
        /// The last name of the user.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = ValidationMessages.MandatoryFieldValidationMessage)]
        [StringLength(maximumLength: ValidationRules.DefaultMaxCharacterInput, ErrorMessage = ValidationMessages.InputTooLongValidationMessage)]
        [RegularExpression(ValidationRules.LettersAndSpacesRegexPattern, ErrorMessage = ValidationMessages.OnlyLettersAndSpacesValidationMessage)]
        public string LastName { get; set; } = "";

        #endregion

    }
}
