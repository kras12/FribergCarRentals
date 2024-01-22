using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FribergCarRentals.DataAccess.EntityClasses;

namespace FribergCarRentals.DataAccess.Repositories
{
    #region Interfaces

    /// <summary>
    /// An interface for a car order repository.
    /// </summary>
    public interface ICarOrderRepository : IRepositoryBase<CarOrderEntity>
    {
        #region Methods

        /// <summary>
        /// Attempts to cancel a car order.
        /// </summary>
        /// <param name="carOrderId">The ID of the order to cancel.</param>
        /// <returns>True if the order was canceled.</returns>
        public Task<bool> CancelOrder(int carOrderId);

        /// <summary>
        /// Attempts to complete an order in the database. The order can only be completeded if the status is 'Created'.
        /// </summary>
        /// <param name="id">The ID of the order to complete.</param>
        /// <returns>A <see cref="Task"/> object containing true if the operation was successful.</returns>
        public Task<bool> CompleteOrder(int id);

        /// <summary>
        /// Attempts to delete an order in the database.
        /// </summary>
        /// <param name="id">The ID of the order to delete.</param>
        /// <returns>A <see cref="Task"/> object containing true if the operation was successful.</returns>
        public Task<bool> DeleteOrder(int id);

        #endregion
    }

    #endregion


}
