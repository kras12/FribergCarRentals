using FribergCarRentals.DataAccess.EntityClasses;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Models
{
    public class CarOrderViewModel : ViewModelBase
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="carOrder">The car order to copy data from.</param>
        public CarOrderViewModel(CarOrderEntity carOrder)
        {
            #region Checks

            if (carOrder.Customer is null)
            {
                throw new ArgumentNullException("The customer can't be null");
            }

            if (carOrder.CarBooking is null)
            {
                throw new ArgumentNullException("The car booking can't be null");
            }

            #endregion

            CarOrderId = carOrder.CarOrderId;
            OrderDate = carOrder.OrderDate;
            OrderSum = carOrder.OrderSum;
            OrderDetails = carOrder.OrderDetails;
            Customer = carOrder.Customer;
            Payments = carOrder.Payments;
            CarBooking = new CarBookingViewModel(carOrder.CarBooking);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The car booking tied to the order.
        /// </summary>
        public CarBookingViewModel CarBooking { get; set; }

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

        #endregion
    }
}
