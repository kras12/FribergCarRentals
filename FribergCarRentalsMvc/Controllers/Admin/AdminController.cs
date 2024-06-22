using MvcRazorPages.Shared.Helpers;
using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.Data;
using FribergCarRentals.Helpers;
using MvcRazorPages.Shared.ViewModels.Admin;
using FribergFastigheter.Server.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FribergFastigheter.Shared.Constants;
using MvcRazorPages.Shared.ViewModels.Customer;

namespace FribergCarRentals.Controllers.Admin
{
    public class AdminController : ViewControllerBase
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
        /// Constructor.
        /// </summary>
        /// <param name="_adminRepository">The injected admin repository.</param>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="signInManager">The injected signin manager.</param>
        public AdminController(IAdminRepository _adminRepository, IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager) : base(authorizationService, signInManager)
        {
            this._adminRepository = _adminRepository;
        }

        #endregion

        #region Actions

        // GET: AdminController
        public async Task<IActionResult> Index()
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(nameof(Index));
            }

            var adminId = int.Parse(User.FindFirst(x => x.Type == ApplicationUserClaims.AdminId)!.Value);
            var admin = await _adminRepository.GetByIdAsync(adminId);

            if (admin is not null)
            {
                AdminViewModel viewModel = new AdminViewModel(admin);
                return View(viewModel);
            }

            throw new Exception("Failed to find the admin in the database.");
        }

        // GET: AdminController
        public async Task<IActionResult> Login()
        {
            if (await IsCustomerLoggedIn())
            {
                await _signInManager.SignOutAsync();
            }
            else if (await IsAdminLoggedIn())
            {
                return TempDataOrHomeRedirect();
            }

            LoginAdminViewModel viewModel = new();
            return View(viewModel);
        }

        // Post: AdminController
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginAdminViewModel loginAdminViewModel)
        {
            if (await IsAdminLoggedIn())
            {
                return TempDataOrHomeRedirect();
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginAdminViewModel.Email, loginAdminViewModel.Password, isPersistent: true, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return TempDataOrHomeRedirect();
                }
                else
                {
                    // The key needs to be the name of the view model (insted of empty string) because the error is shown in a partial view. 
                    ModelState.AddModelError(nameof(LoginAdminViewModel), "No account matched the entered email/password.");
                }
            }

            return View(loginAdminViewModel);
        }

        // GET: AdminController
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), ControllerHelper.GetControllerName<HomeController>());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Redirects the admin to the page stored in the temp storage if such data exists, else redirects the admin to the homepage. 
        /// </summary>
        /// <returns><see cref="IActionResult"/>.</returns>
        [NonAction]
        private ActionResult TempDataOrHomeRedirect()
        {
            if (TempDataHelper.TryGet<RedirectToActionData>(TempData, RedirectToPageTempDataKey, out var data))
            {
                return RedirectToAction(data.Action, data.Controller, data.RouteValues);
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region OtherMethods

        /// <summary>
        /// Redirects to the login page and request a redirect back afterwards. 
        /// </summary>
        /// <param name="action">The action to redirect to.</param>
        /// <returns><see cref="IActionResult"/>.</returns>
        private IActionResult RedirectToLogin(string action)
        {
            TempDataHelper.Set(TempData, RedirectToPageTempDataKey, new RedirectToActionData(
                    action, ControllerHelper.GetControllerName<AdminController>()));

            return RedirectToAction(nameof(Login), ControllerHelper.GetControllerName<AdminController>());
        }

        #endregion
    }
}
