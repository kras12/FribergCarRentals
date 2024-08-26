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
using FribergCarRentals.Shared.Models.ViewModels.Message;

namespace FribergCarRentals.Areas.Admin.Pages.CarCategories
{
    /// <summary>
    /// Page model class for editing a car category in the admin back office. 
    /// </summary>
    public class EditModel : AdminPageModelBase
    {
        #region Constants

        /// <summary>
        /// The key to use when storing page sub titles in temporary storage.
        /// </summary>
        private const string PageSubTitleTempDataKey = "AdminCarCategoryEditPageSubTitle";

        /// <summary>
        /// The page URL relative to the login page
        /// </summary>
        public const string PageUrlRelativeToLoginPage = "Car/Category/Edit";

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
        public EditModel(ICarCategoryRepository carCategoryRepository, IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager, IMapper mapper) : base(authorizationService, signInManager)
        {
            _carCategoryRepository = carCategoryRepository;
            _mapper = mapper;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The view model used for editing a car category. 
        /// </summary>
        [BindProperty]
        public EditCarCategoryViewModel EditCarCategoryViewModel { get; set; } = new();

        #endregion

        #region HandlerMethods        

        /// <summary>
        /// Handler for GET requests. 
        /// </summary>
        /// <param name="id">The ID of the car category to edit.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing an <see cref="IActionResult"/>.</returns>
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
                    EditCarCategoryViewModel = _mapper.Map<EditCarCategoryViewModel>(category);
                    TempDataHelper.Set(TempData, PageSubTitleTempDataKey, EditCarCategoryViewModel.PageSubTitle!);
                    return Page();
                }
            }

            throw new Exception($"Failed to show the car with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        /// <summary>
        /// Handler for POST requests. 
        /// </summary>
        /// <param name="id">The ID of the car to edit.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing an <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(new RedirectToPageData(PageUrlRelativeToLoginPage, new RouteValueDictionary(new { id = id }), area: Area));
            }

            if (id <= 0 || id != EditCarCategoryViewModel.CarCategoryId)
            {
                throw new Exception($"Invalid ID or ID mismatch - QueryParameter: {id} - ViewModel: {EditCarCategoryViewModel.CarCategoryId}");
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var category = _mapper.Map<CarCategoryEntity>(EditCarCategoryViewModel);
                await _carCategoryRepository.UpdateAsync(category);
                EditCarCategoryViewModel = _mapper.Map<EditCarCategoryViewModel>(category);
                EditCarCategoryViewModel.Messages.Add(MessageViewModelHelper.CreateCarCategoryUpdateSuccessMessage(category.CarCategoryId));
                return Page();
            }

            if (TempDataHelper.TryGet(TempData, PageSubTitleTempDataKey, out string? pageSubTitle))
            {
				EditCarCategoryViewModel.SetPageSubTitle(pageSubTitle);
                TempDataHelper.Set(TempData, PageSubTitleTempDataKey, EditCarCategoryViewModel.PageSubTitle!); // The user can fail again.
            }

            return Page();
        }

        #endregion
    }
}
