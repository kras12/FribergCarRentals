using FribergCarRentals.Controllers.Customer;
using FribergCarRentals.Data;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Data.Repositories;
using FribergCarRentals.Helpers;
using FribergCarRentals.Models;
using FribergCarRentals.Models.Admin;
using FribergCarRentals.Models.Customer;
using FribergCarRentals.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.Controllers.Admin
{
    public class AdminController : ViewControllerBase
    {
        #region Constants

        public const string RedirectToActionTempDataKey = "AdminLoginRedirectToAction";

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
        public ActionResult Index()
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                TempDataHelper.Set(TempData, RedirectToActionTempDataKey, new LoginRedirectActionWithId(
                    nameof(Index),
                    ControllerHelper.GetControllerName<AdminController>()));

                return RedirectToAction(nameof(Login));
            }

            return View();
        }

        // GET: AdminController
        public ActionResult Login()
        {
            if (UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToAction(nameof(Index));
            }

            return View(new AdminLoginViewModel());
        }

        // Post: AdminController
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(CustomerLoginViewModel customerModel)
        {
            if (ModelState.Count > 0 && ModelState.IsValid && !UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                var admin = await _adminRepository.GetMatchingAdmin(customerModel.Email, customerModel.Password);

                if (admin is not null)
                {
                    LoginAdmin(admin);
                    return TempDataOrHomeRedirect();
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: AdminController
        public ActionResult Logout()
        {
            if (UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                UserSessionHandler.RemoveUserData(HttpContext.Session);
            }

            return RedirectToAction(nameof(Index), ControllerHelper.GetControllerName<HomeController>());
        }

        #endregion

        #region Methods       

        [NonAction]
        private void LoginAdmin(AdminEntity admin)
        {
            UserSessionHandler.SetUserData(HttpContext.Session,
                    new UserSessionData(admin.UserId, admin.Email, admin.UserRole));
        }

        [NonAction]
        private ActionResult TempDataOrHomeRedirect()
        {
            if (TempDataHelper.TryGet<LoginRedirectActionWithId>(TempData, RedirectToActionTempDataKey, out var data))
            {
                return RedirectToAction(data.Action, data.Controller, data.RouteValues);
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
