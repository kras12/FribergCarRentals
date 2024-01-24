using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.DataAccess.DatabaseContexts;
using FribergCarRentals.DataAccess.Types;

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
            entity.CarBookings.ForEach(booking => _databaseContext.Entry(booking!.Car!.RentalStatus!).State = EntityState.Unchanged);

            await _databaseContext.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Attempts to cancel a car order. The order can only be completeded if the status is 'Created'
        /// </summary>
        /// <param name="id">The ID of the order to cancel.</param>
        /// <returns>True if the order was canceled.</returns>
        public async Task<bool> CancelOrder(int id)
        {
            var order = _databaseContext.CarOrders
                .Where(x => x.CarOrderId == id && x.OrderStatus == OrderStatusEntity.CreateSeedObject(OrderStatus.Created))
                .FirstOrDefault();

            if (order != null)
            {
                foreach (var booking in order.CarBookings)
                {
                    // If we run the code when the status is already set to rentable, EF Core will start to track another entity with the same ID and throw an error. 
                    if (booking.Car!.RentalStatus!.StatusType != RentalCarStatus.Rentable)
                    {
                        booking.Car!.RentalStatus = CarRentalStatusEntity.CreateSeedObject(RentalCarStatus.Rentable);
                        _databaseContext.CarRentalStatuses.Entry(booking.Car!.RentalStatus).State = EntityState.Unchanged;
                    }
                }

                order.OrderStatus = OrderStatusEntity.CreateSeedObject(OrderStatus.Canceled);
                _databaseContext.OrderStatuses.Entry(order.OrderStatus).State = EntityState.Unchanged;

                await _databaseContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to complete an order in the database. The order can only be completeded if the status is 'Created'.
        /// </summary>
        /// <param name="id">The ID of the order to complete.</param>
        /// <returns>A <see cref="Task"/> object containing true if the operation was successful.</returns>
        public async Task<bool> CompleteOrder(int id)
        {
            var order = _databaseContext.CarOrders
                .Where(x => x.CarOrderId == id && x.OrderStatus == OrderStatusEntity.CreateSeedObject(OrderStatus.Created))
                .FirstOrDefault();

            if (order is not null)
            {
                order.OrderStatus = OrderStatusEntity.CreateSeedObject(OrderStatus.Completed);
                _databaseContext.Entry(order.OrderStatus).State = EntityState.Unchanged;

                foreach (var booking in order.CarBookings)
                {
                    booking.Car!.RentalStatus = CarRentalStatusEntity.CreateSeedObject(RentalCarStatus.Rentable);
                    _databaseContext.Entry(booking.Car.RentalStatus).State = EntityState.Unchanged;
                }

                return await _databaseContext.SaveChangesAsync() > 0;
            }

            return false;
        }

        /// <summary>
        /// Attempts to delete an order in the database.
        /// </summary>
        /// <param name="id">The ID of the order to delete.</param>
        /// <returns>A <see cref="Task"/> object containing true if the operation was successful.</returns>
        public async Task<bool> DeleteOrder(int id)
        {
            // If the order is cancelable it will be done and the car will get rentable again.
            await CancelOrder(id);
            return await _databaseContext.CarOrders.Where(x => x.CarOrderId == id).ExecuteDeleteAsync() > 0;
        }

        #endregion
    }
}
