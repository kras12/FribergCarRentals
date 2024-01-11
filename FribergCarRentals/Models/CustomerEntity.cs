using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FribergCarRentals.Models
{
    [Table("Customers")]
    public class CustomerEntity
    {
        #region Constructors

        /// <summary>
        /// A constructor intended for EF core.
        /// </summary>
        /// <param name="customerId">The database ID for the entity.</param>
        /// <param name="firstName">The first name for the customer.</param>
        /// <param name="lastName">The last name for the customer.</param>
        /// <param name="email">The email address for the customer.</param>
        /// <param name="hashedPassword">The hashed password for the customer.</param>
        /// <param name="orders">A collection of orders for the customer.</param>
        private CustomerEntity(int customerId, string firstName, string lastName, string email, string hashedPassword, List<CarOrderEntity> orders)
        {
            CustomerId = customerId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            HashedPassword = hashedPassword;
            Orders = orders;
        }

        /// <summary>
        /// A constructor intended for EF core.
        /// </summary>
        /// <param name="customerId">The database ID for the entity.</param>
        /// <param name="firstName">The first name for the customer.</param>
        /// <param name="lastName">The last name for the customer.</param>
        /// <param name="email">The email address for the customer.</param>
        /// <param name="hashedPassword">The hashed password for the customer.</param>
        /// <param name="orders">A collection of orders for the customer.</param>
        public CustomerEntity(string firstName, string lastName, string email, string hashedPassword, List<CarOrderEntity> orders) : 
            this(customerId: 0, firstName, lastName, email, hashedPassword, orders)
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// The database ID for the entity.
        /// </summary>
        [Key]
        public int CustomerId { get; private set; }

        /// <summary>
        /// The first name for the customer.
        /// </summary>
        public string FirstName { get; set; } = "";

        /// <summary>
        /// The last name for the customer.
        /// </summary>
        public string LastName { get; set; } = "";

        /// <summary>
        /// The email address for the customer.
        /// </summary>
        public string Email { get; set; } = "";

        /// <summary>
        /// The hashed password for the customer.
        /// </summary>
        public string HashedPassword { get; set; } = "";

        /// <summary>
        /// A collection of orders for the customer. 
        /// </summary>
        public List<CarOrderEntity> Orders { get; } = new();

        #endregion
    }
}
