using FribergCarRentals.Shared.Constants;
using FribergCarRentals.Shared.Models.ViewModels.CarCategory;
using FribergCarRentals.Shared.Models.ViewModels.Image;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Shared.Models.ViewModels.Car
{
    /// <summary>
    /// A view model class that handles car data.
    /// </summary>
    public class CarViewModel : CarViewModelBase
    {
        #region Properties

        /// <summary>
        /// The ID for the car.
        /// </summary>
        [DisplayName("Car ID")]
        public int CarId { get; }

        /// <summary>
        /// The category for the car.
        /// </summary>
        [DisplayName("Category")]
        public virtual CarCategoryViewModel Category { get; set; }

        /// <summary>
        /// A collection of images for the car.
        /// </summary>
        [DisplayName("Images")]
        public List<ImageViewModel> Images { get; set; } = new();

        /// <summary>
        /// The propulsion system for the car.
        /// </summary>
        [DisplayName("Propulsion")]
        [Required(AllowEmptyStrings = false, ErrorMessage = ValidationMessages.MandatoryFieldValidationMessage)]
        public virtual VehiclePropulsionViewModel PropulsionSystem { get; set; } = new();

        /// <summary>
        /// The rental cost per day.
        /// </summary>
        [DisplayName("Cost per day")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = ValidationRules.DefaultPriceOutputFormatString)]
        public override decimal RentalCostPerDay { get; set; }

        /// <summary>
        /// The rental status for the car.
        /// </summary>
        [DisplayName("Status")]
        [Required(AllowEmptyStrings = false, ErrorMessage = ValidationMessages.MandatoryFieldValidationMessage)]
        public virtual CarRentalStatusViewModel RentalStatus { get; set; } = new();

        #endregion
    }
}