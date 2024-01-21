using System.ComponentModel;
using FribergCarRentals.Models.User;

namespace FribergCarRentals.Models.Customer
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
