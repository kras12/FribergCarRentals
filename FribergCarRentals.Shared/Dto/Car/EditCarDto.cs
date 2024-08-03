namespace FribergCarRentals.Shared.Dto.Car
{
    /// <summary>
    /// A DTO class to handle data used for editing a car. 
    /// </summary>
    public class EditCarDto : CarDtoBase
    {
        #region Properties

        /// <summary>
        /// A an optional collection of images to delete.
        /// </summary>
        public List<int>? DeleteImages { get; set; } = new();

        /// <summary>
        /// The ID of the selected category.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// The ID of the propulsion system for the car.
        /// </summary>
        public int PropulsionSystemId { get; set; }

        /// <summary>
        /// The ID of the rental status for the car.
        /// </summary>
        public int RentalStatusId { get; set; }

        #endregion
    }
}