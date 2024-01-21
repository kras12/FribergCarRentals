using FribergCarRentals.DataAccess.EntityClasses;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Data.User
{
    public abstract class UserViewModel : ViewModelBase
    {
        #region Constructors

        /// <summary>
        ///  A constructor.
        /// </summary>
        /// <param name="userId">The ID for the user. Can't be negative.</param>
        /// <param name="firstName">The first name for the user.</param>
        /// <param name="lastName">The last name for the user.</param>
        /// <param name="email">The email address for the user.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        protected UserViewModel(int userId, string firstName, string lastName, string email)
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

            #endregion

            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The ID for the user.
        /// </summary>
        [DisplayName("ID")]
        public int UserId { get; set; }

        /// <summary>
        /// The first name for the user.
        /// </summary>
        [DisplayName("First Name")]
        public string FirstName { get; set; } = "";

        /// <summary>
        /// Returns the full name of the customer.
        /// </summary>
        [BindNever]
        [DisplayName("Full Name")]
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        /// <summary>
        /// The last name for the user.
        /// </summary>
        [DisplayName("Last Name")]
        public string LastName { get; set; } = "";

        /// <summary>
        /// The email address for the user.
        /// </summary>
        [DisplayName("Email")]
        public string Email { get; set; } = "";

        #endregion
    }
}
