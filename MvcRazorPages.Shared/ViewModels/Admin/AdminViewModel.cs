using FribergCarRentals.Data.EntityClasses;
using MvcRazorPages.Shared.ViewModels.User;

namespace MvcRazorPages.Shared.ViewModels.Admin
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
        public AdminViewModel() : base(0, "", "", "")
        {

        }

        /// <summary>
        ///  A constructor.
        /// </summary>
        /// <param name="admin">The admin to copy data from.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public AdminViewModel(AdminEntity admin) :
            base(admin.AdminId, admin.User.FirstName, admin.User.LastName, admin.User.Email!)
        {

        }

        #endregion
    }
}
