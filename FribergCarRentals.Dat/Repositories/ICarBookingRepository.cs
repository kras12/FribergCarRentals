using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentals.DataAccess.Repositories
{
    /// <summary>
    /// An interface for a car booking repository.
    /// </summary>
    //public interface ICarBookingRepository
    //{
    //    #region Methods

    //    /// <summary>
    //    /// Attmempts to book a car. 
    //    /// </summary>
    //    /// <param name="carBooking">The car booking data to use.</param>
    //    /// <returns>True if the car was booked. False if the car could not be booked.</returns>
    //    public Task<bool> BookCar(CarBookingEntity carBooking);

    //    /// <summary>
    //    /// Attempts to cancel a future booking that is atleast one day ahead in time.
    //    /// </summary>
    //    /// <param name="carBooking">The ID of the car booking to cancel.</param>
    //    /// <param name="minDaysAheadInTime">The minimum number of days ahead in time that the pickup date must be. Minimum value is 1.</param>
    //    /// <returns>True if the booking was canceled. False if the date of the booking is in the past.</returns>
    //    public Task<bool> CancelFutureBooking(int carBookingId, int minDaysAheadInTime = 1);

    //    /// <summary>
    //    /// Returns all future bookings.
    //    /// </summary>
    //    /// <param name="minDaysAheadInTime">The minimum number of days ahead in time that the pickup date must be. Minimum value is 1.</param>
    //    /// <returns>A collection of bookings.</returns>
    //    public Task<List<CarBookingEntity>> GetFutureBookings(int minDaysAheadInTime = 1);

    //    /// <summary>
    //    /// Returns all past bookings.
    //    /// </summary>
    //    /// <param name="minDaysAheadInTime">The minimum number of days back in time that the pickup date must be. Minimum value is 1.</param>
    //    /// <returns>A collection of bookings.</returns>
    //    public Task<List<CarBookingEntity>> GetPastBookings(int minDaysBackInTime = 1);

    //    /// <summary>
    //    /// Returns all the cars that are availble to be rented out. 
    //    /// </summary>
    //    /// <returns>A collection of cars.</returns>
    //    public Task<List<CarEntity>> GetRentableCars();
    //    #endregion
    //}
}
