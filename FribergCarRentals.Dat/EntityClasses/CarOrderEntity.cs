using FribergCarRentals.DataAccess.EntityClasses;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FribergCarRentals.Models
{
    /// <summary>
    /// A class that represents a car order. 
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
        /// <param name="orderStatus">The order status. Can't be null.</param>
        /// <param name="orderDate">The order date.</param>
        /// <param name="pickupDate">The pickup date.</param>
        /// <param name="returnDate">The return date.</param>
        /// <param name="car">The car that was rented.</param>
        /// <param name="rentalCostPerDay">The rental cost per day. Can't be negative.</param>
        /// <param name="customer">The customer that rented the car.</param>
        /// <param name="orderSum">The total sum of the order.</param>
        /// <param name="payments"> A collection of payments tied to the order.</param>
        /// <param name="orderDetails">Details about the order. </param>
        /// <param name="carBooking">The car booking tied to the order.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public CarOrderEntity(OrderStatusEntity orderStatus, DateTime orderDate, CarEntity car, CustomerEntity customer, decimal orderSum,
            List<PaymentEntity> payments, string orderDetails, CarBookingEntity carBooking)
        {
            #region Checks

            if (orderStatus is null)
            {
                throw new ArgumentNullException(nameof(orderStatus), $"The value of parameter '{orderStatus}' can't be null.");
            }

            if (orderSum < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(orderSum), $"The value of parameter '{orderSum}' can't be negative.");
            }

            if (orderDetails is null)
            {
                throw new ArgumentNullException(nameof(orderDetails), $"The value of parameter '{orderDetails}' can't be null.");
            }

            if (car is null)
            {
                throw new ArgumentNullException(nameof(car), $"The value of parameter '{car}' can't be null.");
            }

            if (customer is null)
            {
                throw new ArgumentNullException(nameof(customer), $"The value of parameter '{customer}' can't be null.");
            }

            if (payments is null)
            {
                throw new ArgumentNullException(nameof(payments), $"The value of parameter '{payments}' can't be null.");
            }

            #endregion

            OrderStatus = orderStatus;
            OrderDate = orderDate;
            OrderSum = orderSum;
            OrderDetails = orderDetails;
            Customer = customer;
            OrderDetails = orderDetails;
            Payments = payments;
            CarBooking = carBooking;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The car booking tied to the order.
        /// </summary>
        [Required]
        public CarBookingEntity? CarBooking { get; set; }

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
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Details about the order. 
        /// </summary>
        public string OrderDetails { get; set; } = "";

        /// <summary>
        /// The order status.
        /// </summary>
        public OrderStatusEntity? OrderStatus { get; set; }

        /// <summary>
        /// The total sum of the order.
        /// </summary>
        public decimal OrderSum { get; set; }

        /// <summary>
        /// A collection of payments tied to the order.
        /// </summary>
        public List<PaymentEntity> Payments { get; set; } = new();
        #endregion
    }
}