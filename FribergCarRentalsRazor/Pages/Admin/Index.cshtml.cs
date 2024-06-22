using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.Data;
using MvcRazorPages.Shared.Helpers;
using MvcRazorPages.Shared.ViewModels.Admin;
using FribergFastigheter.Server.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FribergFastigheter.Shared.Constants;

namespace FribergCarRentals.Pages.Admin
{
    /// <summary>
    /// Page model for the main page in the admin back office.
    /// </summary>
    public class IndexModel : PageModelBase
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
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="signInManager">The injected signin manager.</param>
        public IndexModel(IAdminRepository adminRepository, IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager) : base(authorizationService, signInManager)
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
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin();                
            }

            var adminId = int.Parse(User.FindFirst(x => x.Type == ApplicationUserClaims.AdminId)!.Value);
            var admin = await _adminRepository.GetByIdAsync(adminId);

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
