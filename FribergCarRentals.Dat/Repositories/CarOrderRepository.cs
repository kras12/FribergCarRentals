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
    /// A repository class that handles the car entity.
    /// </summary>
    /// <remarks>This repository class works on detached entities. All fetched entities will not be tracked by EF Core.</remarks>
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
        /// Adds a car to the database.
        /// </summary>
        /// <param name="entity">The car to add.</param>
        /// <returns>A <see cref="Task"/> object.</returns>
        public override async Task AddAsync(CarOrderEntity entity)
        {
            // Instead of adding the order first and manually change the tracking state of all existing entities in the database, 
            // we instead attach existing entities first and modify the tracking states later as necessary. 
            entity.CarBookings.ForEach(booking =>
            {
                _databaseContext.Attach(booking.Car!);
                _databaseContext.Entry(booking.Car!).State = EntityState.Modified;  // The rental status will have changed
            });
            entity.Customer!.Orders.Clear();                                        // Cars from other others will cause conflicts
            _databaseContext.Attach(entity.Customer!);
            await _databaseContext.CarOrders.AddAsync(entity);
            _databaseContext.Entry(entity.OrderStatus!).State = EntityState.Unchanged;
            await _databaseContext.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes an order in the database.
        /// </summary>
        /// <param name="id">The ID of the order to delete.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public async Task DeleteOrderAsync(int id)
        {
            var order = await _databaseContext.CarOrders.IgnoreAutoIncludes()
                .Include(x => x.OrderStatus).Include(x => x.CarBookings).ThenInclude(x => x.Car).ThenInclude(x => x.RentalStatus)
                .Where(x => x.CarOrderId == id).FirstOrDefaultAsync();

            if (order is not null)
            {
                MarkCarsAsRentable(order);
                _databaseContext.CarOrders.Remove(order);
                await _databaseContext.SaveChangesAsync();
                return;
            }

            throw new Exception("The order could not be found.");
        }

        /// <summary>
        /// Returns all orders that belongs to a customer.
        /// </summary>
        /// <param name="customerId">The ID of the customer to fetch the orders for.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the orders found.</returns>
        public async Task<IEnumerable<CarOrderEntity>> GetAllByCustomer(int customerId)
        {
            return await _databaseContext.CarOrders.AsNoTracking().Where(x => x.Customer.UserId == customerId).ToListAsync();
        }

        /// <summary>
        /// Gets a car by ID.
        /// </summary>
        /// <remarks>Returned entities will not be tracked by EF Core.</remarks>
        /// <param name="id">The ID of the car.</param>
        /// <returns>A <see cref="Task"/> object containg the entity.</returns>
        public override Task<CarOrderEntity?> GetByIdAsync(int id)
        {
            return _databaseContext.CarOrders.AsNoTracking().SingleOrDefaultAsync(x => x.CarOrderId == id);
        }

        /// <summary>
        /// Attempts to cancel a car order. The order can only be canceled if the status is 'Created'.
        /// </summary>
        /// <param name="id">The ID of the order to cancel.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing true if the order was canceled, or false if the order was not cancellable (wrong status).</returns>
        public async Task<bool> TryCancelOrderAsync(int id)
        {
            var order = await _databaseContext.CarOrders.IgnoreAutoIncludes()
                .Include(x => x.OrderStatus).Include(x => x.CarBookings).ThenInclude(x => x.Car).ThenInclude(x => x.RentalStatus)
                .Where(x => x.CarOrderId == id).FirstOrDefaultAsync();

            if (order is not null && order.OrderStatus!.StatusType == OrderStatus.Created)
            {
                MarkCarsAsRentable(order);
                SetOrderStatus(order, OrderStatus.Canceled);
                return await _databaseContext.SaveChangesAsync() > 0;
            }

            return false;
        }

        /// <summary>
        /// Attempts to complete an order in the database. The order can only be completed if the status is 'Created'.
        /// </summary>
        /// <param name="id">The ID of the order to complete.</param>
        /// <returns>A <see cref="Task"/> containing true if the order was completed, or false if the order was not completable (wrong status).</returns>
        public async Task<bool> TryCompleteOrderAsync(int id)
        {
            var order = await _databaseContext.CarOrders.IgnoreAutoIncludes()
                .Include(x => x.OrderStatus).Include(x => x.CarBookings).ThenInclude(x => x.Car).ThenInclude(x => x.RentalStatus)
                .Where(x => x.CarOrderId == id).FirstOrDefaultAsync();

            if (order is not null && order.OrderStatus!.StatusType == OrderStatus.Created)
            {
                MarkCarsAsRentable(order);
                SetOrderStatus(order, OrderStatus.Completed);
                return await _databaseContext.SaveChangesAsync() > 0;
            }

            return false;
        }

        /// <summary>
        /// Marks cars in an order as rentable without saving the changes to the database.
        /// </summary>
        /// <param name="order">The order.</param>
        private void MarkCarsAsRentable(CarOrderEntity order)
        {
            foreach (var booking in order.CarBookings)
            {
                // If we run the code when the status is already set to rentable, EF Core will start to track another entity with the same ID and throw an error. 
                if (booking.Car!.RentalStatus!.StatusType != RentalCarStatus.Rentable)
                {
                    booking.Car!.RentalStatus = CarRentalStatusEntity.CreateFromType(RentalCarStatus.Rentable);
                    _databaseContext.CarRentalStatuses.Entry(booking.Car!.RentalStatus).State = EntityState.Unchanged;
                }
            }
        }

        /// <summary>
        /// Sets the order status for an order without saving the changes to the database.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="status">The status to set.</param>
        private void SetOrderStatus(CarOrderEntity order, OrderStatus status)
        {
            var trackedStatus = _databaseContext.OrderStatuses.Local.Where(x => x.StatusType == status).FirstOrDefault();

            if (trackedStatus != null)
            {
                order.OrderStatus = trackedStatus;
            }
            else
            {
                order.OrderStatus = OrderStatusEntity.CreateFromType(status);
            }

            _databaseContext.OrderStatuses.Entry(order.OrderStatus).State = EntityState.Unchanged;
        }

        #endregion
    }
}
