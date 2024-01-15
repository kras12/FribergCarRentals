using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Models
{
    internal class CarOrderViewModel : ViewModelBase
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="carOrder">The caro order to copy data from.</param>
        public CarOrderViewModel(CarOrderEntity carOrder)
        {
            CarOrderId = carOrder.CarOrderId;
            OrderDate = carOrder.OrderDate;
            PickupDate = carOrder.PickupDate;
            ReturnDate = carOrder.ReturnDate;
            RentalCostPerDay = carOrder.RentalCostPerDay;
            OrderSum = carOrder.OrderSum;
            OrderDetails = carOrder.OrderDetails;
            Car = carOrder.Car;
            Customer = carOrder.Customer;
            Payments = carOrder.Payments;
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
        [DisplayName("ID")]
        public int CarOrderId { get; set; }

        /// <summary>
        /// The customer that rented the car.
        /// </summary>
        public CustomerEntity Customer { get; set; }

        /// <summary>
        /// The order date.
        /// </summary>
        [DisplayName("Date")]
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Details about the order. 
        /// </summary>
        [DisplayName("Details")]
        public string OrderDetails { get; set; } = "";

        /// <summary>
        /// The total sum of the order.
        /// </summary>
        [DisplayName("Order Sum")]
        public decimal OrderSum { get; set; }

        /// <summary>
        /// A collection of payments tied to the order.
        /// </summary>
        public List<PaymentEntity> Payments { get; set; } = new();

        /// <summary>
        /// The pickup date.
        /// </summary>
        [DisplayName("Pickup Date")]
        public DateTime PickupDate { get; set; }

        public string PickupDateString
        {
            get
            {
                return PickupDate.ToString("d");
            }
        }

        /// <summary>
        /// The rental cost per day.
        /// </summary>
        [DisplayName("Cost Per Day")]
        public decimal RentalCostPerDay { get; set; }

        /// <summary>
        /// The return date.
        /// </summary>
        [DisplayName("Return Date")]
        public DateTime ReturnDate { get; set; }

        public string ReturnDateString
        {
            get
            {
                return ReturnDate.ToString("d");
            }
        }

        #endregion
    }
}
