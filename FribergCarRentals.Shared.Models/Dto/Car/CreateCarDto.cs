using FribergCarRentals.Shared.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Shared.Models.Dto.Car
{
    /// <summary>
    ///  A DTO class that handles data for creating a new car.
    /// </summary>
    public class CreateCarDto : CarDtoBase
    {
        #region Properties       

        /// <summary>
        /// The ID of the selected category.
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.MandatoryFieldValidationMessage)]
        [Range(1, int.MaxValue)]
        [DefaultValue(1)]
        public int CategoryId { get; set; }

        /// <summary>
        /// The propulsion system for the car.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = ValidationMessages.MandatoryFieldValidationMessage)]
        [Range(1, int.MaxValue)]
        [DefaultValue(1)]
        public int PropulsionSystemId { get; set; }

        /// <summary>
        /// The rental status for the car.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = ValidationMessages.MandatoryFieldValidationMessage)]
        [Range(1, int.MaxValue)]
        [DefaultValue(1)]
        public int RentalStatusId { get; set; }

        #endregion
    }
}