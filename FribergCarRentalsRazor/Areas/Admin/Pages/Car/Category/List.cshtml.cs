using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.Helpers;
using MvcRazorPages.Shared.Data;
using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FribergCarRentals.Shared.Models.ViewModels.CarCategory;
using FribergCarRentals.Shared.Models.ViewModels.Other;
using AutoMapper;
using FribergCarRentals.Shared.Models.ViewModels.Message;

namespace FribergCarRentals.Areas.Admin.Pages.CarCategories
{
    /// <summary>
    /// Page model class for listing car categories in the admin back office.
    /// </summary>
    public class ListModel : AdminPageModelBase
    {
        #region Constants

        /// <summary>
        /// The page URL relative to the login page
        /// </summary>
        public const string PageUrlRelativeToLoginPage = "Car/Category/List";

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
		public ListModel(ICarCategoryRepository carCategoryRepository, IAuthorizationService authorizationService,
			SignInManager<ApplicationUser> signInManager, IMapper mapper) : base(authorizationService, signInManager)
		{
			_carCategoryRepository = carCategoryRepository;
			_mapper = mapper;
		}

		#endregion

		#region Properties

		/// <summary>
		/// A view model used to present a list of car categories. 
		/// </summary>
		public ListViewModel<CarCategoryViewModel> CarCategoryListViewModel { get; private set; } = new();

        #endregion

        #region HandlerMethods       

        /// <summary>
        /// Handler for GET requests.
        /// </summary>
        /// <returns><see cref="Task{TResult}"/> containing an <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnGetAsync()
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(new RedirectToPageData(PageUrlRelativeToLoginPage, area: Area));
            }

            CarCategoryListViewModel = new(_mapper.Map<List<CarCategoryViewModel>>(await _carCategoryRepository.GetCategoryStatistics()));
            TempDataHelper.Set(TempData, DeleteModel.RedirectToPageAfterDeleteTempDataKey, 
                new RedirectToPageData("List", area: Area));

            if (TempDataHelper.TryGet(TempData, DeleteModel.DeletedCarCategoryIdTempDataKey, out int deletedCarCategoryId))
            {
                CarCategoryListViewModel.Messages.Add(MessageViewModelHelper.CreateCarCategoryDeletionSuccessMessage(deletedCarCategoryId));
            }

            return Page();
        }

        #endregion
    }
}
