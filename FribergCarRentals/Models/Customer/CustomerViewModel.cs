using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.Models.User;
using System.ComponentModel;

namespace FribergCarRentals.Models.Customer
{
    public class CustomerViewModel : UserViewModel
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public CustomerViewModel() : base(0, "", "", "")
        {

        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="customer">The customer to copy data from.</param>
        public CustomerViewModel(CustomerEntity customer) :
            base(customer.UserId, customer.FirstName, customer.LastName, customer.Email)
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// A collection of orders for the customer. 
        /// </summary>
        public List<CarOrderEntity> Orders { get; } = new();

        #endregion
    }
}
