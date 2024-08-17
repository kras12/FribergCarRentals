using FribergCarRentals.Shared.Models.ViewModels.User;
using System.ComponentModel;

namespace FribergCarRentals.Shared.Models.ViewModels.Admin
{
    /// <summary>
    /// A view model class to handle data for an admin.
    /// </summary>
    public class AdminViewModel : UserViewModelBase
    {
        #region Properties

        /// <summary>
        /// The ID for the Admin.
        /// </summary>
        [DisplayName("Admin ID")]
        public int AdminId { get; }

        #endregion
    }
}
