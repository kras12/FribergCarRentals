using FribergCarRentals.Shared.Dto.Car;

namespace FribergCarRentals.Shared.Dto.Order
{
    /// <summary>
    /// A DTO class for searching rentable cars.
    /// </summary>
    public class CarRentalSearchDto
    {
        #region Properties

        /// <summary>
        /// The car category filter to use when searching for cars. 
        /// </summary>
        public int? SelectedCarCategoryFilter { get; set; } = null;

        /// <summary>
        /// The pickup date filter to use when searching for cars.
        /// </summary>
        public DateTime PickupDateLocalTime { get; set; }

        /// <summary>
        /// The return date filter to use when searching for cars.
        /// </summary>
        public DateTime ReturnDateLocalTime { get; set; }

        #endregion
    }
}
