namespace FribergCarRentals.Shared.Models.Dto.Order
{
    /// <summary>
    /// A DTO class for car orders.
    /// </summary>
    public class CarOrderDto
    {
        #region Properties

        /// <summary>
        /// The car bookings tied to the order.
        /// </summary>
        public List<CarBookingDto> CarBookings { get; set; } = new();

        /// <summary>
        /// The order ID.
        /// </summary>
        public int CarOrderId { get; set; }

        /// <summary>
        /// The customer that rented the car.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// The order date.
        /// </summary>
        public DateTime OrderDateUtc { get; set; }

        /// <summary>
        /// The order status.
        /// </summary>
        public OrderStatusDto OrderStatus { get; set; }

        /// <summary>
        /// A collection of payments tied to the order.
        /// </summary>
        public List<PaymentDto> Payments { get; set; } = new();

        #endregion
    }
}