using System.ComponentModel;
using FribergCarRentals.Shared.Models.ViewModels.User;

namespace FribergCarRentals.Shared.Models.ViewModels.Customer
{
    /// <summary>
    /// A view model class that contains customer data for an order. 
    /// </summary>
    public class CarOrderCustomerViewModel : UserViewModelBase
    {
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
