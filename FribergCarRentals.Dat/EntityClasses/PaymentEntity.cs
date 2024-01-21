using FribergCarRentals.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Models
{
    /// <summary>
    /// A class that represents a payment.
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
        /// A constructor intended for EF core.
        /// </summary>
        /// <param name="order">The order the payment belongs to.</param>
        /// <param name="amount">The amount paid.</param>
        /// <param name="paymentDetails">The details of the payment</param>
        public PaymentEntity(CarOrderEntity order, decimal amount, string paymentDetails)
        {
            #region Checks

            if (order is null)
            {
                throw new ArgumentNullException(nameof(order), $"The value of parameter '{order}' can't be null.");
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
        /// The id of the entity in the database.
        /// </summary>
        [Key]
        public int PaymentId { get; private set; }

        /// <summary>
        /// The details of the payment.
        /// </summary>
        public string PaymentDetails { get; set; } = "";

        #endregion
    }
}
