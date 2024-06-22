using FribergFastigheter.Server.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FribergCarRentals.Pages.Admin
{
    /// <summary>
    /// The page model for logging out an admin.
    /// </summary>
    public class LogoutModel : PageModelBase
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

        #region Methods

        /// <summary>
        /// Handler for GET requests.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Index");
        }

        #endregion
    }
}
