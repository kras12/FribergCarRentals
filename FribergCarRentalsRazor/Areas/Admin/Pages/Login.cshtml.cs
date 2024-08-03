using MvcRazorPages.Shared.Data;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.Helpers;
using Microsoft.AspNetCore.Mvc;
using MvcRazorPages.Shared.ViewModels.Admin;
using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace FribergCarRentals.Areas.Admin.Pages
{
    /// <summary>
    /// Page model for admin login.
    /// </summary>
    public class LoginModel : AdminPageModelBase
    {
        #region Constants

        /// <summary>
        /// The key for the redirection data for the page to redirect to after logins. 
        /// </summary>
        public const string RedirectToPageTempDataKey = "AdminLoginRedirectToPage";

        #endregion

        #region Fields

        /// <summary>
        /// The injected admin repository.
        /// </summary>
        private readonly IAdminRepository _adminRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="adminRepository">Injected admin repository.</param>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="signInManager">The injected signin manager.</param>
        public LoginModel(IAdminRepository adminRepository, IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager) : base(authorizationService, signInManager)
        {
            _adminRepository = adminRepository;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The view model for admin login forms.
        /// </summary>
        [BindProperty]
        public LoginAdminViewModel AdminLoginViewModel { get; set; } = default!;

        #endregion

        #region HandlerMethods

        /// <summary>
        /// The handler for Get requests.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGet()
        {
            if (await IsCustomerLoggedIn())
            {
                await _signInManager.SignOutAsync();
            }
            else if (await IsAdminLoggedIn())
            {
                return TempDataOrHomeRedirect();
            }

            AdminLoginViewModel = new LoginAdminViewModel();
            return Page();
        }       

        /// <summary>
        /// Handler for POST requests.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (await IsAdminLoggedIn())
            {
                return TempDataOrHomeRedirect();
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(AdminLoginViewModel.Email, AdminLoginViewModel.Password, isPersistent: true, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return TempDataOrHomeRedirect();
                }
                else
                {
                    // The key needs to be the name of the view model (insted of empty string) because the error is shown in a partial view. 
                    ModelState.AddModelError(nameof(AdminLoginViewModel), "No account matched the entered email/password.");
                }
            }

            return Page();
        }

        #endregion

        #region OtherMethods

        /// <summary>
        /// Redirects the admin to the page stored in the temp storage if such data exists, else redirects the admin to the homepage. 
        /// </summary>
        /// <returns><see cref="IActionResult"/>.</returns>
        [NonAction]
        private IActionResult TempDataOrHomeRedirect()
        {
            if (TempDataHelper.TryGet<RedirectToPageData>(TempData, RedirectToPageTempDataKey, out var data))
            {
                return RedirectToPage(data.Page, data.RouteValues);
            }

            return RedirectToPageInArea(IndexModel.PageUrlRelativeToLoginPage);
        }

        #endregion
    }
}
