using MvcRazorPages.Shared.Attributes;
using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.Models.Other;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentalsBravo.Models.CarCategory
{
    /// <summary>
    /// A view model class that handles data for editing a car category.
    /// </summary>
    public class EditCarCategoryViewModel : ViewModelBase
    {
        #region Constructors

        /// <summary>
        /// A constructor
        public EditCarCategoryViewModel()
        {
            
        }

        /// <summary>
        /// A constructor
        /// </summary>
        /// <param name="category">The car category to model.</param>
        public EditCarCategoryViewModel(CarCategoryEntity category)
        {
            #region Checks

            if (category is null)
            {
                throw new ArgumentNullException(nameof(category), $"The value of parameter '{nameof(category)}' can't be null.");  
            }

            if (category.CarCategoryId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(category.CarCategoryId), $"The value of property '{nameof(category.CarCategoryId)}' can't be negative.");
            }

            #endregion

            CarCategoryId = category.CarCategoryId;
            CategoryName = category.CategoryName;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The ID for the car category.
        /// </summary>
        [DisplayName("Category ID")]
        [Required]
        public int CarCategoryId { get; set; }

        /// <summary>
        /// The filename for the category.
        /// </summary>
        [DisplayName("Category Name")]
        [Required]
        [StringLength(maximumLength: DefaultMaxCharacterInput, ErrorMessage = InputTooLongValidationMessage)]
        [ServerSideRegularExpression(LettersAndSpacesRegexPattern, ErrorMessage = OnlyLettersAndSpacesValidationMessage)]
        public string CategoryName { get; set; } = "";

        #endregion
    }
}
