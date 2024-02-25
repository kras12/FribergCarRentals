using FribergCarRentals.DataAccess.EntityClasses;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Models.User
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
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        protected UserViewModel(int userId, string firstName, string lastName, string email) : base(firstName, lastName, email)
        {
            #region Checks

            if (userId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId), $"The value of parameter '{userId}' can't be negative.");
            }

            #endregion

            UserId = userId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The ID for the user.
        /// </summary>
        [DisplayName("ID")]
        [BindNever]
        public virtual int UserId { get; }

        #endregion
    }
}
