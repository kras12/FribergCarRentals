using System.ComponentModel;
using FribergCarRentals.Models.User;

namespace FribergCarRentals.Models.Customer
{
    public class CustomerLoginViewModel : UserLoginViewModel
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public CustomerLoginViewModel() : base()
        {

        }

        public CustomerLoginViewModel(string email, string password) : base(email, password)
        {

        }

        #endregion
    }
}
