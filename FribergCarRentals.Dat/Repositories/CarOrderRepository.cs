using FribergCarRentals.Data;
using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.Models;
using FribergCars.Shared.SharedTypes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentals.DataAccess.Repositories
{
    /// <summary>
    /// A repository class to handle the car entity.
    /// </summary>
    public class CarOrderRepository : GenericRepository<CarOrderEntity>, ICarOrderRepository
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="databaseContext">The database context to use.</param>
        public CarOrderRepository(ApplicationDbContext databaseContext) : base(databaseContext)
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Attempts to cancel a future booking that is atleast one day ahead in time. 
        /// If the order only contains one booking the order will be canceled as well. 
        /// </summary>
        /// <param name="carBooking">The ID of the car booking to cancel.</param>
        /// <returns>True if the booking was canceled. False if the date of the booking is in the past.</returns>
        public async Task<CancelCarBookingResult> CancelCarBookingOrOrder(int carBookingId)
        {
            CancelCarBookingResult result = CancelCarBookingResult.BookingNotFound;

            var order = await _databaseContext.CarOrders.SingleOrDefaultAsync(x => x.CarBookings.Any(x => x.CarBookingId == carBookingId));

            if (order != null)
            {
                var targetBooking = order.CarBookings.Single(x => x.CarBookingId == carBookingId);
                _databaseContext.CarBookingStatuses.Entry(targetBooking.BookingStatus!).State = EntityState.Detached;
                targetBooking.BookingStatus = CarBookingStatusEntity.CreateSeedObject(CarBookingStatus.Canceled);
                _databaseContext.CarBookingStatuses.Entry(targetBooking.BookingStatus!).State = EntityState.Unchanged;

                _databaseContext.CarRentalStatuses.Entry(targetBooking.Car!.RentalStatus!).State = EntityState.Detached;
                targetBooking.Car!.RentalStatus = CarRentalStatusEntity.CreateSeedObject(CarRentalStatus.Available);
                _databaseContext.CarRentalStatuses.Entry(targetBooking.Car!.RentalStatus).State = EntityState.Unchanged;

                result = CancelCarBookingResult.BookingCanceled;

                if (order.CarBookings.Count == 0)
                {
                    order.OrderStatus = OrderStatusEntity.CreateSeedObject(OrderStatus.Canceled);
                    _databaseContext.OrderStatuses.Entry(order.OrderStatus).State = EntityState.Unchanged;
                    result = CancelCarBookingResult.BookingAndOrderCanceled;
                }

                await _databaseContext.SaveChangesAsync();
            }
            else
            {
                result = CancelCarBookingResult.BookingNotFound;
            }

            return result;
        }

        /// <summary>
        /// Attempts to delete an order from the database.
        /// </summary>
        /// <param name="id">The ID of the entity to delete.</param>
        /// <returns>A <see cref="Task"/> object.</returns>
        public async Task<bool> DeleteOrder(int id)
        {
            //var carOrder = new CarOrderEntity() { CarOrderId = id };
            var carOrder = await _databaseContext.CarOrders.SingleOrDefaultAsync(x => x.CarOrderId == id);

            if (carOrder != null)
            {
                foreach (var booking in carOrder.CarBookings)
                {
                    booking.Car!.RentalStatus = CarRentalStatusEntity.CreateSeedObject(CarRentalStatus.Available);
                    _databaseContext.CarRentalStatuses.Entry(booking.Car!.RentalStatus).State = EntityState.Unchanged;
                }
               
                _databaseContext.CarOrders.Remove(carOrder);
                await _databaseContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns all orders that contains future bookings.
        /// </summary>
        /// <param name="minDaysAheadInTime">The minimum number of days ahead in time that the pickup date must be. Minimum value is 1.</param>
        /// <returns>A collection of bookings.</returns>
        public Task<List<CarOrderEntity>> GetOrdersWithFutureCarBookings(int minDaysAheadInTime = 1)
        {
            minDaysAheadInTime = Math.Max(minDaysAheadInTime, 1);
            return _databaseContext.CarOrders.Where(x => x.CarBookings.Any(x => x.PickupDateUtc.Date >= DateTime.UtcNow.Date.AddDays(minDaysAheadInTime))).ToListAsync();
        }

        /// <summary>
        /// Returns all orders that contains past bookings.
        /// </summary>
        /// <param name="minDaysAheadInTime">The minimum number of days back in time that the pickup date must be. Minimum value is 1.</param>
        /// <returns>A collection of bookings.</returns>
        public Task<List<CarOrderEntity>> GetOrdersWithPastCarBookings(int minDaysBackInTime = 1)
        {
            minDaysBackInTime = Math.Max(minDaysBackInTime, 1);
            return _databaseContext.CarOrders.Where(x => x.CarBookings.Any(x => x.PickupDateUtc.Date <= DateTime.UtcNow.Date.AddDays(-minDaysBackInTime))).ToListAsync();
        }

        /// <summary>
        /// Returns an order that contains a specifc car booking. 
        /// </summary>
        /// <param name="carBookingId">The ID of the carbooking to search for.</param>
        /// <returns>A <see cref="CarOrderEntity"/> object if the order was found or else null.</returns>
        public Task<CarOrderEntity?> GetOrderByCarBookingId(int carBookingId)
        {
            return _databaseContext.CarOrders.SingleOrDefaultAsync(x => x.CarBookings.Any(x => x.CarBookingId == carBookingId));
        }

        /// <summary>
        /// Returns a car booking.
        /// </summary>
        /// <param name="carBookingId">The ID of the car booking to search for.</param>
        /// <returns>A <see cref="CarBookingEntity"/> object.</returns>
        public Task<CarBookingEntity?> GetCarBookingById(int carBookingId)
        {
            return _databaseContext.CarBookings.SingleOrDefaultAsync(x => x.CarBookingId == carBookingId);
        }

        /// <summary>
        /// Returns all future car bookings.
        /// </summary>
        /// <param name="minDaysAheadInTime">The minimum number of days ahead in time that the pickup date must be. Minimum value is 1.</param>
        /// <returns>A collection of bookings.</returns>
        public Task<List<CarBookingEntity>> GetFutureCarBookings(int minDaysAheadInTime = 1)
        {
            minDaysAheadInTime = Math.Max(minDaysAheadInTime, 1);
            return _databaseContext.CarBookings
                .Where(x => x.PickupDateUtc.Date >= DateTime.UtcNow.Date.AddDays(minDaysAheadInTime) && x.BookingStatus!.StatusType == CarBookingStatus.Pending)
                .ToListAsync();
        }

        /// <summary>
        /// Returns all past car bookings.
        /// </summary>
        /// <param name="minDaysAheadInTime">The minimum number of days back in time that the pickup date must be. Minimum value is 1.</param>
        /// <returns>A collection of bookings.</returns>
        public Task<List<CarBookingEntity>> GetPastCarBookings(int minDaysBackInTime = 1)
        {
            minDaysBackInTime = Math.Max(minDaysBackInTime, 1);
            return _databaseContext.CarBookings.Where(x => x.PickupDateUtc.Date <= DateTime.UtcNow.Date.AddDays(-minDaysBackInTime)).ToListAsync();
        }

        #endregion
    }
}
