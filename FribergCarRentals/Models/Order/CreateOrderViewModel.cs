using FribergCarRentals.DataAccess.EntityClasses;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace FribergCarRentals.Data.Order
{
    public class CreateOrderViewModel
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public CreateOrderViewModel()
        {

        }

        /// <summary>
        /// A constructor. 
        /// </summary>
        /// <param name="customerId">The customer for the order.</param>
        /// <param name="car">The car for the order.</param>
        /// <param name="pickupDate">The pickup date for the booking.</param>
        /// <param name="returnDate">The return date the booking.</param>
        public CreateOrderViewModel(int customerId, CarEntity car, DateTime pickupDate, DateTime returnDate)
        {
            CustomerId = customerId;
            CarId = car.CarId;
            CarDescription = $"{car.Brand} {car.Model} {car.ModelYear} ";
            PickupDateString = pickupDate.ToString("yyyy-MM-ddTHH:mm:00");
            ReturnDateString = returnDate.ToString("yyyy-MM-ddTHH:mm:00");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Description of the car.
        /// </summary>
        [BindNever]
        public string CarDescription { get; set; } = "";

        /// <summary>
        /// The car for the order.
        /// </summary>
        public int CarId { get; set; }
        /// <summary>
        /// The customer for the car.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// The return date.
        /// </summary>
        [DisplayName("Pickup Date")]
        public string PickupDateString { get; set; }

        /// <summary>
        /// The return date.
        /// </summary>
        [DisplayName("Return Date")]
        public string ReturnDateString { get; set; }

        #endregion
    }
}
