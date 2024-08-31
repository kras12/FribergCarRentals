namespace FribergCarRentals.Shared.Models.Dto.Order
{
    /// <summary>
    /// A DTO class for payments.
    /// </summary>
    public class PaymentDto
    {
        #region Properties

        /// <summary>
        /// The amount paid.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// The order the payment belongs to. 
        /// </summary>
        public CarOrderDto? Order { get; set; }

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
