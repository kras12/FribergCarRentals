using MvcRazorPages.Shared.Data;
using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.DataAccess.Repositories;
using MvcRazorPages.Shared.Helpers;
using FribergCarRentals.Models.Admin;
using MvcRazorPages.Shared.Sessions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FribergCarRentals.Pages.Admin
{
    /// <summary>
    /// Page model for admin login.
    /// </summary>
    public class LoginModel : PageModel
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
        public LoginModel(IAdminRepository adminRepository)
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
        public IActionResult OnGet()
        {
            if (UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                UserSessionHandler.RemoveUserData(HttpContext.Session);
            }

            if (UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
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
            if (UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return TempDataOrHomeRedirect();
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {

                var admin = await _adminRepository.GetMatchingAdminAsync(AdminLoginViewModel.Email, AdminLoginViewModel.Password);

                if (admin is null)
                {
                    ModelState.AddModelError("", "No account matched the entered email/password.");
                    return Page();
                }
                else
                {
                    LoginAdmin(admin);
                    return TempDataOrHomeRedirect();
                }
            }

            return Page();
        }

        #endregion

        #region OtherMethods

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
        private IActionResult TempDataOrHomeRedirect()
        {
            if (TempDataHelper.TryGet<RedirectToPageData>(TempData, RedirectToPageTempDataKey, out var data))
            {
                return RedirectToPage(data.Page, data.RouteValues);
            }

            return RedirectToPage("/Admin/Index");
        }

        #endregion
    }
}
