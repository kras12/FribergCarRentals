using FribergCarRentals.DataAccess.EntityClasses;
using System.ComponentModel;

namespace FribergCarRentals.Models
{
    public class CustomerViewModel : UserViewModel
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="customer">The customer to copy data from.</param>
        public CustomerViewModel(CustomerEntity customer) :
            base(customer.UserId, customer.FirstName, customer.LastName, customer.Email, customer.HashedPassword)
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
