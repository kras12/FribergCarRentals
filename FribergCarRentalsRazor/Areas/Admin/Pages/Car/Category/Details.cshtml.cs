using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.Helpers;
using MvcRazorPages.Shared.Data;
using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FribergCarRentals.Shared.Models.ViewModels.CarCategory;
using AutoMapper;

namespace FribergCarRentals.Areas.Admin.Pages.CarCategories
{
    /// <summary>
    /// Page model for showing details for a car category in the admin back office. 
    /// </summary>
    public class DetailsModel : AdminPageModelBase
    {
        #region Constants

        /// <summary>
        /// The page URL relative to the login page
        /// </summary>
        public const string PageUrlRelativeToLoginPage = "Car/Category/Details";

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
		public DetailsModel(ICarCategoryRepository carCategoryRepository, IAuthorizationService authorizationService,
			SignInManager<ApplicationUser> signInManager, IMapper mapper) : base(authorizationService, signInManager)
		{
			_carCategoryRepository = carCategoryRepository;
			_mapper = mapper;
		}

		#endregion

		#region Properties

		/// <summary>
		/// The view model used for showing car details. 
		/// </summary>
		public CarCategoryViewModel CarCategoryViewModel { get; set; } = null!;

        #endregion

        #region HandlerMethods

        /// <summary>
        /// Handler for GET requests. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A <see cref="Task{TResult}"/> containing <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(new RedirectToPageData(PageUrlRelativeToLoginPage, new RouteValueDictionary(new { id = id }), area: Area));
            }

            if (id < 0)
            {
                throw new Exception($"Invalid ID: {id}");
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var category = await _carCategoryRepository.GetByIdAsync(id);

                if (category is not null)
                {
                    CarCategoryViewModel = _mapper.Map<CarCategoryViewModel>(category);

                    if (TempDataHelper.TryGet(TempData, CreateModel.CreatedCarCategoryIdTempDataKey, out int categoryId))
                    {
                        CarCategoryViewModel.Messages.Add(UserMesssageHelper.CreateCarCategoryCreationSuccessMessage(categoryId));
                    }

                    return Page();
                }
            }

            throw new Exception($"Failed to show the car category with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        #endregion
    }
}
