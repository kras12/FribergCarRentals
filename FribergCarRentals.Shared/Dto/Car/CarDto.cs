using FribergCarRentals.Shared.Dto.Image;

namespace FribergCarRentals.Shared.Dto.Car
{
    /// <summary>
    /// A DTO class that represents a car.
    /// </summary>
    public class CarDto : CarDtoBase
    {
        #region Properties        

        /// <summary>
        /// The ID for the car.
        /// </summary>
        public int CarId { get; set; }

        /// <summary>
        /// The category for the car. 
        /// </summary>
        public CarCategoryDto Category { get; set; } = null!;

        /// <summary>
        /// A collection of images for the car.
        /// </summary>
        public List<CarImageDto> Images { get; set; } = new();

        /// <summary>
        /// The propulsion system for the car.
        /// </summary>
        public VehiclePropulsionDto PropulsionSystem { get; set; } = null!;

        /// <summary>
        /// The rental status for the car.
        /// </summary>
        public CarRentalStatusDto RentalStatus { get; set; } = null!;

        #endregion
    }
}