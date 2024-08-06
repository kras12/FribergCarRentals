namespace FribergCarRentals.Shared.Models.Dto.Order
{
    /// <summary>
    /// A DTO class for order statuses.
    /// </summary>
    public class OrderStatusDto
    {
        #region Properties

        /// <summary>
        /// The ID for the status.
        /// </summary>
        public int OrderStatusId { get; private set; }

        /// <summary>
        /// The description for the status.
        /// </summary>
        public string StatusDescription { get; private set; } = "";

        /// <summary>
        /// The name for the status.
        /// </summary>
        public string StatusName { get; private set; } = "";

        #endregion
    }
}
