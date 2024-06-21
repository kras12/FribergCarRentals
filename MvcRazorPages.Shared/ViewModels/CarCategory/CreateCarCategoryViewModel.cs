using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MvcRazorPages.Shared.Attributes;
using MvcRazorPages.Shared.ViewModels.Other;

namespace MvcRazorPages.Shared.ViewModels.CarCategory
{
    /// <summary>
    /// A view model class that handles data for a car category. 
    /// </summary>
    public class CreateCarCategoryViewModel : ViewModelBase
    {
        #region Constructors

        /// <summary>
        /// A constructor
        public CreateCarCategoryViewModel()
        {

        }

        /// <summary>
        /// A constructor
        /// </summary>
        /// <param name="categoryName">The name for the category.</param>
        public CreateCarCategoryViewModel(string categoryName)
        {
            #region Checks

            if (categoryName is null)
            {
                throw new ArgumentNullException(nameof(categoryName), $"The value of parameter '{nameof(categoryName)}' can't be null.");
            }

            #endregion

            CategoryName = categoryName;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The name for the category.
        /// </summary>
        [DisplayName("Category Name")]
        [Required]
        [StringLength(maximumLength: DefaultMaxCharacterInput, ErrorMessage = InputTooLongValidationMessage)]
        [ServerSideRegularExpression(LettersAndSpacesRegexPattern, ErrorMessage = OnlyLettersAndSpacesValidationMessage)]
        public string CategoryName { get; set; } = "";

        #endregion
    }
}
