using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FribergCars.Shared.SharedTypes;

namespace FribergCarRentals.Models
{
    /// <summary>
    /// A class that represents a car
    /// </summary>
    [Table("Cars")]
    public class CarEntity
    {
        #region Constants

        /// <summary>
        /// The oldest model year to support. Cars have been manufactured since late 1700. 
        /// </summary>
        private const int OldestModelYearSupported = 1700;

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor mainly intended for EF Core.
        /// </summary>
        /// <param name="carId">The id for the car. Can't be a negative value.</param>
        /// <param name="brand">The brand for the car.</param>
        /// <param name="color">The color for the car.</param>
        /// <param name="model">The model for the car.</param>
        /// <param name="modelYear">The model year for the car.</param>
        /// <param name="registrationNumber">The registration number for the car.</param>
        private CarEntity(int carId, string brand, string color, string model, int modelYear, string registrationNumber)
        {
            #region Checks

            if (carId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(carId), $"The value of parameter '{carId}' can't be a negative value.");
            }

            if (brand is null)
            {
                throw new ArgumentNullException(nameof(brand), $"The value of parameter '{brand}' can't be null");
            }

            if (color is null)
            {
                throw new ArgumentNullException(nameof(color), $"The value of parameter '{color}' can't be null");
            }

            if (model is null)
            {
                throw new ArgumentNullException(nameof(model), "The value of parameter '{model}' can't be null");
            }

            //  We may have to support the next year's model
            if (modelYear < OldestModelYearSupported || modelYear > DateTime.Now.Year + 1)
            {
                throw new ArgumentOutOfRangeException(nameof(modelYear), $"The model year must be between '{OldestModelYearSupported}' and '{DateTime.Now.Year + 1}'.");
            }

            if (registrationNumber is null)
            {
                throw new ArgumentNullException(nameof(registrationNumber), $"The value of parameter '{registrationNumber}' can't be null");
            }

            #endregion

            CarId = carId;
            Brand = brand;
            Color = color;
            Model = model;
            ModelYear = modelYear;
            RegistrationNumber = registrationNumber;

            // EF Core can't set navigational properties through a constructor, 
            // so these will be setby EF Core via the properties after the constructor have run. 
            PropulsionSystem = null!;
            RentalStatus = null!;
        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="brand">The brand for the car.</param>
        /// <param name="color">The color for the car.</param>
        /// <param name="model">The model for the car.</param>
        /// <param name="modelYear">The model year for the car.</param>
        /// <param name="registrationNumber">The registration number for the car.</param>
        /// <param name="propulsionSystem">The propulsion system for the car.</param>
        /// <param name="rentalStatus">The rental status for the car.</param>
        public CarEntity(string brand, string color, string model, int modelYear, string registrationNumber, 
            VehiclePropulsionEntity propulsionSystem, CarRentalStatusEntity rentalStatus) : 
            this(carId: 0, brand, color, model, modelYear, registrationNumber)
        {
            #region Checks

            if (propulsionSystem is null)
            {
                throw new ArgumentNullException(nameof(propulsionSystem), $"The value of parameter '{propulsionSystem}' can't be null");
            }

            if (rentalStatus is null)
            {
                throw new ArgumentNullException(nameof(rentalStatus), $"The value of parameter '{rentalStatus}' can't be null");
            }

            #endregion

            // EF Core can't set navigational properties through a constructor, 
            // so these values will have to be set in this constructor. 

            PropulsionSystem = propulsionSystem;
            RentalStatus = rentalStatus;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The brand for the car.
        /// </summary>
        public string Brand { get; set; } = "";

        /// <summary>
        /// The ID for the car.
        /// </summary>
        [Key]
        public int CarId { get; private set; }

        /// <summary>
        /// The color for the car.
        /// </summary>
        public string Color { get; set; } = "";

        /// <summary>
        /// The model for the car.
        /// </summary>
        public string Model { get; set; } = "";

        /// <summary>
        /// The model year for the car.
        /// </summary>
        public int ModelYear { get; set; }

        [Required]
        /// <summary>
        /// The registration number for the car.
        /// </summary>
        public string RegistrationNumber { get; set; } = "";

        /// <summary>
        /// A collection of images for the car.
        /// </summary>
        public List<ImageEntity> Images { get; set; } = new();

        /// <summary>
        /// The rental status for the car.
        /// </summary>
        public CarRentalStatusEntity RentalStatus { get; set; }

        /// <summary>
        /// The propulsion system for the car.
        /// </summary>
        public VehiclePropulsionEntity PropulsionSystem { get; set; }

        #endregion
    }
}