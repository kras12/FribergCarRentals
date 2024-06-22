using FribergCarRentals.Data.EntityClasses;

namespace FribergCarRentals.Data.Repositories
{
    #region Interfaces

    /// <summary>
    /// An interface for a car order repository.
    /// </summary>
    public interface ICarOrderRepository : IGenericRepository<CarOrderEntity>
    {
        #region Methods

        /// <summary>
        /// Deletes an order in the database.
        /// </summary>
        /// <param name="id">The ID of the order to delete.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public Task DeleteOrderAsync(int id);

        /// <summary>
        /// Returns all orders that belongs to a customer.
        /// </summary>
        /// <param name="customerId">The ID of the customer to fetch the orders for.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the orders found.</returns>
        public Task<IEnumerable<CarOrderEntity>> GetAllByCustomer(int customerId);

        /// <summary>
        /// Attempts to cancel a car order. The order can only be canceled if the status is 'Created'.
        /// </summary>
        /// <param name="carOrderId">The ID of the order to cancel.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing true if the order was canceled, or false if the order was not cancellable (wrong status).</returns>
        public Task<bool> TryCancelOrderAsync(int carOrderId);

        /// <summary>
        /// Attempts to complete an order in the database. The order can only be completed if the status is 'Created'.
        /// </summary>
        /// <param name="id">The ID of the order to complete.</param>
        /// <returns>A <see cref="Task"/> containing true if the order was completed, or false if the order was not completable (wrong status).</returns>
        public Task<bool> TryCompleteOrderAsync(int id);

        #endregion
    }

    #endregion


}
