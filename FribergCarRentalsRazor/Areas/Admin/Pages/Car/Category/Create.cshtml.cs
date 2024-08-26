using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Data.Repositories;
using FribergCarRentals.Shared.Mvc.Helpers;
using FribergCarRentals.Shared.Mvc.Data;
using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using FribergCarRentals.Shared.Models.ViewModels.CarCategory;

namespace FribergCarRentals.Areas.Admin.Pages.CarCategories
{
    /// <summary>
    /// A page model for creating car categories in the admin back office.
    /// </summary>
    public class CreateModel : AdminPageModelBase
    {
        #region Constants

        /// <summary>
        /// The key for the ID of the car category that was created.
        /// </summary>
        public const string CreatedCarCategoryIdTempDataKey = "AdminCreatedCarCategoryId";

        /// <summary>
        /// The page URL relative to the login page
        /// </summary>
        public const string PageUrlRelativeToLoginPage = "Car/Category/Create";

        #endregion

        #region Fields

        /// <summary>
        /// The injected car category repository.
        /// </summary>
        private readonly ICarCategoryRepository _carCategoryRepository;

        // The injected Auto Mapper.
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor. 
        /// </summary>
        /// <param name="carCategoryRepository">The injected car category repository.</param>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="signInManager">The injected signin manager.</param>
        /// <param name="mapper">The injected Auto Mapper.</param>
        public CreateModel(ICarCategoryRepository carCategoryRepository, IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager, IMapper mapper) : base(authorizationService, signInManager)
        {
            _carCategoryRepository = carCategoryRepository;
            _mapper = mapper;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The view model used for car creation.
        /// </summary>
        [BindProperty]
        public CreateCarCategoryViewModel CreateCarCategoryViewModel { get; set; } = new CreateCarCategoryViewModel();

        #endregion

        #region HandlerMethods

        /// <summary>
        /// Handler for GET requests.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync()
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(new RedirectToPageData(PageUrlRelativeToLoginPage, area: Area));
            }

            return Page();
        }

        /// <summary>
        /// Handler for POST requests.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(new RedirectToPageData(PageUrlRelativeToLoginPage, area: Area));
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var category = _mapper.Map<CarCategoryEntity>(CreateCarCategoryViewModel);
                await _carCategoryRepository.AddAsync(category);
                TempDataHelper.Set(TempData, CreatedCarCategoryIdTempDataKey, category.CarCategoryId);
                return RedirectToPage("Details", new { id = category.CarCategoryId });
            }

            return Page();
        }

        #endregion
    }
}
