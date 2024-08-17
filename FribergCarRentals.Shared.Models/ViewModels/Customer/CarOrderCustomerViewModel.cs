using System.ComponentModel;
using FribergCarRentals.Shared.Models.ViewModels.User;

namespace FribergCarRentals.Shared.Models.ViewModels.Customer
{
    /// <summary>
    /// A view model class that contains customer data for an order. 
    /// </summary>
    public class CarOrderCustomerViewModel : UserViewModelBase
    {
        #region Constructors

        /// <summary>
        ///  A constructor.
        /// </summary>
        /// <param name="customerId">The customer ID.</param>
        /// <param name="firstName">The first name for the user.</param>
        /// <param name="lastName">The last name for the user.</param>
        /// <param name="email">The email address for the user.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public CarOrderCustomerViewModel(int customerId, string firstName, string lastName, string email)
            : base(firstName, lastName, email)
        {
            #region Checks

            if (customerId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(customerId), $"The value of parameter '{customerId}' must be larger than zero.");
            }

            #endregion

            CustomerId = customerId;
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

        #endregion
    }
}
