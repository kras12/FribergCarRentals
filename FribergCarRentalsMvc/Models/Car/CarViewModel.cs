using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.Models.Other;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Models.Car
{
    /// <summary>
    /// A view model class that handles car data.
    /// </summary>
    public class CarViewModel : CarViewModelBase
    {

        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="carEntity">The car to model.</param>
        public CarViewModel(CarEntity carEntity) :
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
        public CarViewModel(string brand, int carId, string color, string model, int modelYear, VehiclePropulsionEntity propulsionSystem,
            string registrationNumber, decimal rentalCostPerDay, CarRentalStatusEntity rentalStatus, List<ImageViewModel> images)
            : base(brand, color, model, modelYear, propulsionSystem, registrationNumber, rentalCostPerDay, rentalStatus)
        {
            #region Checks

            if (carId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(carId), $"The value of parameter '{carId}' can't be negative.");
            }

            if (images is null)
            {
                throw new ArgumentNullException(nameof(images), $"The value of parameter '{images}' can't be null.");
            }

            #endregion

            CarId = carId;
            Images = images;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The ID for the car.
        /// </summary>
        [DisplayName("Car ID")]
        [BindNever]
        public int CarId { get; }

        /// <summary>
        /// A collection of images for the car.
        /// </summary>
        [DisplayName("Images")]
        [BindNever]
        public List<ImageViewModel> Images { get; } = new();

        /// <summary>
        /// The rental cost per day.
        /// </summary>
        [DisplayName("Cost per day")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = DefaultPriceOutputFormatString)]
        [BindNever]
        public override decimal RentalCostPerDay { get; set; }

        #endregion
    }
}