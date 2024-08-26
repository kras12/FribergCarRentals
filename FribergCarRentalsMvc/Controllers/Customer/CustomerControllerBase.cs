using FribergCarRentals.Shared.Mvc.Helpers;
using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Shared.Mvc.Data;
using FribergCarRentals.Shared.Mvc.Helpers;

namespace FribergCarRentals.Controllers.Customer
{
    /// <summary>
    /// Base class for customer controllers. 
    /// </summary>
    public abstract class CustomerControllerBase : ViewControllerBase
    {
        #region Constants

        /// <summary>
        /// The key for the redirection data for the page to redirect to after logins. 
        /// </summary>
        public const string LoginRedirectToPageTempDataKey = "CustomerLoginRedirectToPage";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="signInManager">The injected signin manager.</param>
        protected CustomerControllerBase(IAuthorizationService authorizationService, SignInManager<ApplicationUser> signInManager) : base(authorizationService, signInManager)
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Redirects to the login page and request a redirect back afterwards. 
        /// </summary>
        /// <remarks>Area is handled by this controller class.</remarks>
        /// <param name="redirectBackToActionData">Contains data for the action to redirect back to after login.</param>
        /// <returns><see cref="IActionResult"/>.</returns>
        protected IActionResult RedirectToLogin(RedirectToActionData redirectBackToActionData)
        {
            TempDataHelper.Set(TempData, LoginRedirectToPageTempDataKey, redirectBackToActionData);
            return RedirectToAction(nameof(CustomerController.Authenticate), ControllerHelper.GetControllerName<CustomerController>());
        }

        #endregion
    }
}
