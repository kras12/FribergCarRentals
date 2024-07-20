using MvcRazorPages.Shared.Helpers;
using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.Data;
using FribergCarRentals.Helpers;
using MvcRazorPages.Shared.ViewModels.Admin;
using FribergFastigheter.Server.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FribergFastigheter.Shared.Constants;

namespace FribergCarRentals.Areas.Admin.Controllers
{
    /// <summary>
    /// Main controller for the admin back office. 
    /// </summary>
    [Area(Area)]
    [Route($"[area]")]
    public class AdminHomeController : AdminControllerBase
    {
        #region Constants

        /// <summary>
        /// The route part for the controller.
        /// </summary>
        private const string CurrentControllerRoutePart = "Home";

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
        public AdminHomeController(IAdminRepository _adminRepository, IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager) : base(authorizationService, signInManager)
        {
            this._adminRepository = _adminRepository;
        }

        #endregion

        #region Actions

        /// <summary>
        /// Serves the admin home page.
        /// </summary>
        /// <returns>A <see cref="Task"/> containing a <see cref="IActionResult"/>.</returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        [Route($"/{Area}")]
        public async Task<IActionResult> Index()
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(new RedirectToActionData(nameof(Index), ControllerHelper.GetControllerName<AdminHomeController>(), area: Area));
            }

            var userId = User.FindFirst(x => x.Type == ApplicationUserClaims.UserId)!.Value;
            var admin = await _adminRepository.GetByUserIdAsync(userId);

            if (admin is not null)
            {
                AdminViewModel viewModel = new AdminViewModel(admin);
                return View(viewModel);
            }

            throw new Exception("Failed to find the admin in the database.");
        }

        /// <summary>
        /// Serves the admin login page.
        /// </summary>
        /// <returns>A <see cref="Task"/> containing a <see cref="IActionResult"/>.</returns>
        [HttpGet]
        [Route($"{nameof(Login)}")]
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

        /// <summary>
        /// Handles the admin login process.
        /// </summary>
        /// <param name="loginAdminViewModel"></param>
        /// <returns>A <see cref="Task"/> containing a <see cref="IActionResult"/>.</returns>
        [HttpPost]
        [Route($"{nameof(Login)}")]
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

        [Route($"{nameof(Logout)}")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToActionInArea(nameof(FribergCarRentals.Controllers.HomeController.Index), ControllerHelper.GetControllerName<FribergCarRentals.Controllers.HomeController>());
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
            if (TempDataHelper.TryGet<RedirectToActionData>(TempData, LoginRedirectToPageTempDataKey, out var data))
            {
                return RedirectToAction(data.Action, data.Controller, data.RouteValues);
            }

            return RedirectToActionInArea(nameof(Index));
        }

        #endregion
    }
}
