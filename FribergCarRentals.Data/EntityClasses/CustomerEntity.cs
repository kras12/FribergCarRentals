using FribergCarRentals.DataAccess.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FribergCarRentals.DataAccess.EntityClasses
{
    /// <summary>
    /// An entity class that represents a customer.
    /// </summary>
    public class CustomerEntity : UserEntity
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public CustomerEntity() : base(UserRoleEntity.CreateFromType(UserRoleType.Customer))
        {

        }

        /// <summary>
        ///  A constructor.
        /// </summary>
        /// <param name="userId">The ID for the user.</param>
        /// <param name="firstName">The first name for the user.</param>
        /// <param name="lastName">The last name for the user.</param>
        /// <param name="email">The email address for the user.</param>
        /// <param name="hashedPassword">The hashed password for the user.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public CustomerEntity(int userId, string firstName, string lastName, string email, string hashedPassword) :
            base(userId, firstName, lastName, email, hashedPassword, UserRoleEntity.CreateFromType(UserRoleType.Customer))
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// A collection of orders for the customer. 
        /// </summary>
        public List<CarOrderEntity> Orders { get; } = new();

        /// <summary>
        /// The user role for the customer.
        /// </summary>
        public override UserRoleEntity UserRole
        {
            get
            {
                return base.UserRole;
            }

            set
            {
                // Safe guard against invalid model bindings.
                if (value.UserRoleType != UserRoleType.Customer)
                {
                    throw new ArgumentException("Invalid user role assignment.");
                }

                base.UserRole = value;
            }
        }

        #endregion
    }
}
