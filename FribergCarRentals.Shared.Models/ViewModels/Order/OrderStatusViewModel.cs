using FribergCarRentals.Shared.Enums;

namespace FribergCarRentals.Shared.Models.ViewModels.Order
{
    /// <summary>
    /// A view model class that handles an order status.
    /// </summary>
    public class OrderStatusViewModel
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public OrderStatusViewModel()
        {

        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="orderStatusId">The database ID for the entity.</param>
        /// <param name="statusName">The status name.</param>
        /// <param name="statusDescription">The status description.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        private OrderStatusViewModel(OrderStatus orderStatusId, string statusName, string statusDescription)
        {
            #region Checks

            if (orderStatusId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(orderStatusId), $"The value of parameter '{orderStatusId}' can't be negative.");
            }

            if (statusName is null)
            {
                throw new ArgumentNullException(nameof(statusName), $"The value of parameter '{statusName}' can't be null.");
            }

            if (statusDescription is null)
            {
                throw new ArgumentNullException(nameof(statusDescription), $"The value of parameter '{statusDescription}' can't be null.");
            }

            #endregion

            OrderStatusId = orderStatusId;
            StatusName = statusName;
            StatusDescription = statusDescription;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The ID for the status.
        /// </summary>
        public OrderStatus OrderStatusId { get; private set; }

        /// <summary>
        /// The description for the status.
        /// </summary>
        public string StatusDescription { get; private set; } = "";

        /// <summary>
        /// The name for the status.
        /// </summary>
        public string StatusName { get; private set; } = "";

        /// <summary>
        /// The type for the status.
        /// </summary>
        public OrderStatus StatusType
        {
            get
            {
                return OrderStatusId;
            }
        }

        #endregion
    }
}
