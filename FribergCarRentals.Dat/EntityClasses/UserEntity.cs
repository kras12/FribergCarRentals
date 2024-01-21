using FribergCarRentals.Data.SharedClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentals.DataAccess.EntityClasses
{
    #region Enums

    public enum UserRoleType
    {
        [EnumDatabaseValue("None", DescriptionValue = "No role.")]
        None = 0,
        [EnumDatabaseValue("Admin", DescriptionValue = "Admin role.")]
        Admin,
        [EnumDatabaseValue("Customer", DescriptionValue = "Customer role.")]
        Customer,
    }

    #endregion

    #region Classes

    public abstract class UserEntity
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="userRole">The user role.</param>
        protected UserEntity(UserRoleEntity userRole)
        {
            UserRole = userRole;
        }

        /// <summary>
        ///  A constructor.
        /// </summary>
        /// <param name="userId">The ID for the user. Can't be negative.</param>
        /// <param name="firstName">The first name for the user.</param>
        /// <param name="lastName">The last name for the user.</param>
        /// <param name="email">The email address for the user.</param>
        /// <param name="password">The password for the user.</param>
        /// <param name="userRole">The user role.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        protected UserEntity(int userId, string firstName, string lastName, string email, string password, UserRoleEntity userRole)
        {
            #region Checks

            if (userId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId), $"The value of parameter '{userId}' can't be negative.");
            }

            if (firstName is null)
            {
                throw new ArgumentNullException(nameof(firstName), $"The value of parameter '{firstName}' can't be null");
            }

            if (lastName is null)
            {
                throw new ArgumentNullException(nameof(lastName), $"The value of parameter '{lastName}' can't be null");
            }

            if (email is null)
            {
                throw new ArgumentNullException(nameof(email), $"The value of parameter '{email}' can't be null");
            }

            if (password is null)
            {
                throw new ArgumentNullException(nameof(password), $"The value of parameter '{password}' can't be null");
            }

            #endregion

            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            UserRole = userRole;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The ID for the user.
        /// </summary>
        [Key]
        public int UserId { get; set; }

        /// <summary>
        /// The first name for the user.
        /// </summary>
        public string FirstName { get; set; } = "";

        /// <summary>
        /// The last name for the user.
        /// </summary>
        public string LastName { get; set; } = "";

        /// <summary>
        /// The email address for the user.
        /// </summary>
        public string Email { get; set; } = "";

        /// <summary>
        /// The password for the user.
        /// </summary>
        public string Password { get; set; } = "";

        /// <summary>
        /// The user role.
        /// </summary>
        public virtual UserRoleEntity UserRole { get; set; }

        #endregion
    }

    #endregion
}
