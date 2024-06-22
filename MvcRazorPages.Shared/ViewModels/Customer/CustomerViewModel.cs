using FribergCarRentals.Data.EntityClasses;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using MvcRazorPages.Shared.ViewModels.User;

namespace MvcRazorPages.Shared.ViewModels.Customer
{
    /// <summary>
    /// A view model class that handles data for a customer.
    /// </summary>
    public class CustomerViewModel : UserViewModel
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="customer">The customer to copy data from.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public CustomerViewModel(CustomerEntity customer) :
            base(customer.UserId, customer.FirstName, customer.LastName, customer.Email)
        {
            OrderCount = customer.Orders.Count();
        }

        /// <summary>
        ///  A constructor.
        /// </summary>
        /// <param name="userId">The ID for the user.</param>
        /// <param name="firstName">The first name for the user.</param>
        /// <param name="lastName">The last name for the user.</param>
        /// <param name="email">The email address for the user.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        protected CustomerViewModel(int userId, string firstName, string lastName, string email) : base(userId, firstName, lastName, email)
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// The full name for the customer.
        /// </summary>
        [BindNever]
        [DisplayName("Customer Name")]
        public override string FullName
        {
            get
            {
                return base.FullName;
            }
        }

        /// <summary>
        /// A collection of orders for the customer. 
        /// </summary>
        [DisplayName("Orders")]
        [BindNever]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = DefaultIntegerNumberOutputFormatString )]
        public int OrderCount { get; }

        /// <summary>
        /// The ID for the customer.
        /// </summary>
        [DisplayName("Customer ID")]
        [BindNever]
        public override int UserId
        {
            get
            {
                return base.UserId;
            }
        }

        #endregion
    }
}
