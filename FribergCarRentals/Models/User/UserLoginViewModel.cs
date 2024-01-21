using System.ComponentModel;

namespace FribergCarRentals.Data.User
{
    public abstract class UserLoginViewModel
    {

        #region Constructors

        protected UserLoginViewModel()
        {

        }

        protected UserLoginViewModel(string email, string password)
        {
            #region Checks

            if (email is null)
            {
                throw new ArgumentNullException(nameof(email), $"The value of parameter '{email}' can't be null.");
            }

            if (password is null)
            {
                throw new ArgumentNullException(nameof(password), $"The value of parameter '{password}' can't be null.");
            }

            #endregion

            Email = email;
            Password = password;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The email address for the user.
        /// </summary>
        [DisplayName("Email")]
        public string Email { get; set; } = "";

        /// <summary>
        /// The password for the user.
        /// </summary>
        [DisplayName("Password")]
        public string Password { get; set; } = "";

        #endregion
    }
}
