using FribergCarRentals.Shared.ViewModels.User;
using System.ComponentModel;

namespace FribergCarRentals.Models.Shared.ViewModels.Admin
{
    /// <summary>
    /// A view model class to handle data for an admin.
    /// </summary>
    public class AdminViewModel : UserViewModel
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public AdminViewModel() : base("0", "", "", "", isEmailConfirmed: false)
        {

        }

        /// <summary>
        ///  A constructor.
        /// </summary>
        /// <param name="adminId">The ID for the admin.</param>
        /// <param name="userId">The ID for the user.</param>
        /// <param name="firstName">The first name for the user.</param>
        /// <param name="lastName">The last name for the user.</param>
        /// <param name="email">The email address for the user.</param>
        /// <param name="isEmailConfirmed">Set to true if the email address is confirmed.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public AdminViewModel(int adminId, string userId, string firstName, string lastName, string email, bool isEmailConfirmed)
            : base(userId, firstName, lastName, email, isEmailConfirmed)
        {
            AdminId = adminId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The ID for the Admin.
        /// </summary>
        [DisplayName("Admin ID")]
        public int AdminId { get; }

        #endregion
    }
}
