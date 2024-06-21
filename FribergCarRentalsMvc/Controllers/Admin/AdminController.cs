using MvcRazorPages.Shared.Helpers;
using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.DataAccess.Repositories;
using FribergCarRentals.Models.Admin;
using FribergCarRentals.Sessions;
using MvcRazorPages.Shared.Data;
using MvcRazorPages.Shared.Helpers;
using FribergCarRentals.Helpers;

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

        private readonly IAdminRepository _adminRepository;

        #endregion

        #region Constructors

        public AdminController(IAdminRepository _adminRepository)
        {
            this._adminRepository = _adminRepository;
        }

        #endregion

        #region Actions

        // GET: AdminController
        public async Task<IActionResult> Index()
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Index));
            }

            var userData = UserSessionHandler.GetUserData(HttpContext.Session);
            var admin = await _adminRepository.GetByIdAsync(userData.UserId);

            if (admin is not null)
            {
                AdminViewModel viewModel = new AdminViewModel(admin);
                return View(viewModel);
            }

            throw new Exception("Failed to find the admin in the database.");
        }

        // GET: AdminController
        public ActionResult Login()
        {
            if (UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                UserSessionHandler.RemoveUserData(HttpContext.Session);
            }

            if (UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
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
            if (UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return TempDataOrHomeRedirect();
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var admin = await _adminRepository.GetMatchingAdminAsync(loginAdminViewModel.Email, loginAdminViewModel.Password);

                if (admin is null)
                {
                    ModelState.AddModelError("", "No account matched the entered email/password.");
                    return View(loginAdminViewModel);
                }
                else
                {
                    LoginAdmin(admin);
                    return TempDataOrHomeRedirect();
                }
            }

            return View(loginAdminViewModel);
        }

        // GET: AdminController
        public ActionResult Logout()
        {
            if (UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                UserSessionHandler.RemoveUserData(HttpContext.Session);
            }

            return RedirectToAction(nameof(HomeController.Index), ControllerHelper.GetControllerName<HomeController>());
        }

        #endregion

        #region Methods       

        /// <summary>
        /// Saves the admin user data in the session storage. 
        /// </summary>
        /// <param name="admin">The admin to login.</param>
        [NonAction]
        private void LoginAdmin(AdminEntity admin)
        {
            UserSessionHandler.SetUserData(HttpContext.Session,
                    new UserSessionData(admin.UserId, admin.Email, admin.UserRole));
        }

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
