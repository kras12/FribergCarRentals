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

        public CarOrderEntity()
        {
            
        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="carOrderId">The database ID for this entity. Can't be a negative value.</param>
        /// <param name="orderDate">The order date.</param>
        /// <param name="pickupDate">The pickup date.</param>
        /// <param name="returnDate">The return date.</param>
        /// <param name="car">The car that was rented.</param>
        /// <param name="rentalCostPerDay">The rental cost per day. Can't be negative.</param>
        /// <param name="customer">The customer that rented the car.</param>
        /// <param name="orderSum">The total sum of the order.</param>
        /// <param name="payments"> A collection of payments tied to the order.</param>
        /// <param name="orderDetails">Details about the order. </param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public CarOrderEntity(DateTime orderDate, DateTime pickupDate, DateTime returnDate,
            CarEntity car, decimal rentalCostPerDay, CustomerEntity customer, decimal orderSum,
            List<PaymentEntity> payments, string orderDetails) :
            this(carOrderId: 0, orderDate, pickupDate, returnDate, rentalCostPerDay, orderSum, orderDetails)
        {
            #region Checks

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

            // EF Core can't set navigational properties through a constructor, 
            // so these values will have to be set in this constructor.
            Car = car;
            Customer = customer;
            OrderDetails = orderDetails;
            Payments = payments;
        }

        /// <summary>
        /// A constructor intended for EF core. 
        /// </summary>
        /// <param name="carOrderId">The database ID for this entity. Can't be a negative value.</param>
        /// <param name="orderDate">The order date.</param>
        /// <param name="pickupDate">The pickup date.</param>
        /// <param name="returnDate">The return date.</param>
        /// <param name="rentalCostPerDay">The rental cost per day. Can't be negative.</param>
        /// <param name="orderSum">The total sum of the order.</param>
        /// <param name="orderDetails">Details about the order. </param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        private CarOrderEntity(int carOrderId, DateTime orderDate, DateTime pickupDate, DateTime returnDate,
            decimal rentalCostPerDay, decimal orderSum, string orderDetails)
        {
            #region Checks

            if (carOrderId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(carOrderId), $"The value of parameter '{carOrderId}' can't be negative.");
            }

            if (rentalCostPerDay < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rentalCostPerDay), $"The value of parameter '{rentalCostPerDay}' can't be negative.");
            }            

            if (orderSum < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(orderSum), $"The value of parameter '{orderSum}' can't be negative.");
            }                        

            if (orderDetails is null)
            {
                throw new ArgumentNullException(nameof(orderDetails), $"The value of parameter '{orderDetails}' can't be null.");
            }

            #endregion

            CarOrderId = carOrderId;
            OrderDate = orderDate;
            PickupDate = pickupDate;
            ReturnDate = returnDate;            
            RentalCostPerDay = rentalCostPerDay;            
            OrderSum = orderSum;
            OrderDetails = orderDetails;

            // EF Core can't set navigational properties through a constructor, 
            // so these will be setby EF Core via the properties after the constructor have run. 
            Car = null!;
            Customer = null!;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The car that was rented.
        /// </summary>
        public CarEntity Car { get; set; }

        /// <summary>
        /// The order ID.
        /// </summary>
        [Key]
        public int CarOrderId { get; set; }

        /// <summary>
        /// The customer that rented the car.
        /// </summary>
        public CustomerEntity Customer { get; set; }

        /// <summary>
        /// The order date.
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Details about the order. 
        /// </summary>
        public string OrderDetails { get; set; } = "";

        /// <summary>
        /// The total sum of the order.
        /// </summary>
        public decimal OrderSum { get; set; }

        /// <summary>
        /// A collection of payments tied to the order.
        /// </summary>
        public List<PaymentEntity> Payments { get; set; } = new();

        /// <summary>
        /// The pickup date.
        /// </summary>
        public DateTime PickupDate { get; set; }
        /// <summary>
        /// The rental cost per day.
        /// </summary>
        public decimal RentalCostPerDay { get; set; }

        /// <summary>
        /// The return date.
        /// </summary>
        public DateTime ReturnDate { get; set; }

        #endregion
    }
}