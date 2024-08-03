using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.Pages.Customer
{
    /// <summary>
    /// Page model for logging out a customer. 
    /// </summary>
    public class LogoutModel : CustomerPageModelBase
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="signInManager">The injected signin manager.</param>
        public LogoutModel(IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager) : base(authorizationService, signInManager)
        {

        }

        #endregion

        #region HandlerMethods

        /// <summary>
        /// Handler for Get requests.
        /// </summary>
        /// <returns><see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnGet()
        {
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Index");
        }

        #endregion
    }
}
