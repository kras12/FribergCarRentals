using FribergCarRentals.Data.EntityClasses;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MvcRazorPages.Shared.ViewModels.User;
using System.ComponentModel;

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
        public AdminViewModel() : base("0", "", "", "", isEmailConfirmed: false)
        {

        }

        /// <summary>
        ///  A constructor.
        /// </summary>
        /// <param name="admin">The admin to copy data from.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public AdminViewModel(AdminEntity admin) :
            base(admin.User.Id, admin.User.FirstName, admin.User.LastName, admin.User.Email!, admin.User.EmailConfirmed)
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
