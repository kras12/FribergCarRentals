using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FribergCarRentals.Shared.Constants;
using FribergCarRentals.Shared.Models.ViewModels.Car;
using FribergCarRentals.Shared.Models.ViewModels.Other;

namespace FribergCarRentals.Shared.Models.ViewModels.Order
{
    /// <summary>
    /// A view model class that handles data relating to car bookings. 
    /// </summary>
    public class CarBookingViewModel : ViewModelBase
    {

        #region Properties

        /// <summary>
        /// The car that was rented.
        /// </summary>
        [DisplayName("Car")]
        public CarViewModel Car { get; set; }

        /// <summary>
        /// The ID for the booking.
        /// </summary>
        [DisplayName("Booking ID")]
        public int CarBookingId { get; set; }

        /// <summary>
        /// The order the booking belongs to.
        /// </summary>
        [DisplayName("Order")]
        public OrderViewModel Order { get; set; } = default!;

        /// <summary>
        /// The car pickup date.
        /// </summary>
        /// <remarks>The date is saved in the database with the time component stripped off.</remarks>
        [DisplayName("Pickup Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = ValidationRules.DateFormatString)]
        public DateTime CarPickupDate { get; set; }

        /// <summary>
        /// The car return date.
        /// </summary>
        /// <remarks>The date is saved in the database with the time component stripped off.</remarks>
        [DisplayName("Return Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = ValidationRules.DateFormatString)]
        public DateTime CarReturnDate { get; set; }

        /// <summary>
        /// The number of rental days the customer is paying for. 
        /// </summary>
        [DisplayName("Debited Days")]
        public int DebitedRentalDays
        {
            get
            {
                return (CarReturnDate - CarPickupDate).Days + 1;
            }
        }

        /// <summary>
        /// The rental cost per day.
        /// </summary>
        [DisplayName("Cost Per Day")]
        [DisplayFormat(DataFormatString = ValidationRules.DefaultPriceOutputFormatString)]
        public decimal RentalCostPerDay { get; set; }

        #endregion
    }
}
