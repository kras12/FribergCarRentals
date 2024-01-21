using System.ComponentModel;
using FribergCarRentals.Data.User;

namespace FribergCarRentals.Data.Customer
{
    public class CustomerRegisterOrLoginViewModel
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public CustomerRegisterOrLoginViewModel()
        {

        }

        #endregion

        #region Properties

        public CustomerRegisterViewModel CustomerCreateViewModel { get; set; } = new();

        public CustomerLoginViewModel CustomerLoginViewModel { get; set; } = new();

        #endregion
    }
}
