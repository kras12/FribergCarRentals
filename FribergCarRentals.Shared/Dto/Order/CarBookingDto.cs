using FribergCarRentals.Shared.Dto.Car;

namespace FribergCarRentals.Shared.Dto.Order
{
    /// <summary>
    /// A DTO class for car bookings.
    /// </summary>
    public class CarBookingDto
    {
        #region Properties

        /// <summary>
        /// The car that was rented.
        /// </summary>
        public CarDto Car { get; set; }

        /// <summary>
        /// The car booking ID.
        /// </summary>
        public int CarBookingId { get; set; }

        /// <summary>
        /// The order the booking belongs to. 
        /// </summary>
        public CarOrderDto? CarOrder { get; set; }

        /// <summary>
        /// The car pickup date in UTC time.
        /// </summary>
        public DateTime PickupDateUtc { get; set; }

        /// <summary>
        /// The rental cost per day.
        /// </summary>
        public decimal RentalCostPerDay { get; set; }

        /// <summary>
        /// The car return date in UTC time.
        /// </summary>
        public DateTime ReturnDateUtc { get; set; }

        #endregion
    }
}
