using FribergCarRentals.Pages;
using FribergFastigheter.Server.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcRazorPages.Shared.Data;
using MvcRazorPages.Shared.Helpers;

namespace FribergCarRentals.Pages
{
    /// <summary>
    /// Base class for customer pages. 
    /// </summary>
    public abstract class CustomerPageModelBase : PageModelBase
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="authorizationService">Injected authorization service.</param>
        /// <param name="signInManager">Injected signin manager.</param>
        protected CustomerPageModelBase(IAuthorizationService authorizationService, SignInManager<ApplicationUser> signInManager) : base(authorizationService, signInManager)
        {

        }

        #endregion

        #region Methods       

        /// <summary>
        /// Redirects to the login page and request a redirect back afterwards. 
        /// </summary>
        /// <remarks>Area is handled by this controller class.</remarks>
        /// <param name="redirectBackToPageData">Contains data for the page to redirect back to after login.</param>
        /// <returns><see cref="IActionResult"/>.</returns>
        protected IActionResult RedirectToLogin(RedirectToPageData redirectBackToPageData)
        {
            TempDataHelper.Set(TempData, LoginRedirectToPageTempDataKey, redirectBackToPageData);
            return RedirectToPage("/Customer/Authenticate");
        }

        #endregion
    }
}
