using FribergCarRentals.Data;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FribergCarRentals.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace FribergCarRentals.Data.Repositories
{
    /// <summary>
    /// A repository that handles car bookings.
    /// </summary>
    //public class CarBookingRepository : GenericRepository<CarBookingEntity>, ICarBookingRepository
    //{
    //    #region Constructors

    //    /// <summary>
    //    /// A constructor.
    //    /// </summary>
    //    /// <param name="databaseContext">The database context to use.</param>
    //    public CarBookingRepository(ApplicationDbContext databaseContext) : base(databaseContext)
    //    {

    //    }

    //    #endregion

    //    #region Methods

    //    /// <summary>
    //    /// Attmempts to book a car. 
    //    /// </summary>
    //    /// <param name="carBooking">The car booking data to use.</param>
    //    /// <returns>True if the car was booked. False if the car could not be booked.</returns>
    //    public async Task<bool> BookCar(CarBookingEntity carBooking)
    //    {
    //        #region Checks

    //        if (carBooking.Car is null)
    //        {
    //            throw new InvalidOperationException("The car in the booking can't be null.");
    //        }

    //        #endregion

    //        if (await _databaseContext.Cars.AnyAsync(x => x.CarId == carBooking.Car.CarId && x.RentalStatus!.StatusType == CarRentalStatus.Available))
    //        {
    //            carBooking.Car.RentalStatus = CarRentalStatusEntity.CreateSeedObject(CarRentalStatus.Rented);
    //            _databaseContext.CarRentalStatuses.Entry(carBooking.Car.RentalStatus).State = EntityState.Unchanged;
    //            await Add(carBooking);
    //            return true;
    //        }

    //        return false;
    //    }

    //    /// <summary>
    //    /// Attempts to cancel a future booking that is atleast one day ahead in time.
    //    /// </summary>
    //    /// <param name="carBooking">The ID of the car booking to cancel.</param>
    //    /// <param name="minDaysAheadInTime">The minimum number of days ahead in time that the pickup date must be. Minimum value is 1.</param>
    //    /// <returns>True if the booking was canceled. False if the date of the booking is in the past.</returns>
    //    public async Task<bool> CancelFutureBooking(int carBookingId, int minDaysAheadInTime = 1)
    //    {
    //        minDaysAheadInTime = Math.Max(minDaysAheadInTime, 1);
    //        var order = await _databaseContext.CarOrders.SingleOrDefaultAsync(x => x.CarBooking!.CarBookingId == carBookingId &&
    //            x.CarBooking.PickupDateUtc.Date >= DateTime.UtcNow.Date.AddDays(minDaysAheadInTime));

    //        if (order != null)
    //        {
    //            order.OrderStatus = OrderStatusEntity.CreateSeedObject(OrderStatus.Canceled);
    //            order.CarBooking!.Car!.RentalStatus = CarRentalStatusEntity.CreateSeedObject(CarRentalStatus.Available);
    //            await _databaseContext.SaveChangesAsync();
    //            return true;
    //        }

    //        return false;
    //    }

    //    /// <summary>
    //    /// Returns all future bookings.
    //    /// </summary>
    //    /// <param name="minDaysAheadInTime">The minimum number of days ahead in time that the pickup date must be. Minimum value is 1.</param>
    //    /// <returns>A collection of bookings.</returns>
    //    public async Task<List<CarBookingEntity>> GetFutureBookings(int minDaysAheadInTime = 1)
    //    {
    //        minDaysAheadInTime = Math.Max(minDaysAheadInTime, 1);
    //        var test = await _databaseContext.CarBookings.ToListAsync();
    //        var date1 = new DateTime(2024, 1, 16).Date;
    //        var date2 = new DateTime(2024, 1, 15).Date.AddDays(1);
    //        var result = date1 >= date2;
    //        return await _databaseContext.CarBookings.Where(x => x.PickupDateUtc.Date >= DateTime.UtcNow.Date.AddDays(minDaysAheadInTime)).ToListAsync();
    //    }

    //    /// <summary>
    //    /// Returns all past bookings.
    //    /// </summary>
    //    /// <param name="minDaysAheadInTime">The minimum number of days back in time that the pickup date must be. Minimum value is 1.</param>
    //    /// <returns>A collection of bookings.</returns>
    //    public Task<List<CarBookingEntity>> GetPastBookings(int minDaysBackInTime = 1)
    //    {
    //        minDaysBackInTime = Math.Max(minDaysBackInTime, 1);
    //        return _databaseContext.CarBookings.Where(x => x.PickupDateUtc.Date <= DateTime.UtcNow.Date.AddDays(-minDaysBackInTime)).ToListAsync();
    //    }

    //    /// <summary>
    //    /// Returns all the cars that are availble to be rented out. 
    //    /// </summary>
    //    /// <returns>A collection of cars.</returns>
    //    public Task<List<CarEntity>> GetRentableCars()
    //    {
    //        return _databaseContext.Cars.Where(x => x.RentalStatus!.StatusType == CarRentalStatus.Available).ToListAsync();
    //    }

    //    #endregion
    //}
}
