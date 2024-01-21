using System.ComponentModel;
using FribergCarRentals.Data.User;
using FribergCarRentals.DataAccess.EntityClasses;

namespace FribergCarRentals.Data.Customer
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
