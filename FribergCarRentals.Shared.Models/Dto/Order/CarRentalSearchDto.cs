using FribergCarRentals.Shared.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Shared.Models.Dto.Order
{
    /// <summary>
    /// A DTO class for searching rentable cars.
    /// </summary>
    public class CarRentalSearchDto
    {
        #region Properties

        /// <summary>
        /// The car category filter to use when searching for cars. 
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = ValidationMessages.MandatoryFieldValidationMessage)]
        [Range(1, int.MaxValue)]
        [DefaultValue(1)]
        public int? SelectedCarCategoryFilter { get; set; } = null;

        /// <summary>
        /// The pickup date filter to use when searching for cars.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = ValidationMessages.MandatoryFieldValidationMessage)]
        public DateTime PickupDateLocalTime { get; set; }

        /// <summary>
        /// The return date filter to use when searching for cars.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = ValidationMessages.MandatoryFieldValidationMessage)]
        public DateTime ReturnDateLocalTime { get; set; }

        #endregion
    }
}
