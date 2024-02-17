using FribergCarRentals.Models.Car;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FribergCarRentals.DataAccess.EntityClasses;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using FribergCarRentals.Models.Other;

namespace FribergCarRentals.Models.Order
{
    /// <summary>
    /// A view model class that handles data relating to car bookings. 
    /// </summary>
    public class CarBookingViewModel : ViewModelBase
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="carBooking">The car booking to copy data from.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public CarBookingViewModel(CarBookingEntity carBooking)
        {
            #region Checks

            if (carBooking.Car is null)
            {
                throw new ArgumentNullException("The car in the booking can't be null.");
            }

            #endregion

            CarBookingId = carBooking.CarBookingId;
            CarOrder = carBooking.CarOrder!;
            Car = new CarViewModel(carBooking.Car);
            RentalCostPerDay = carBooking.RentalCostPerDay;
            CarPickupDate = carBooking.PickupDateUtc.Date;
            CarReturnDate = carBooking.ReturnDateUtc.Date;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The car that was rented.
        /// </summary>
        [DisplayName("Car")]
        [BindNever]
        public CarViewModel Car { get; }

        /// <summary>
        /// The ID for the booking.
        /// </summary>
        [DisplayName("Booking ID")]
        [BindNever]
        public int CarBookingId { get; }

        /// <summary>
        /// The order the booking belongs to.
        /// </summary>
        [DisplayName("Order")]
        [BindNever]
        public CarOrderEntity CarOrder { get; }

        /// <summary>
        /// The ID for the order.
        /// </summary>
        [DisplayName("Order ID")]
        [BindNever]
        public int CarOrderId
        {
            get
            {
                return CarOrder.CarOrderId;
            }
        }

        /// <summary>
        /// The car pickup date.
        /// </summary>
        /// <remarks>The date is saved in the database with the time component stripped off.</remarks>
        [DisplayName("Pickup Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = DateFormatString)]
        [BindNever]
        public DateTime CarPickupDate { get; }

        /// <summary>
        /// The car return date.
        /// </summary>
        /// <remarks>The date is saved in the database with the time component stripped off.</remarks>
        [DisplayName("Return Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = DateFormatString)]
        [BindNever]
        public DateTime CarReturnDate { get; }

        /// <summary>
        /// The number of rental days the customer is paying for. 
        /// </summary>
        [DisplayName("Debited Days")]
        [BindNever]
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
        [DisplayFormat(DataFormatString = DefaultPriceOutputFormatString)]
        [BindNever]
        public decimal RentalCostPerDay { get; }

        #endregion
    }
}
