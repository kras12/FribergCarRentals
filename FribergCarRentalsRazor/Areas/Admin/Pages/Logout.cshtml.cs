using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.Areas.Admin.Pages
{
    /// <summary>
    /// The page model for logging out an admin.
    /// </summary>
    public class LogoutModel : AdminPageModelBase
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
