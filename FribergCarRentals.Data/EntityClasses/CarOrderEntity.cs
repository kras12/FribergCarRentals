using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FribergCarRentals.Data.EntityClasses
{
    /// <summary>
    /// An entity class that represents a car order. 
    /// </summary>
    [Table("CarOrders")]
    public class CarOrderEntity
    {
        #region Constructors

        /// <summary>
        /// A constructor
        /// </summary>
        public CarOrderEntity()
        {

        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="customer">The customer that rented the car.</param>
        /// <param name="carBookings">The car bookings tied to the order.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public CarOrderEntity(CustomerEntity customer) :
            this(OrderStatusEntity.CreateFromType(Types.OrderStatus.Created),
                DateTime.UtcNow, customer, payments: new(), carBookings: new())
        {

        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="orderStatus">The order status.</param>
        /// <param name="orderDate">The order date.</param>
        /// <param name="customer">The customer that rented the car.</param>
        /// <param name="payments"> A collection of payments tied to the order.</param>
        /// <param name="carBookings">The car bookings tied to the order.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public CarOrderEntity(OrderStatusEntity orderStatus, DateTime orderDate, CustomerEntity customer,
            List<PaymentEntity> payments, List<CarBookingEntity> carBookings)
        {
            #region Checks

            if (orderStatus is null)
            {
                throw new ArgumentNullException(nameof(orderStatus), $"The value of parameter '{orderStatus}' can't be null.");
            }

            if (customer is null)
            {
                throw new ArgumentNullException(nameof(customer), $"The value of parameter '{customer}' can't be null.");
            }

            if (payments is null)
            {
                throw new ArgumentNullException(nameof(payments), $"The value of parameter '{payments}' can't be null.");
            }

            if (carBookings is null)
            {
                throw new ArgumentNullException(nameof(carBookings), $"The value of parameter '{carBookings}' can't be null.");
            }

            #endregion

            CarOrderId = 0;
            OrderStatus = orderStatus;
            OrderDateUtc = orderDate;
            Customer = customer;
            Payments = payments;
            CarBookings = carBookings;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The car bookings tied to the order.
        /// </summary>
        public List<CarBookingEntity> CarBookings { get; set; } = new();

        /// <summary>
        /// The order ID.
        /// </summary>
        [Key]
        public int CarOrderId { get; set; }

        /// <summary>
        /// The customer that rented the car.
        /// </summary>
        [Required]
        public CustomerEntity? Customer { get; set; }

        /// <summary>
        /// The order date.
        /// </summary>
        public DateTime OrderDateUtc { get; set; }

        /// <summary>
        /// The order status.
        /// </summary>
        [Required]
        public OrderStatusEntity? OrderStatus { get; set; }

        /// <summary>
        /// A collection of payments tied to the order.
        /// </summary>
        public List<PaymentEntity> Payments { get; set; } = new();

        #endregion
    }
}