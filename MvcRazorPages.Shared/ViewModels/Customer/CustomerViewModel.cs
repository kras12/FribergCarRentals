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
            this(customer.User.Id, customer.CustomerId, customer.User.FirstName!, customer.User.LastName, customer.User.Email!, customer.Orders.Count())
        {
            OrderCount = customer.Orders.Count();
        }

        /// <summary>
        ///  A constructor.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="userId">The customer ID.</param>
        /// <param name="firstName">The first name for the user.</param>
        /// <param name="lastName">The last name for the user.</param>
        /// <param name="email">The email address for the user.</param>
        /// <param name="orderCount">The number of orders the customer has.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        protected CustomerViewModel(string userId, int customerId, string firstName, string lastName, string email, int orderCount) 
            #region Checks

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentOutOfRangeException(nameof(userId), $"The value of parameter '{userId}' can't null or empty.");
            }

            if (customerId <= 0)
        {
                throw new ArgumentOutOfRangeException(nameof(userId), $"The value of parameter '{customerId}' must be larger than zero.");
            }

            #endregion

            CustomerId = customerId;
            OrderCount = orderCount;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The ID for the customer.
        /// </summary>
        [DisplayName("Customer ID")]
        [BindNever]
        public int CustomerId { get; }

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

        #endregion
    }
}
