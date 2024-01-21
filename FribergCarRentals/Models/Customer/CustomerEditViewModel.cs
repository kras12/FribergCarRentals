using System.ComponentModel;
using FribergCarRentals.Models.User;

namespace FribergCarRentals.Models.Customer
{
    public class CustomerEditViewModel : UserEditViewModel
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public CustomerEditViewModel() : this(0, "", "", "")
        {

        }

        public CustomerEditViewModel(CustomerEntity customer) :
            this(customer.UserId, customer.FirstName, customer.LastName, customer.Email)
        {

        }

        public CustomerEditViewModel(int userId, string firstName, string lastName, string email) :
            base(userId, firstName, lastName, email)
        {

        }

        #endregion
    }
}
