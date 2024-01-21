using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Models.User
{
    public abstract class UserCreateViewModel
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        protected UserCreateViewModel()
        {

        }

        #endregion

        #region Properties

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

        /// <summary>
        /// The password for the user.
        /// </summary>
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        #endregion
    }
}
