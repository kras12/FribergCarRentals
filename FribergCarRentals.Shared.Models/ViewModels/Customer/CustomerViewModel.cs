using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FribergCarRentals.Shared.Constants;
using FribergCarRentals.Shared.Models.ViewModels.User;

namespace FribergCarRentals.Shared.Models.ViewModels.Customer
{
    /// <summary>
    /// A view model class that handles data for a customer.
    /// </summary>
    public class CustomerViewModel : UserViewModelBase
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

        /// <summary>
        /// Returns true if the email address is confirmed.
        /// </summary>
        public bool IsEmailConfirmed { get; }

        /// <summary>
        /// The number of orders the customer have.
        /// </summary>
        [DisplayName("Orders")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = ValidationRules.DefaultIntegerNumberOutputFormatString)]
        public int OrderCount { get; set; }

        #endregion
    }
}
