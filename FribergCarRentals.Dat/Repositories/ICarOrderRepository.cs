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
        /// Attempts to delete an order from the database.
        /// </summary>
        /// <param name="id">The ID of the entity to delete.</param>
        /// <returns>A <see cref="Task"/> object.</returns>
        public Task<bool> DeleteOrder(int id);

        #endregion
    }

    #endregion


}
