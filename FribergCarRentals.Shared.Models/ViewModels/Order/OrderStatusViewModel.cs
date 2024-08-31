using FribergCarRentals.Shared.Enums;

namespace FribergCarRentals.Shared.Models.ViewModels.Order
{
    /// <summary>
    /// A view model class that handles an order status.
    /// </summary>
    public class OrderStatusViewModel
    {
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
