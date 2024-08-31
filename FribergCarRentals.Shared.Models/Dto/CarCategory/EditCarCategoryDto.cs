using FribergCarRentals.Shared.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Shared.Models.Dto.CarCategory
{
    /// <summary>
    /// A DTO class that handles data for editing a car category.
    /// </summary>
    public class EditCarCategoryDto
    {
        #region Properties

        /// <summary>
        /// The name for the category.
        /// </summary>
        [Required]
        [StringLength(maximumLength: ValidationRules.DefaultMaxCharacterInput, ErrorMessage = ValidationMessages.InputTooLongValidationMessage)]
        [RegularExpression(ValidationRules.LettersAndSpacesRegexPattern, ErrorMessage = ValidationMessages.OnlyLettersAndSpacesValidationMessage)]
        [DefaultValue("SUV")]
        public string CategoryName { get; set; } = "";

        #endregion
    }
}
