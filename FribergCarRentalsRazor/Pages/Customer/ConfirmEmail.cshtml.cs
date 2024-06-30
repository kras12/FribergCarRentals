using FribergFastigheter.Server.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using MvcRazorPages.Shared.Data;
using MvcRazorPages.Shared.Helpers;
using System.Text;

namespace FribergCarRentals.Pages.Customer
{
    public class ConfirmEmailModel : PageModelBase
    {
        #region fields

        // The injected user manager.
        private readonly UserManager<ApplicationUser> _userManager;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="userManager">The injected user manager.</param>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="signInManager">The injected signin manager.</param>
        public ConfirmEmailModel(UserManager<ApplicationUser> userManager, IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager) : base(authorizationService, signInManager)
        {
            _userManager = userManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Confirms the customers email address.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="code">The code to confirm the email address.</param>
        /// <returns><see cref="IActionResult"/>.</returns>
        /// <exception cref="Exception"></exception>
        public async Task<IActionResult> OnGet(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId) ?? throw new Exception($"Failed to find user with ID: {userId}");
            string decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var confirmResult = await _userManager.ConfirmEmailAsync(user, decodedCode);

            if (confirmResult.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return TempDataOrHomeRedirect();
            }

            throw new Exception($"Failed to confirm email for user with ID: {userId}");
        }

        #endregion

        #region OtherMethods

        /// <summary>
        /// Redirects the user to the page stored in the temp storage if such data exists, else redirects the user to the homepage. 
        /// </summary>
        /// <returns><see cref="IActionResult"/>.</returns>
        [NonAction]
        private ActionResult TempDataOrHomeRedirect()
        {
            if (TempDataHelper.TryGet<RedirectToPageData>(TempData, AuthenticateModel.RedirectInstructionsTempDataKey, out var data))
            {
                return RedirectToPage(data.Page, data.RouteValues);
            }

            return RedirectToPage("/Index");
        }

        #endregion
    }
}
