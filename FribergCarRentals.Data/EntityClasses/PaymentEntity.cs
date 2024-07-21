using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Data.EntityClasses
{
    /// <summary>
    /// An entity class that represents a payment.
    /// </summary>
    public class PaymentEntity
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public PaymentEntity()
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
        public PaymentEntity(CarOrderEntity order, decimal amount, string paymentDetails)
        {
            #region Checks

            if (order is null)
            {
                throw new ArgumentNullException(nameof(order), $"The value of parameter '{order}' can't be null.");
            }

            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), $"The value of parameter '{amount}' can't be negative.");
            }

            if (paymentDetails is null)
            {
                throw new ArgumentNullException(nameof(paymentDetails), $"The value of parameter '{paymentDetails}' can't be null.");
            }

            #endregion

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
        [Required]
        public CarOrderEntity? Order { get; set; }

        /// <summary>
        /// The details of the payment.
        /// </summary>
        public string PaymentDetails { get; set; } = "";

        /// <summary>
        /// The id of the payment.
        /// </summary>
        [Key]
        public int PaymentId { get; private set; }

        #endregion
    }
}
