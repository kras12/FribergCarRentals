using FribergCars.Shared.SharedTypes;
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
        /// A constructor intended for EF core.
        /// </summary>
        /// <param name="paymentId">The id of the entity in the database. Can't be negative.</param>
        /// <param name="amount">The amount paid.</param>
        /// <param name="paymentDetails">The details of the payment</param>
        private PaymentEntity(int paymentId, decimal amount, string paymentDetails)
        {
            #region Checks

            if (paymentId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paymentId), $"The value of parameter '{paymentId}' can't be negative.");
            }

            if (paymentDetails is null)
            {
                throw new ArgumentNullException(nameof(paymentDetails), $"The value of parameter '{paymentDetails}' can't be null.");
            }

            #endregion

            Amount = amount;
            PaymentId = paymentId;

            // EF Core can't set navigational properties through a constructor, 
            // so these will be setby EF Core via the properties after the constructor have run. 
            Customer = null!;
        }

        /// <summary>
        /// A constructor intended for EF core.
        /// </summary>
        /// <param name="customer">The customer that made the payment. </param>
        /// <param name="amount">The amount paid.</param>
        /// <param name="paymentDetails">The details of the payment</param>
        public PaymentEntity(CustomerEntity customer, decimal amount, string paymentDetails) : 
            this(paymentId: 0, amount, paymentDetails)
        {
            #region Checks

            if (customer is null)
            {
                throw new ArgumentNullException(nameof(customer), $"The value of parameter '{customer}' can't be null.");
            }

            #endregion

            // EF Core can't set navigational properties through a constructor, 
            // so these values will have to be set in this constructor.
            Customer = customer;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The amount paid.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// The customer that made the payment. 
        /// </summary>
        public CustomerEntity Customer { get; set; }

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
