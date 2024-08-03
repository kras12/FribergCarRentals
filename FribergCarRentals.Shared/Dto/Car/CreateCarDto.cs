namespace FribergCarRentals.Shared.Dto.Car
{
    /// <summary>
    ///  A DTO class that handles data for creating a new car.
    /// </summary>
    public class CreateCarDto
    {
        #region Properties

        /// <summary>
        /// The brand for the car.
        /// </summary>
        public string Brand { get; set; } = "";

        /// <summary>
        /// The ID of the selected category.
        /// </summary>
        public int CategoryId { get; set; }

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

        /// <summary>
        /// The propulsion system for the car.
        /// </summary>
        public int PropulsionSystemId { get; set; }

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
        public int RentalStatusId { get; set; }

        #endregion
    }
}