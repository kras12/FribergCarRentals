using FribergCarRentals.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentals.DataAccess.EntityClasses
{
    public class AdminEntity : UserEntity
    {
        #region Constructors
        
        /// <summary>
        /// A constructor.
        /// </summary>
        public AdminEntity()
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
        /// <param name="userRole">The user role.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public AdminEntity(int userId, string firstName, string lastName, string email, string hashedPassword, UserRole userRole) : 
            base(userId, firstName, lastName, email, hashedPassword, userRole)
        {

        }

        #endregion
    }
}