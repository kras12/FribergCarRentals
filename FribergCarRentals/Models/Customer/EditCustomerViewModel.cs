using System.ComponentModel;
using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.Models.User;

namespace FribergCarRentals.Models.Customer
{
    /// <summary>
    /// A view model class that handles data related to editing a customer.
    /// </summary>
    public class EditCustomerViewModel : UserEditViewModel
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public EditCustomerViewModel() : this(0, "", "", "")
        {

        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="customer">The customer to model.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public EditCustomerViewModel(CustomerEntity customer) :
            this(customer.UserId, customer.FirstName, customer.LastName, customer.Email)
        {

        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="userId">The ID for the customer.</param>
        /// <param name="firstName">The first name for the customer.</param>
        /// <param name="lastName">The last name for the customer.</param>
        /// <param name="email">The email for the customer.</param>
        public EditCustomerViewModel(int userId, string firstName, string lastName, string email) :
            base(userId, firstName, lastName, email)
        {
            PageSubTitle = $"#{UserId} - {FullName}";
        }

        #endregion
    }
}
