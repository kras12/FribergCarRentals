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
            this(customer.UserId, customer.FirstName, customer.LastName, customer.Email, customer.HashedPassword)
        {
            
        }

        /// <summary>
        ///  A constructor.
        /// </summary>
        /// <param name="userId">The ID for the user. Can't be negative.</param>
        /// <param name="firstName">The first name for the user.</param>
        /// <param name="lastName">The last name for the user.</param>
        /// <param name="email">The email address for the user.</param>
        /// <param name="hashedPassword">The hashed password for the user.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public CustomerViewModel(int userId, string firstName, string lastName, string email, string hashedPassword) :
            base(userId, firstName, lastName, email, hashedPassword)
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
