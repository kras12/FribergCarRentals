using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.Models.Car;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Models.Order
{
    public class CarBookingViewModel : ViewModelBase
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="carBooking">The car booking to copy data from.</param>
        public CarBookingViewModel(CarBookingEntity carBooking)
        {
            #region Checks

            if (carBooking.Car is null)
            {
                throw new ArgumentNullException("The car in the booking can't be null.");
            }

            #endregion
            CarBookingId = carBooking.CarBookingId;
            CarOrder = carBooking.CarOrder;
            Car = new CarViewModel(carBooking.Car);
            RentalCostPerDay = carBooking.RentalCostPerDay;
            PickupDateUtc = carBooking.PickupDateUtc;
            ReturnDateUtc = carBooking.ReturnDateUtc;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The car that was rented.
        /// </summary>
        public CarViewModel Car { get; set; }

        /// <summary>
        /// The car booking ID.
        /// </summary>
        [DisplayName("Booking ID")]
        public int CarBookingId { get; set; }

        /// <summary>
        /// The order the booking belongs to.
        /// </summary>
        public CarOrderEntity CarOrder { get; set; }

        /// <summary>
        /// The car order id.
        /// </summary>
        [DisplayName("Order ID")]
        public int CarOrderId
        {
            get
            {
                return CarOrder.CarOrderId;
            }
        }

        /// <summary>
        /// The pickup date in local time.
        /// </summary>        
        [DisplayName("Pickup Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]
        public DateTime PickupDateLocal
        {
            get
            {
                return PickupDateUtc.ToLocalTime();
            }
        }

        /// <summary>
        /// The pickup date.
        /// </summary>
        [DisplayName("Pickup Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]
        public DateTime PickupDateUtc { get; set; }

        /// <summary>
        /// The rental cost per day.
        /// </summary>
        [DisplayName("Cost Per Day")]
        public decimal RentalCostPerDay { get; set; }

        /// <summary>
        /// The return date in local time.
        /// </summary>
        [DisplayName("Return Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]
        public DateTime ReturnDateLocal
        {
            get
            {
                return ReturnDateUtc.ToLocalTime();
            }
        }

        /// <summary>
        /// The return date.
        /// </summary>
        [DisplayName("Return Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]
        public DateTime ReturnDateUtc { get; set; }

        #endregion
    }
}
