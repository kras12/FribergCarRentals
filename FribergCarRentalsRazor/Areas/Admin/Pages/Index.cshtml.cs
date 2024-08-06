using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.Data;
using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FribergFastigheter.Shared.Constants;
using FribergCarRentals.Shared.Models.ViewModels.Admin;
using AutoMapper;

namespace FribergCarRentals.Areas.Admin.Pages
{
    /// <summary>
    /// Page model for the main page in the admin back office.
    /// </summary>
    public class IndexModel : AdminPageModelBase
    {
        #region Constants

        /// <summary>
        /// The page URL relative to the login page
        /// </summary>
        public const string PageUrlRelativeToLoginPage = "Index";

        #endregion

        #region Fields

        /// <summary>
        /// The injected admin repository.
        /// </summary>
        private readonly IAdminRepository _adminRepository;

		// The injected Auto Mapper.
		private readonly IMapper _mapper;

		#endregion

		#region Constructors

		/// <summary>
		/// A constructor.
		/// </summary>
		/// <param name="adminRepository">Injected admin repository.</param>
		/// <param name="authorizationService">The injected authorization service.</param>
		/// <param name="signInManager">The injected signin manager.</param>
		/// <param name="mapper">The injected Auto Mapper.</param>
		public IndexModel(IAdminRepository adminRepository, IAuthorizationService authorizationService,
			SignInManager<ApplicationUser> signInManager, IMapper mapper) : base(authorizationService, signInManager)
		{
			_adminRepository = adminRepository;
			_mapper = mapper;
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
                return RedirectToLogin(new RedirectToPageData(PageUrlRelativeToLoginPage, area: Area));
            }

            var userId = User.FindFirst(x => x.Type == ApplicationUserClaims.UserId)!.Value;
            var admin = await _adminRepository.GetByUserIdAsync(userId);

            if (admin is not null)
            {
                AdminViewModel = _mapper.Map<AdminViewModel>(admin);
                return Page();
            }

            throw new Exception("Failed to find the admin in the database.");
        }

        #endregion
    }
}
