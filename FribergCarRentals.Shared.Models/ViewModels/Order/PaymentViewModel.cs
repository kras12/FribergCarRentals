namespace FribergCarRentals.Shared.Models.ViewModels.Order
{
    /// <summary>
    /// A view model class that handles a payment.
    /// </summary>
    public class PaymentViewModel
    {
        #region Properties

        /// <summary>
        /// The amount paid.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// The order the payment belongs to. 
        /// </summary>
        public OrderViewModel Order { get; set; } = new();

        /// <summary>
        /// The details of the payment.
        /// </summary>
        public string PaymentDetails { get; set; } = "";

        /// <summary>
        /// The id of the payment.
        /// </summary>
        public int PaymentId { get; private set; }

        #endregion
    }
}
