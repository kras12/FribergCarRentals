using System.ComponentModel;

namespace FribergCarRentals.Shared.ViewModels.User
{
    /// <summary>
    /// A viewmodel base class that handles data related to an user. 
    /// </summary>
    public abstract class UserViewModel : UserViewModelBase
    {
        #region Constructors

        /// <summary>
        ///  A constructor.
        /// </summary>
        /// <param name="userId">The ID for the user.</param>
        /// <param name="firstName">The first name for the user.</param>
        /// <param name="lastName">The last name for the user.</param>
        /// <param name="email">The email address for the user.</param>
        /// <param name="isEmailConfirmed">Set to true if the email address is confirmed.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        protected UserViewModel(string userId, string firstName, string lastName, string email, bool isEmailConfirmed) : base(firstName, lastName, email)
        {
            #region Checks

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentOutOfRangeException(nameof(userId), $"The value of parameter '{userId}' can't be null or empty.");
            }

            #endregion

            UserId = userId;
            IsEmailConfirmed = isEmailConfirmed;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns true if the email address is confirmed.
        /// </summary>
        public bool IsEmailConfirmed { get; }

        /// <summary>
        /// The ID for the user.
        /// </summary>
        [DisplayName("User ID")]
        public string UserId { get; }

        #endregion
    }
}
