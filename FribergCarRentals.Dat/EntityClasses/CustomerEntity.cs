using FribergCarRentals.Data.EntityClasses;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FribergCarRentals.Models
{
    public class CustomerEntity : UserEntity
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public CustomerEntity() : base(UserRoleEntity.CreateSeedObject(UserRoleType.Customer))
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
        public CustomerEntity(int userId, string firstName, string lastName, string email, string hashedPassword) :
            base(userId, firstName, lastName, email, hashedPassword, UserRoleEntity.CreateSeedObject(UserRoleType.Customer))
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// A collection of orders for the customer. 
        /// </summary>
        public List<CarOrderEntity> Orders { get; } = new();

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
                    throw new InvalidOperationException("Invalid user role assignment.");
                }

                base.UserRole = value;
            }
        }

        #endregion
    }
}
