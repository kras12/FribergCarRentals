using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using MvcRazorPages.Shared.Data;
using MvcRazorPages.Shared.Helpers;
using MvcRazorPages.Shared.ViewModels.Customer;
using System.Text;

namespace FribergCarRentals.Pages.Customer
{
    public class RegistrationConfirmationModel : CustomerPageModelBase
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
        public RegistrationConfirmationModel(UserManager<ApplicationUser> userManager, IAuthorizationService authorizationService, 
            SignInManager<ApplicationUser> signInManager) : base(authorizationService, signInManager)
        {
            _userManager = userManager;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The view model for confirming customer registrations.
        /// </summary>
        public ConfirmCustomerRegistrationViewModel ConfirmCustomerRegistrationViewModel { get; set; } = new();

        #endregion

        #region Methods        

        /// <summary>
        /// Shows the customer registration confirmation page. 
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns><see cref="IActionResult"/>.</returns>
        /// <exception cref="Exception"></exception>
        public async Task<IActionResult> OnGet(string userId)
        {
            TempDataHelper.TryRenew<RedirectToPageData>(TempData, LoginRedirectToPageTempDataKey);

            string? confirmEmailLink = null;
            bool showConfirmEmailLink = true;

            if (showConfirmEmailLink)
            {
                var user = await _userManager.FindByIdAsync(userId) ?? throw new Exception($"Failed to find user with ID: {userId}");
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                confirmEmailLink = Url.Page("ConfirmEmail", new { userId = userId, code = code });
            }

            ConfirmCustomerRegistrationViewModel = new ConfirmCustomerRegistrationViewModel(confirmEmailLink);

            return Page();
        }

        #endregion
    }
}
