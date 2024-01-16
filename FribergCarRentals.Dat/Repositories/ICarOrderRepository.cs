using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentals.DataAccess.Repositories
{
    #region Enums
    public enum CancelCarBookingResult
    {
        BookingNotFound,
        BookingCanceled,
        BookingAndOrderCanceled
    }

    #endregion

    #region Interfaces

    /// <summary>
    /// An interface for a car order repository.
    /// </summary>
    public interface ICarOrderRepository : IRepositoryBase<CarOrderEntity>
    {
        #region Methods

        /// <summary>
        /// Attempts to cancel a future booking that is atleast one day ahead in time. 
        /// If the order only contains one booking the order will be canceled as well. 
        /// </summary>
        /// <param name="carBooking">The ID of the car booking to cancel.</param>
        /// <returns>True if the booking was canceled. False if the date of the booking is in the past.</returns>
        public Task<CancelCarBookingResult> CancelCarBookingOrOrder(int carBookingId);

        /// <summary>
        /// Attempts to delete an order from the database.
        /// </summary>
        /// <param name="id">The ID of the entity to delete.</param>
        /// <returns>A <see cref="Task"/> object.</returns>
        public Task<bool> DeleteOrder(int id);

        /// <summary>
        /// Returns a car booking.
        /// </summary>
        /// <param name="carBookingId">The ID of the car booking to search for.</param>
        /// <returns>A <see cref="CarBookingEntity"/> object.</returns>
        public Task<CarBookingEntity?> GetCarBookingById(int carBookingId);

        /// <summary>
        /// Returns all future car bookings.
        /// </summary>
        /// <param name="minDaysAheadInTime">The minimum number of days ahead in time that the pickup date must be. Minimum value is 1.</param>
        /// <returns>A collection of bookings.</returns>
        public Task<List<CarBookingEntity>> GetFutureCarBookings(int minDaysAheadInTime = 1);

        /// <summary>
        /// Returns an order that contains a specifc car booking. 
        /// </summary>
        /// <param name="carBookingId">The ID of the carbooking to search for.</param>
        /// <returns>A <see cref="CarOrderEntity"/> object if the order was found or else null.</returns>
        public Task<CarOrderEntity?> GetOrderByCarBookingId(int carBookingId);

        /// <summary>
        /// Returns all orders that contains future bookings.
        /// </summary>
        /// <param name="minDaysAheadInTime">The minimum number of days ahead in time that the pickup date must be. Minimum value is 1.</param>
        /// <returns>A collection of bookings.</returns>
        public Task<List<CarOrderEntity>> GetOrdersWithFutureCarBookings(int minDaysAheadInTime = 1);

        /// <summary>
        /// Returns all orders that contains past bookings.
        /// </summary>
        /// <param name="minDaysAheadInTime">The minimum number of days back in time that the pickup date must be. Minimum value is 1.</param>
        /// <returns>A collection of bookings.</returns>
        public Task<List<CarOrderEntity>> GetOrdersWithPastCarBookings(int minDaysBackInTime = 1);
        /// <summary>
        /// Returns all past car bookings.
        /// </summary>
        /// <param name="minDaysAheadInTime">The minimum number of days back in time that the pickup date must be. Minimum value is 1.</param>
        /// <returns>A collection of bookings.</returns>
        public Task<List<CarBookingEntity>> GetPastCarBookings(int minDaysBackInTime = 1);

        #endregion
    }

    #endregion


}
