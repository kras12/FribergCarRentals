using FribergCarRentals.Shared.Models.ViewModels.Customer;

namespace FribergCarRentals.Models.Customer
{
    public class RegisterOrLoginCustomerViewModel
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public RegisterOrLoginCustomerViewModel()
        {

        }

        #endregion

        #region Properties

        public RegisterCustomerViewModel RegisterCustomerViewModel { get; set; } = new();

        public LoginCustomerViewModel LoginCustomerViewModel { get; set; } = new();

        #endregion
    }
}
