using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FribergCarRentals.Shared.Attributes;
using FribergCarRentals.Shared.Constants;
using FribergCarRentals.Shared.Models.ViewModels.Other;

namespace FribergCarRentals.Shared.Models.ViewModels.CarCategory
{
    /// <summary>
    /// A view model class that handles data for a car category. 
    /// </summary>
    public class CreateCarCategoryViewModel : ViewModelBase
    {
        #region Properties

        /// <summary>
        /// The name for the category.
        /// </summary>
        [DisplayName("Category Name")]
        [Required]
        [StringLength(maximumLength: ValidationRules.DefaultMaxCharacterInput, ErrorMessage = ValidationMessages.InputTooLongValidationMessage)]
        [ServerSideRegularExpression(ValidationRules.LettersAndSpacesRegexPattern, ErrorMessage = ValidationMessages.OnlyLettersAndSpacesValidationMessage)]
        public string CategoryName { get; set; } = "";

        #endregion
    }
}
