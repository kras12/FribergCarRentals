using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FribergCarRentals.Sessions;

namespace FribergCarRentals.Pages.Customer
{
    /// <summary>
    /// Page model for logging out a customer. 
    /// </summary>
    public class LogoutModel : PageModel
    {
        #region HandlerMethods

        /// <summary>
        /// Handler for Get requests.
        /// </summary>
        /// <returns><see cref="IActionResult"/>.</returns>
        public IActionResult OnGet()
        {
            if (UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                UserSessionHandler.RemoveUserData(HttpContext.Session);
            }

            return RedirectToPage("/Index");
        }

        #endregion
    }
}
