using FribergCarRentals.Shared.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Shared.Models.Dto.Car
{
    /// <summary>
    /// A DTO base class that handles car data. 
    /// </summary>
    public abstract class CarDtoBase
    {
        #region Properties

        /// <summary>
        /// The brand for the car.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = ValidationMessages.MandatoryFieldValidationMessage)]
        [StringLength(maximumLength: ValidationRules.DefaultMaxCharacterInput, ErrorMessage = ValidationMessages.InputTooLongValidationMessage)]
        [RegularExpression(ValidationRules.LettersNumbersAndSpacesRegexPattern, ErrorMessage = ValidationMessages.OnlyLettersNumbersAndSpacesValidationMessage)]
        [DefaultValue("Tesla")]
        public string Brand { get; set; } = "";

        /// <summary>
        /// The color for the car.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = ValidationMessages.MandatoryFieldValidationMessage)]
        [StringLength(maximumLength: ValidationRules.DefaultMaxCharacterInput, ErrorMessage = ValidationMessages.InputTooLongValidationMessage)]
        [RegularExpression(ValidationRules.LettersAndSpacesRegexPattern, ErrorMessage = ValidationMessages.OnlyLettersAndSpacesValidationMessage)]
        [DefaultValue("Red")]
        public string Color { get; set; } = "";

        /// <summary>
        /// The model for the car.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = ValidationMessages.MandatoryFieldValidationMessage)]
        [StringLength(maximumLength: ValidationRules.DefaultMaxCharacterInput, ErrorMessage = ValidationMessages.InputTooLongValidationMessage)]
        [RegularExpression(ValidationRules.LettersNumbersAndSpacesRegexPattern, ErrorMessage = ValidationMessages.OnlyLettersNumbersAndSpacesValidationMessage)]
        [DefaultValue("Model S")]
        public string Model { get; set; } = "";

        /// <summary>
        /// The model year for the car.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = ValidationMessages.MandatoryFieldValidationMessage)]
        [Range(ValidationRules.CarModelYearMinimum, ValidationRules.CarModelYearMaximum)]
        [DefaultValue(2024)]
        public int ModelYear { get; set; }

        /// <summary>
        /// The registration number for the car.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = ValidationMessages.MandatoryFieldValidationMessage)]
        [StringLength(maximumLength: 6, ErrorMessage = ValidationMessages.RegistrationNumberValidationMessage)]
        [RegularExpression(ValidationRules.RegistrationNumberRegexPattern, ErrorMessage = ValidationMessages.RegistrationNumberValidationMessage)]
        [DefaultValue("ABC123")]
        public string RegistrationNumber { get; set; } = "";

        /// <summary>
        /// The rental cost per day.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = ValidationMessages.MandatoryFieldValidationMessage)]
        [Range(ValidationRules.RentalCostPerDayMinimum, ValidationRules.RentalCostPerDayMaximum)]
        [DefaultValue(2500)]
        public decimal RentalCostPerDay { get; set; }

        #endregion
    }
}