using FribergCarRentals.Data;
using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.Models;
using FribergCars.Shared.SharedTypes;
using Microsoft.AspNetCore.Mvc;
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

        public override async Task<CarOrderEntity> Add(CarOrderEntity entity)
        {
            await _databaseContext.CarOrders.AddAsync(entity);

            _databaseContext.Entry(entity.OrderStatus!).State = EntityState.Unchanged;
            _databaseContext.Entry(entity.Customer!).State = EntityState.Unchanged;

            foreach (var booking in entity.CarBookings)
            {
                _databaseContext.Entry(booking.BookingStatus!).State = EntityState.Unchanged;
                _databaseContext.Entry(booking!.Car!.RentalStatus!).State = EntityState.Unchanged;
            }

            await _databaseContext.SaveChangesAsync();
            return entity;

        }

        /// <summary>
        /// Attempts to cancel a car order.
        /// </summary>
        /// <param name="carOrderId">The ID of the order to cancel.</param>
        /// <returns>True if the order was canceled.</returns>
        public async Task<bool> CancelOrder(int carOrderId)
        {
            var order = await GetById(carOrderId);

            if (order != null)
            {
                foreach (var booking in order.CarBookings)
                {
                    //_databaseContext.CarBookingStatuses.Entry(booking.BookingStatus!).State = EntityState.Detached;
                    booking.BookingStatus = CarBookingStatusEntity.CreateSeedObject(CarBookingStatus.Canceled);
                    _databaseContext.CarBookingStatuses.Entry(booking.BookingStatus!).State = EntityState.Unchanged;

                    //_databaseContext.CarRentalStatuses.Entry(booking.Car!.RentalStatus!).State = EntityState.Detached;
                    booking.Car!.RentalStatus = CarRentalStatusEntity.CreateSeedObject(CarRentalStatus.Available);
                    _databaseContext.CarRentalStatuses.Entry(booking.Car!.RentalStatus).State = EntityState.Unchanged;
                }

                order.OrderStatus = OrderStatusEntity.CreateSeedObject(OrderStatus.Canceled);
                _databaseContext.OrderStatuses.Entry(order.OrderStatus).State = EntityState.Unchanged;

                await _databaseContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to delete an order from the database.
        /// </summary>
        /// <param name="id">The ID of the entity to delete.</param>
        /// <returns>A <see cref="Task"/> object.</returns>
        public Task<bool> DeleteOrder(int id)
        {
            return Task.Run(() => _databaseContext.CarOrders.Where(x => x.CarOrderId == id).ExecuteDelete() > 0);
        }

        #endregion
    }
}
