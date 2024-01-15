using FribergCarRentals.DataAccess.EntityClasses;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Models
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
            CarBookingId = carBooking.CarBookingId;
            Customer = carBooking.Customer ?? throw new ArgumentNullException("The customer can't be null.");
            Car = carBooking.Car ?? throw new ArgumentNullException("The car can't be null");
            RentalCostPerDay = carBooking.RentalCostPerDay;
            PickupDateUtc = carBooking.PickupDateUtc;
            ReturnDateUtc = carBooking.ReturnDateUtc;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The return date.
        /// </summary>
        [DisplayName("Return Date")]
        public DateTime ReturnDateUtc { get; set; }


        /// <summary>
        /// The return date.
        /// </summary>
        [DisplayName("Pickup Date")]
        public DateTime PickupDateUtc { get; set; }

        /// <summary>
        /// Returna the return date as a formatted string.
        /// </summary>
        public string ReturnDateString
        {
            get
            {
                return ReturnDateUtc.ToLocalTime().ToString("d");
            }
        }

        /// <summary>
        /// Returna the picup date as a formatted string.
        /// </summary>
        public string PickupDateString
        {
            get
            {
                return PickupDateUtc.ToLocalTime().ToString("d");
            }
        }

        /// <summary>
        /// The rental cost per day.
        /// </summary>
        [DisplayName("Cost Per Day")]
        public decimal RentalCostPerDay { get; set; }

        /// <summary>
        /// The car that was rented.
        /// </summary>
        public CarEntity Car { get; set; }

        /// <summary>
        /// The car booking ID.
        /// </summary>
        public int CarBookingId { get; set; }

        /// <summary>
        /// The customer that rented the car.
        /// </summary>
        public CustomerEntity Customer { get; set; }

        #endregion
    }
}
