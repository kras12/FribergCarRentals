using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FribergCarRentals.DataAccess.Repositories;
using MvcRazorPages.Shared.Data;
using MvcRazorPages.Shared.Helpers;
using FribergCarRentals.Sessions;
using FribergCarRentals.Models.Admin;

namespace FribergCarRentals.Pages.Admin
{
    /// <summary>
    /// Page model for the main page in the admin back office.
    /// </summary>
    public class IndexModel : PageModel
    {
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
        public IndexModel(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The view model for the page. 
        /// </summary>
        public AdminViewModel AdminViewModel { get; set; } = new();

        #endregion

        #region HandlerMethods    

        /// <summary>
        /// The handler for GET requests.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync()
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin();                
            }

            var userData = UserSessionHandler.GetUserData(HttpContext.Session);
            var admin = await _adminRepository.GetByIdAsync(userData.UserId);

            if (admin is not null)
            {
                AdminViewModel = new AdminViewModel(admin);
                return Page();
            }

            throw new Exception("Failed to find the admin in the database.");
        }

        #endregion

        #region OtherMethods

        /// <summary>
        /// Redirects to the login page and request a redirect back afterwards. 
        /// </summary>
        /// <returns><see cref="IActionResult"/>.</returns>
        private IActionResult RedirectToLogin()
        {
            TempDataHelper.Set(TempData, LoginModel.RedirectToPageTempDataKey, new RedirectToPageData(
                    "/Admin/Index"));

            return RedirectToPage("/Admin/Login");
        }

        #endregion
    }
}
