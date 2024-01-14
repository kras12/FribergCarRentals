using FribergCarRentals.DataAccess.EntityClasses;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Models
{
    public abstract class UserViewModel
    {
        #region Constructors

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
        protected UserViewModel(int userId, string firstName, string lastName, string email, string hashedPassword)
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

            if (hashedPassword is null)
            {
                throw new ArgumentNullException(nameof(hashedPassword), $"The value of parameter '{hashedPassword}' can't be null");
            }

            #endregion

            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            InputPassword = hashedPassword;
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
        /// The input password for the user.
        /// </summary>
        /// <remarks>Is used to input data to form fields and get data from form submissions.</remarks>
        [DisplayName("Password")]
        public string InputPassword { get; set; } = "";

        #endregion
    }
}
