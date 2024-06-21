using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FribergCarRentals.DataAccess.DatabaseContexts;
using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.Helpers;
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
