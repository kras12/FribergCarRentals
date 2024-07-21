using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentals.Data.EntityClasses
{
    /// <summary>
    /// An entity class that represents a car booking.
    /// </summary>
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
        /// <param name="order">The order the booking belongs to.</param>
        /// <param name="car">The car for the booking. Can't be null.</param>
        /// <param name="pickupDateUTC">The car pickup date (UTC) for the booking.</param>
        /// <param name="returnDateUTC">The return pickup date (UTC) for the booking.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public CarBookingEntity(CarOrderEntity order, CarEntity car, DateTime pickupDateUTC, DateTime returnDateUTC)
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

            if (returnDateUTC < pickupDateUTC)
            {
                throw new ArgumentOutOfRangeException("The return date can't be smaller than the pickup date.");
            }

            #endregion

            CarOrder = order;
            Car = car;
            RentalCostPerDay = car.RentalCostPerDay;
            PickupDateUtc = pickupDateUTC;
            ReturnDateUtc = returnDateUTC;
        }

        #endregion

        #region Properties

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
        /// The order the booking belongs to. 
        /// </summary>
        [Required]
        public CarOrderEntity? CarOrder { get; set; }

        /// <summary>
        /// The car pickup date in UTC time.
        /// </summary>
        public DateTime PickupDateUtc { get; set; }

        /// <summary>
        /// The rental cost per day.
        /// </summary>
        public decimal RentalCostPerDay { get; set; }

        /// <summary>
        /// The car return date in UTC time.
        /// </summary>
        public DateTime ReturnDateUtc { get; set; }

        #endregion
    }
}
