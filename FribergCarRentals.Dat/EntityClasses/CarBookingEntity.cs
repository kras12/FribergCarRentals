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
        /// <param name="customer">The customer tied to the booking. Can't be null.</param>
        /// <param name="car">The car for the booking. Can't be null.</param>
        /// <param name="rentalCostPerDay">The rental cost per day. Can't be negative.</param>
        /// <param name="pickupDate"></param>
        /// <param name="returnDate"></param>
        public CarBookingEntity(CustomerEntity customer, CarEntity car,
            decimal rentalCostPerDay, DateTime pickupDate, DateTime returnDate)
        {
            #region Checks

            if (customer is null)
            {
                throw new ArgumentNullException(nameof(customer), $"The value of parameter '{customer}' can't be null.");
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

            Customer = customer;
            Car = car;
            RentalCostPerDay = rentalCostPerDay;
            PickupDateUtc = pickupDate;            
            ReturnDateUtc = returnDate;
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
        /// The customer that rented the car.
        /// </summary>
        [Required]
        public CustomerEntity? Customer { get; set; }

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
