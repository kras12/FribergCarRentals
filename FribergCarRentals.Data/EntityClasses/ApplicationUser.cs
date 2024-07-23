using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FribergFastigheter.Server.Data.Entities
{
    /// <summary>
    /// An entity class that represent a User.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {        
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public ApplicationUser()
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="firstName">The first name of the user.</param>
        /// <param name="lastName">The last name of the user.</param>
        /// <param name="userName">The user name of the user.</param>
        /// <param name="email">The email of the user.</param>
        /// <param name="phoneNumber">The phonenumber of the user.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <param name="emailConfirmed">True to set email as confirmed.</param>
        /// <param name="password">The password to use when creating users.</param>
        public ApplicationUser(string firstName, string lastName, string userName, string email, string phoneNumber, bool emailConfirmed = false, string? password = null)
        {
            #region Checks

            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentException($"The value of parameter '{nameof(firstName)}' can't be null or empty.", nameof(firstName));
            }

            if (string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentException($"The value of parameter '{nameof(lastName)}' can't be null or empty.", nameof(lastName));
            }

            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException($"The value of parameter '{nameof(email)}' can't be null or empty.", nameof(email));
            }

            if (string.IsNullOrEmpty(phoneNumber))
            {
                throw new ArgumentException($"The value of parameter '{nameof(phoneNumber)}' can't be null or empty.", nameof(phoneNumber));
            }

            #endregion

            FirstName = firstName;
            LastName = lastName;
            UserName = userName;            
            Email = email;            
            EmailConfirmed = emailConfirmed;
            PhoneNumber = phoneNumber;
            Password = password;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The first name of the user.
        /// </summary>
        public string FirstName { get; set; } = "";

        /// <summary>
        /// The last name of the user.
        /// </summary>
        public string LastName { get; set; } = "";

        /// <summary>
        /// The password to use when creating or updating users.
        /// </summary>
        [NotMapped]
        public string? Password { get; set; } = null;

        #endregion

    }
}
