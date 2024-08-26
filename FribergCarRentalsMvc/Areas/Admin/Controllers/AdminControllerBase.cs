using FribergCarRentals.Controllers;
using FribergCarRentals.Data.Entities;
using FribergCarRentals.Shared.Mvc.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Shared.Mvc.Data;
using FribergCarRentals.Shared.Mvc.Helpers;

namespace FribergCarRentals.Areas.Admin.Controllers
{
    /// <summary>
    /// Base class for admin controllers. 
    /// </summary>
    [Area(Area)]
    public abstract class AdminControllerBase : ViewControllerBase
    {
        #region Constants

        /// <summary>
        /// The area for the controller.
        /// </summary>
        public const string Area = "Admin";

        /// <summary>
        /// The key for the redirection data for the page to redirect to after logins. 
        /// </summary>
        public const string LoginRedirectToPageTempDataKey = "AdminLoginRedirectToPage";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="signInManager">The injected signin manager.</param>
        protected AdminControllerBase(IAuthorizationService authorizationService, SignInManager<ApplicationUser> signInManager) : base(authorizationService, signInManager)
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Redirects the user to an action within the admin area.
        /// </summary>
        /// <param name="action">The action to redirect to.</param>
        /// <param name="routeValues">Optional route values to send with the request.</param>
        /// <returns><see cref="RedirectToActionResult"/>.</returns>
        protected RedirectToActionResult RedirectToActionInArea(string action, RouteValueDictionary? routeValues = null)
        {
            return RedirectToActionInArea(action, ControllerContext.ActionDescriptor.ControllerName, routeValues);            
        }

        /// <summary>
        /// Redirects the user to an action within the admin area.
        /// </summary>
        /// <param name="action">The action to redirect to.</param>
        /// <param name="controller">The controller that handles the action.</param>
        /// <param name="routeValues">Optional route values to send with the request.</param>
        /// <returns><see cref="RedirectToActionResult"/>.</returns>
        protected RedirectToActionResult RedirectToActionInArea(string action, string controller, RouteValueDictionary? routeValues = null)
        {
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary(new { Area = Area });

            if (routeValues != null)
            {
                routeValues.ToList().ForEach(x => routeValueDictionary.TryAdd(x.Key, x.Value));
            }

            return base.RedirectToAction(action, controller, routeValueDictionary);
        }

        /// <summary>
        /// Redirects to the login page and request a redirect back afterwards. 
        /// </summary>
        /// <remarks>Area is handled by this controller class.</remarks>
        /// <param name="redirectBackToActionData">Contains data for the action to redirect back to after login.</param>
        /// <returns><see cref="IActionResult"/>.</returns>
        protected IActionResult RedirectToLogin(RedirectToActionData redirectBackToActionData)
        {
            TempDataHelper.Set(TempData, LoginRedirectToPageTempDataKey, redirectBackToActionData);
            return RedirectToActionInArea(nameof(AdminHomeController.Login), ControllerHelper.GetControllerName<AdminHomeController>());
        }

        #endregion
    }
}
