using FribergCarRentals.Shared.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Shared.Models.Dto.Car
{
    /// <summary>
    /// A DTO class to handle data used for editing a car. 
    /// </summary>
    public class EditCarDto : CarDtoBase
    {
        #region Properties

        /// <summary>
        /// A an optional collection of images to delete.
        /// </summary>
        [DefaultValue(null)]
        public List<int>? DeleteImages { get; set; } = new();

        /// <summary>
        /// The ID of the selected category.
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.MandatoryFieldValidationMessage)]
        [Range(1, int.MaxValue)]
        [DefaultValue(1)]
        public int CategoryId { get; set; }

        /// <summary>
        /// The ID of the propulsion system for the car.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = ValidationMessages.MandatoryFieldValidationMessage)]
        [Range(1, int.MaxValue)]
        [DefaultValue(1)]
        public int PropulsionSystemId { get; set; }

        /// <summary>
        /// The ID of the rental status for the car.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = ValidationMessages.MandatoryFieldValidationMessage)]
        [Range(1, int.MaxValue)]
        [DefaultValue(1)]
        public int RentalStatusId { get; set; }

        #endregion
    }
}