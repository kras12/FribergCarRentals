using FribergCarRentals.DataAccess.EntityClasses;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        public CarOrderViewModel()
        {
            
        }

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

            if (carOrder.CarBookings is null)
            {
                throw new ArgumentNullException("The car booking can't be null");
            }

            #endregion

            CarOrderId = carOrder.CarOrderId;
            OrderDate = carOrder.OrderDate;
            Customer = carOrder.Customer;
            Payments = carOrder.Payments;
            CarBooking = carOrder.CarBookings.Count > 0 ? new CarBookingViewModel(carOrder.CarBookings.First()) 
                : throw new InvalidOperationException("Could not find a car booking");
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
        [DisplayName("Order ID")]
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
        /// The total sum of the order.
        /// </summary>
        [DisplayName("Order Sum")]
        public decimal OrderSum
        {
            get
            {
                return CarBooking?.RentalCostPerDay * (CarBooking?.ReturnDateUtc - CarBooking?.PickupDateUtc)?.Days ?? 0;
            }
        }

        /// <summary>
        /// The total sum of all payments.
        /// </summary>
        [DisplayName("Payments Sum")]
        public decimal PaymentsSum
        {
            get
            {
                return Payments.Sum(x => x.Amount);
            }
        }

        /// <summary>
        /// A collection of payments tied to the order.
        /// </summary>
        public List<PaymentEntity> Payments { get; set; } = new();

        #endregion
    }
}
