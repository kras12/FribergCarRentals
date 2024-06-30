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
        public AdminViewModel() : base("", "", "", "")
        {

        }

        /// <summary>
        ///  A constructor.
        /// </summary>
        /// <param name="admin">The admin to copy data from.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public AdminViewModel(AdminEntity admin) :
            base(admin.User.Id, admin.User.FirstName, admin.User.LastName, admin.User.Email!)
        {
            AdminId = admin.AdminId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The ID for the Admin.
        /// </summary>
        [DisplayName("Admin ID")]
        [BindNever]
        public int AdminId { get; }

        #endregion
    }
}
