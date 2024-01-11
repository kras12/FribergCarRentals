using System.ComponentModel.DataAnnotations;
using FribergCars.Shared.SharedTypes;

namespace FribergCarRentals.Models
{
    /// <summary>
    /// A class that represents a car
    /// </summary>
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
        /// <param name="propulsionSystem">The propulsion system for the car.</param>
        private CarEntity(int carId, string brand, string color, string model, int modelYear, string registrationNumber, CarPropulsionSystem propulsionSystem)
        {
            #region Checks

            if (carId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(carId), "The ID can't be a negative value.");
            }

            if (brand is null)
            {
                throw new ArgumentNullException("The brand can't be null", nameof(brand));
            }

            if (color is null)
            {
                throw new ArgumentNullException("The color can't be null", nameof(color));
            }

            if (model is null)
            {
                throw new ArgumentNullException("The model can't be null", nameof(model));
            }

            //  We may have to support the next year's model
            if (modelYear < OldestModelYearSupported || modelYear > DateTime.Now.Year + 1)
            {
                throw new ArgumentOutOfRangeException(nameof(modelYear), $"The model year must be between '{OldestModelYearSupported}' and '{DateTime.Now.Year + 1}'.");
            }

            if (registrationNumber is null)
            {
                throw new ArgumentNullException("The registration number can't be null", nameof(registrationNumber));
            }

            #endregion

            CarId = carId;
            Brand = brand;
            Color = color;
            Model = model;
            ModelYear = modelYear;
            RegistrationNumber = registrationNumber;
            PropulsionSystem = propulsionSystem;
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
        public CarEntity(string brand, string color, string model, int modelYear, string registrationNumber, CarPropulsionSystem propulsionSystem) : 
            this(carId: 0, brand, color, model, modelYear, registrationNumber, propulsionSystem)
        {

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
        /// The propulsion system for the car.
        /// </summary>
        public CarPropulsionSystem PropulsionSystem {  get; set; } = CarPropulsionSystem.None;

        /// <summary>
        /// A collection of images for the car.
        /// </summary>
        public List<ImageEntity> Images { get; set; } = new List<ImageEntity>();

        #endregion
    }
}
