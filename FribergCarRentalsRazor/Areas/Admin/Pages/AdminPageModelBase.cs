using FribergCarRentals.Pages;
using FribergFastigheter.Server.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcRazorPages.Shared.Data;
using MvcRazorPages.Shared.Helpers;

namespace FribergCarRentals.Areas.Admin.Pages
{
    /// <summary>
    /// Base class for admin pages. 
    /// </summary>
    public abstract class AdminPageModelBase : PageModelBase
    {
        #region Constants

        /// <summary>
        /// The area for the controller.
        /// </summary>
        public const string Area = "Admin";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="authorizationService">Injected authorization service.</param>
        /// <param name="signInManager">Injected signin manager.</param>
        protected AdminPageModelBase(IAuthorizationService authorizationService, SignInManager<ApplicationUser> signInManager) : base(authorizationService, signInManager)
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Redirects the user to an action within the admin area.
        /// </summary>
        /// <param name="pageName">The page to redirect to.</param>
        /// <param name="controller">The controller that handles the action.</param>
        /// <param name="routeValues">Optional route values to send with the request.</param>
        /// <returns><see cref="RedirectToPageResult"/>.</returns>
        protected RedirectToPageResult RedirectToPageInArea(string pageName, RouteValueDictionary? routeValues = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary(new { Area = Area });

            if (routeValues != null)
            {
                routeValues.ToList().ForEach(x => routeValueDictionary.TryAdd(x.Key, x.Value));
            }

            return base.RedirectToPage(pageName, routeValueDictionary);
        }

        /// <summary>
        /// Redirects to the login page and request a redirect back afterwards. 
        /// </summary>
        /// <remarks>Area is handled by this controller class.</remarks>
        /// <param name="redirectBackToPageData">Contains data for the page to redirect back to after login.</param>
        /// <returns><see cref="IActionResult"/>.</returns>
        protected IActionResult RedirectToLogin(RedirectToPageData redirectBackToPageData)
        {
            TempDataHelper.Set(TempData, LoginRedirectToPageTempDataKey, redirectBackToPageData);
            return RedirectToPageInArea("login");
        }

        #endregion
    }
}
