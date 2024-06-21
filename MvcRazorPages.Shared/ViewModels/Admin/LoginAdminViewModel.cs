using MvcRazorPages.Shared.ViewModels.User;
using System.ComponentModel;

namespace MvcRazorPages.Shared.ViewModels.Admin
{
    /// <summary>
    /// A view model class to handle login input data for an admin. 
    /// </summary>
    public class LoginAdminViewModel : UserLoginViewModel
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public LoginAdminViewModel() : base()
        {

        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="email">The email for the admin.</param>
        /// <param name="password">The password for the admin.</param>
        public LoginAdminViewModel(string email, string password) : base(email, password)
        {

        }

        #endregion
    }
}
