using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MvcRazorPages.Shared.Helpers;
using FribergCarRentals.Sessions;

namespace FribergCarRentals.Pages.Admin
{
    /// <summary>
    /// The page model for logging out an admin.
    /// </summary>
    public class LogoutModel : PageModel
    {

        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public LogoutModel()
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Handler for GET requests.
        /// </summary>
        /// <returns></returns>
        public IActionResult OnGetAsync()
        {
            if (UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                UserSessionHandler.RemoveUserData(HttpContext.Session);
            }

            return RedirectToPage("/Index");
        }

        #endregion
    }
}
