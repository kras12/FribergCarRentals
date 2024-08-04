using FribergCarRentals.Data.Entities;
using FribergCarRentals.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FribergCarRentals.Pages
{
    /// <summary>
    /// Base class for page models. 
    /// </summary>
    public class PageModelBase : PageModel
    {
        #region Constants

        /// <summary>
        /// The key for the redirection data for the page to redirect to after logins. 
        /// </summary>
        public const string LoginRedirectToPageTempDataKey = "LoginRedirectToPage";

        #endregion

        #region Fields

        // The injected authorization service.
        protected readonly IAuthorizationService _authorizationService;

        // The injected signin manager.
        protected readonly SignInManager<ApplicationUser> _signInManager;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="signInManager">The injected signin manager.</param>
        public PageModelBase(IAuthorizationService authorizationService, SignInManager<ApplicationUser> signInManager)
        {
            _authorizationService = authorizationService;
            _signInManager = signInManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks whether the current user is a logged in admin. 
        /// </summary>
        /// <returns>True if the user is a logged in admin.</returns>
        protected async Task<bool> IsAdminLoggedIn()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var authorizationResult = await _authorizationService.AuthorizeAsync(User, ApplicationUserPolicies.Admin);

                if (authorizationResult.Succeeded)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks whether the current user is a logged in customer. 
        /// </summary>
        /// <returns>True if the user is a logged in customer.</returns>
        protected async Task<bool> IsCustomerLoggedIn()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var authorizationResult = await _authorizationService.AuthorizeAsync(User, ApplicationUserPolicies.Customer);

                if (authorizationResult.Succeeded)
                {
                    return true;
                }
            }

            return false;
        }        

        #endregion
    }
}
