namespace FribergCarRentals.Shared.ViewModels.Order
{
    /// <summary>
    /// A view model class that handles a payment.
    /// </summary>
    public class PaymentViewModel
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public PaymentViewModel()
        {

        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="order">The order the payment belongs to.</param>
        /// <param name="amount">The amount paid.</param>
        /// <param name="paymentDetails">The details of the payment</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public PaymentViewModel(OrderViewModel order, decimal amount, string paymentDetails)
        {
            Order = order;
            Amount = amount;
            PaymentDetails = paymentDetails;
        }

        #endregion

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
