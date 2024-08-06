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
        #region Constructors

        public CarViewModel()
        {

        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="carId">The ID for the car.</param>
        /// <param name="category">The category for the car.</param>
        /// <param name="brand">The brand for the car.</param>
        /// <param name="color">The color for the car.</param>
        /// <param name="model">The model for the car.</param>
        /// <param name="modelYear">The model year for the car.</param>
        /// <param name="propulsionSystem">The propulsion system for the car.</param>
        /// <param name="registrationNumber">The registration number for the car.</param>
        /// <param name="rentalCostPerDay">The rental cost per day.</param>
        /// <param name="rentalStatus">The rental status for the car.</param>
        /// <param name="images">The images for the car.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public CarViewModel(int carId, CarCategoryViewModel category, string brand, string color, string model, int modelYear, VehiclePropulsionViewModel propulsionSystem,
            string registrationNumber, decimal rentalCostPerDay, CarRentalStatusViewModel rentalStatus, List<ImageViewModel>? images = null)
            : base(brand, color, model, modelYear, propulsionSystem, registrationNumber, rentalCostPerDay, rentalStatus)
        {
            CarId = carId;
            Category = category;

            if (images != null)
            {
                Images = images;
            }
        }

        #endregion

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
        /// The rental cost per day.
        /// </summary>
        [DisplayName("Cost per day")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = ValidationRules.DefaultPriceOutputFormatString)]
        public override decimal RentalCostPerDay { get; set; }

        #endregion
    }
}