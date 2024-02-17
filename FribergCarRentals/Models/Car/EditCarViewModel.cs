using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.Models.Other;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Models.Car
{
    /// <summary>
    /// A view model class to handle data used for editing a car. 
    /// </summary>
    public class EditCarViewModel : CarViewModelBase
    {

        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public EditCarViewModel() : base()
        {

        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="carEntity">The car to model.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public EditCarViewModel(CarEntity carEntity) : 
            this(carEntity.Brand, carEntity.CarId, carEntity.Color, carEntity.Model, carEntity.ModelYear, 
                carEntity.PropulsionSystem!, carEntity.RegistrationNumber, carEntity.RentalCostPerDay, 
                carEntity.RentalStatus!, carEntity.Images.Select(x => new ImageViewModel(x)).ToList())
        {

        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="brand">The brand for the car.</param>
        /// <param name="carId">The ID for the car.</param>
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
        public EditCarViewModel(string brand, int carId, string color, string model, int modelYear, VehiclePropulsionEntity propulsionSystem,
            string registrationNumber, decimal rentalCostPerDay, CarRentalStatusEntity rentalStatus, List<ImageViewModel> images)
            : base(brand, color, model, modelYear, propulsionSystem, registrationNumber, rentalCostPerDay, rentalStatus)
        {
            #region Checks

            if (carId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(carId), $"The value of parameter '{carId}' can't be negative.");
            }

            #endregion

            CarId = carId;
            Images = images;

            PageSubTitle = $"#{CarId} - {CarInfo}";
        }

        #endregion

        #region Properties

        /// <summary>
        /// The ID for the car.
        /// </summary>
        [DisplayName("ID")]
        [Required]
        public int CarId { get; set; }

        /// <summary>
        /// The images to delete.
        /// </summary>
        [DisplayName("Delete Images")]
        public List<int>? DeleteImages { get; set; } = new();

        /// <summary>
        /// A collection of images for the car.
        /// </summary>
        [DisplayName("Images")]
        [BindNever]
        public List<ImageViewModel> Images { get; set;  } = new();

        /// <summary>
        /// The images to upload
        /// </summary>
        [DisplayName("Upload Images")]
        public List<IFormFile>? UploadImages { get; set; } = null;

        #endregion

    }
}