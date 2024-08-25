using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FribergCarRentals.Shared.Constants;
using FribergCarRentals.Shared.Attributes;
using FribergCarRentals.Shared.Models.ViewModels.Other;

namespace FribergCarRentals.Shared.Models.ViewModels.Car
{
    /// <summary>
    /// A view model base class that handles car data. 
    /// </summary>
    public abstract class CarViewModelBase : ViewModelBase
    {
        #region Constructors

        /// <summary>
        /// A constructor. 
        /// </summary>
        protected CarViewModelBase()
        {

        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="brand">The brand for the car.</param>
        /// <param name="color">The color for the car.</param>
        /// <param name="model">The model for the car.</param>
        /// <param name="modelYear">The model year for the car.</param>
        /// <param name="propulsionSystem">The propulsion system for the car.</param>
        /// <param name="registrationNumber">The registration number for the car.</param>
        /// <param name="rentalCostPerDay">The rental cost per day.</param>
        /// <param name="rentalStatus">The rental status for the car.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        protected CarViewModelBase(string brand, string color, string model, int modelYear, 
            string registrationNumber, decimal rentalCostPerDay)
        {
            #region Checks            

            if (brand is null)
            {
                throw new ArgumentNullException(nameof(brand), $"The value of parameter '{brand}' can't be null");
            }

            if (color is null)
            {
                throw new ArgumentNullException(nameof(color), $"The value of parameter '{color}' can't be null");
            }

            if (model is null)
            {
                throw new ArgumentNullException(nameof(model), $"The value of parameter '{model}' can't be null");
            }

            if (modelYear < ValidationRules.CarModelYearMinimum || modelYear > ValidationRules.CarModelYearMaximum)
            {
                throw new ArgumentOutOfRangeException(nameof(modelYear), $"The value of parameter '{modelYear}' ({modelYear}) must fit in the interval of '{ValidationRules.CarModelYearMinimum}' and '{ValidationRules.CarModelYearMaximum}'.");
            }

            if (registrationNumber is null)
            {
                throw new ArgumentNullException(nameof(registrationNumber), $"The value of parameter '{registrationNumber}' can't be null");
            }

            if (RentalCostPerDay < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(RentalCostPerDay), $"The value of parameter '{RentalCostPerDay}' can't be negative.");
            }

            #endregion

            Brand = brand;
            Color = color;
            Model = model;
            ModelYear = modelYear;
            RegistrationNumber = registrationNumber;
            RentalCostPerDay = rentalCostPerDay;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The brand for the car.
        /// </summary>
        [DisplayName("Brand")]
        [Required(AllowEmptyStrings = false, ErrorMessage = ValidationMessages.MandatoryFieldValidationMessage)]
        [StringLength(maximumLength: ValidationRules.DefaultMaxCharacterInput, ErrorMessage = ValidationMessages.InputTooLongValidationMessage)]
        [ServerSideRegularExpression(ValidationRules.LettersNumbersAndSpacesRegexPattern, ErrorMessage = ValidationMessages.OnlyLettersNumbersAndSpacesValidationMessage)]
        public virtual string Brand { get; set; } = "";

        /// <summary>
        /// Returns a short description of the car (brand, model, and year).
        /// </summary>
        [DisplayName("Car")]
        public string CarInfo
        {
            get
            {
                return $"{Brand} {Model} {ModelYear} ";
            }
        }

        /// <summary>
        /// Returns a short description of the car (registration number - brand, model, and year).
        /// </summary>
        [DisplayName("Car")]
        public string CarInfoWithRegistrationNumber
        {
            get
            {
                return $"{RegistrationNumber} - {Brand} {Model} {ModelYear} ";
            }
        }

        /// <summary>
        /// The color for the car.
        /// </summary>
        [DisplayName("Color")]
        [Required(AllowEmptyStrings = false, ErrorMessage = ValidationMessages.MandatoryFieldValidationMessage)]
        [StringLength(maximumLength: ValidationRules.DefaultMaxCharacterInput, ErrorMessage = ValidationMessages.InputTooLongValidationMessage)]
        [ServerSideRegularExpression(ValidationRules.LettersAndSpacesRegexPattern, ErrorMessage = ValidationMessages.OnlyLettersAndSpacesValidationMessage)]
        public virtual string Color { get; set; } = "";

        /// <summary>
        /// The model for the car.
        /// </summary>
        [DisplayName("Model")]
        [Required(AllowEmptyStrings = false, ErrorMessage = ValidationMessages.MandatoryFieldValidationMessage)]
        [StringLength(maximumLength: ValidationRules.DefaultMaxCharacterInput, ErrorMessage = ValidationMessages.InputTooLongValidationMessage)]
        [ServerSideRegularExpression(ValidationRules.LettersNumbersAndSpacesRegexPattern, ErrorMessage = ValidationMessages.OnlyLettersNumbersAndSpacesValidationMessage)]
        public virtual string Model { get; set; } = "";

        /// <summary>
        /// The model year for the car.
        /// </summary>
        [DisplayName("Year")]
        [Required(AllowEmptyStrings = false, ErrorMessage = ValidationMessages.MandatoryFieldValidationMessage)]
        [Range(ValidationRules.CarModelYearMinimum, ValidationRules.CarModelYearMaximum)]
        public virtual int ModelYear { get; set; }

        /// <summary>
        /// The registration number for the car.
        /// </summary>
        [DisplayName("Reg Nr")]
        [Required(AllowEmptyStrings = false, ErrorMessage = ValidationMessages.MandatoryFieldValidationMessage)]
        [StringLength(maximumLength: 6, ErrorMessage = ValidationMessages.RegistrationNumberValidationMessage)]
        [RegularExpression(ValidationRules.RegistrationNumberRegexPattern, ErrorMessage = ValidationMessages.RegistrationNumberValidationMessage)]
        public virtual string RegistrationNumber { get; set; } = "";

        /// <summary>
        /// The rental cost per day.
        /// </summary>
        [DisplayName("Cost per day")]
        [Required(AllowEmptyStrings = false, ErrorMessage = ValidationMessages.MandatoryFieldValidationMessage)]
        [Range(ValidationRules.RentalCostPerDayMinimum, ValidationRules.RentalCostPerDayMaximum)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = ValidationRules.DefaultFloatNumberInputFormatString)]
        public virtual decimal RentalCostPerDay { get; set; }

        #endregion
    }
}