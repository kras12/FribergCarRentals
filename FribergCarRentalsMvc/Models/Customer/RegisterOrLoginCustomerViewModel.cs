using FribergCarRentals.Shared.Models.ViewModels.Customer;

namespace FribergCarRentals.Models.Customer
{
    /// <summary>
    /// View model class that wraps a register and a login customer viewmodel. 
    /// </summary>
    public class RegisterOrLoginCustomerViewModel
    {
        #region Properties

        /// <summary>
        /// The Login customer viewmodel
        /// </summary>
        public LoginCustomerViewModel LoginCustomerViewModel { get; set; } = new();

        /// <summary>
        /// The register customer viewmodel.
        /// </summary>
        public RegisterCustomerViewModel RegisterCustomerViewModel { get; set; } = new();

        #endregion
    }
}
