using FribergCarRentals.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentals.DataAccess.EntityClasses
{
    public class CarBookingEntity
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public CarBookingEntity()
        {

        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="bookingStatus">The status of the booking.</param>
        /// <param name="order">The order the booking belongs to.</param>
        /// <param name="car">The car for the booking. Can't be null.</param>
        /// <param name="rentalCostPerDay">The rental cost per day. Can't be negative.</param>
        /// <param name="pickupDate"></param>
        /// <param name="returnDate"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public CarBookingEntity(CarBookingStatusEntity bookingStatus, CarOrderEntity order, CarEntity car,
            decimal rentalCostPerDay, DateTime pickupDate, DateTime returnDate)
        {
            #region Checks

            if (order is null)
            {
                throw new ArgumentNullException(nameof(order), $"The value of parameter '{order}' can't be null.");
            }

            if (car is null)
            {
                throw new ArgumentNullException(nameof(car), $"The value of parameter '{car}' can't be null.");
            }

            if (rentalCostPerDay < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rentalCostPerDay), $"The value of parameter '{rentalCostPerDay}' can't be negative.");
            }

            #endregion

            BookingStatus = bookingStatus;
            CarOrder = order;
            Car = car;
            RentalCostPerDay = rentalCostPerDay;
            PickupDateUtc = pickupDate;            
            ReturnDateUtc = returnDate;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The status of the booking.
        /// </summary>
        [Required]
        public CarBookingStatusEntity? BookingStatus { get; set; }

        /// <summary>
        /// The order the booking belongs to. 
        /// </summary>
        [Required]
        public CarOrderEntity CarOrder { get; set; }

        /// <summary>
        /// The car that was rented.
        /// </summary>
        [Required]
        public CarEntity? Car { get; set; }

        /// <summary>
        /// The car booking ID.
        /// </summary>
        [Key]
        public int CarBookingId { get; set; }

        /// <summary>
        /// The pickup date.
        /// </summary>
        public DateTime PickupDateUtc { get; set; }

        /// <summary>
        /// The rental cost per day.
        /// </summary>
        public decimal RentalCostPerDay { get; set; }

        /// <summary>
        /// The return date.
        /// </summary>
        public DateTime ReturnDateUtc { get; set; }

        #endregion
    }
}
