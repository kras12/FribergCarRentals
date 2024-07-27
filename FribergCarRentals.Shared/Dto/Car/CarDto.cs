using FribergCarRentals.Shared.Dto.Image;

namespace FribergCarRentals.Shared.Dto.Car
{
    /// <summary>
    /// A DTO class that represents a car.
    /// </summary>
    public class CarDto
    {
        #region Properties

        /// <summary>
        /// The brand for the car.
        /// </summary>
        public string Brand { get; set; } = "";

        /// <summary>
        /// The category for the car. 
        /// </summary>
        public CarCategoryDto Category { get; set; }

        /// <summary>
        /// The ID for the car.
        /// </summary>
        public int CarId { get; set; }

        /// <summary>
        /// The color for the car.
        /// </summary>
        public string Color { get; set; } = "";

        /// <summary>
        /// A collection of images for the car.
        /// </summary>
        public List<CarImageDto> Images { get; set; } = new();

        /// <summary>
        /// The model for the car.
        /// </summary>
        public string Model { get; set; } = "";

        /// <summary>
        /// The model year for the car.
        /// </summary>
        public int ModelYear { get; set; }

        /// <summary>
        /// The propulsion system for the car.
        /// </summary>
        public VehiclePropulsionDto PropulsionSystem { get; set; }

        /// <summary>
        /// The registration number for the car.
        /// </summary>
        public string RegistrationNumber { get; set; } = "";

        /// <summary>
        /// The rental cost per day.
        /// </summary>
        public decimal RentalCostPerDay { get; set; }

        /// <summary>
        /// The rental status for the car.
        /// </summary>
        public CarRentalStatusDto RentalStatus { get; set; }

        #endregion
    }
}