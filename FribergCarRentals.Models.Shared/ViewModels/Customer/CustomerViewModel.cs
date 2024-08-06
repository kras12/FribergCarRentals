using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FribergCarRentals.Shared.Constants;
using FribergCarRentals.Shared.ViewModels.User;

namespace FribergCarRentals.Shared.ViewModels.Customer
{
    /// <summary>
    /// A view model class that handles data for a customer.
    /// </summary>
    public class CustomerViewModel : UserViewModel
    {
        #region Constructors

        /// <summary>
        ///  A constructor.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="userId">The customer ID.</param>
        /// <param name="firstName">The first name for the user.</param>
        /// <param name="lastName">The last name for the user.</param>
        /// <param name="email">The email address for the user.</param>
        /// <param name="isEmailConfirmed">Set to true if the email address is confirmed.</param>
        /// <param name="orderCount">The number of orders the customer has.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        protected CustomerViewModel(string userId, int customerId, string firstName, string lastName, string email, bool isEmailConfirmed, int orderCount)
            : base(userId, firstName, lastName, email, isEmailConfirmed)
        {

            #region Checks

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentOutOfRangeException(nameof(userId), $"The value of parameter '{userId}' can't null or empty.");
            }

            if (customerId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId), $"The value of parameter '{customerId}' must be larger than zero.");
            }

            #endregion

            CustomerId = customerId;
            OrderCount = orderCount;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The ID for the customer.
        /// </summary>
        [DisplayName("Customer ID")]
        public int CustomerId { get; set; }

        /// <summary>
        /// The full name for the customer.
        /// </summary>
        [DisplayName("Customer Name")]
        public override string FullName
        {
            get
            {
                return base.FullName;
            }
        }

        /// <summary>
        /// The number of orders the customer have.
        /// </summary>
        [DisplayName("Orders")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = ValidationRules.DefaultIntegerNumberOutputFormatString)]
        public int OrderCount { get; set; }

        #endregion
    }
}
