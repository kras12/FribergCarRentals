using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.DataAccess.Repositories;
using FribergCarRentals.Sessions;
using MvcRazorPages.Shared.Helpers;
using FribergCarRentalsBravo.Models.CarCategory;
using MvcRazorPages.Shared.Data;

namespace FribergCarRentals.Pages.Admin.CarCategories
{
    /// <summary>
    /// Page model class for editing a car category in the admin back office. 
    /// </summary>
    public class EditModel : PageModel
    {
        #region Constants

        /// <summary>
        /// The key to use when storing page sub titles in temporary storage.
        /// </summary>
        private const string PageSubTitleTempDataKey = "AdminCarCategoryEditPageSubTitle";

        #endregion

        #region Fields

        /// <summary>
        /// The injected car category repository.
        /// </summary>
        private readonly ICarCategoryRepository _carCategoryRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor. 
        /// </summary>
        /// <param name="carCategoryRepository">The injected car category repository.</param>
        public EditModel(ICarCategoryRepository carCategoryRepository)
        {
            _carCategoryRepository = carCategoryRepository;
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
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(id);
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
                    EditCarCategoryViewModel = new EditCarCategoryViewModel(category);
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
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(id);
            }

            if (id <= 0 || id != EditCarCategoryViewModel.CarCategoryId)
            {
                throw new Exception($"Invalid ID or ID mismatch - QueryParameter: {id} - ViewModel: {EditCarCategoryViewModel.CarCategoryId}");
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (DataTransferHelper.TryTransferData(EditCarCategoryViewModel, out CarCategoryEntity category))
                {
                    await _carCategoryRepository.UpdateAsync(category);
                    EditCarCategoryViewModel = new EditCarCategoryViewModel(category);
                    EditCarCategoryViewModel.Messages.Add(UserMesssageHelper.CreateCarCategoryUpdateSuccessMessage(category.CarCategoryId));
                    return Page();
                }
            }

            if (TempDataHelper.TryGet(TempData, PageSubTitleTempDataKey, out string? pageSubTitle))
            {
                EditCarCategoryViewModel.PageSubTitle = pageSubTitle;
                TempDataHelper.Set(TempData, PageSubTitleTempDataKey, EditCarCategoryViewModel.PageSubTitle!); // The user can fail again.
            }

            return Page();
        }

        #endregion

        #region OtherMethods

        /// <summary>
        /// Redirects to the login page and request a redirect back afterwards. 
        /// </summary>
        /// <param name="id">The ID of the car category to edit.</param>
        /// <returns>An <see cref="IActionResult"/>.</returns>
        private IActionResult RedirectToLogin(int id)
        {
            TempDataHelper.Set(TempData, LoginModel.RedirectToPageTempDataKey, new RedirectToPageData(
                    "Car/Category/Edit",
                    new RouteValueDictionary(new { id = id })));

            return RedirectToPage("../Login");
        }

        #endregion
    }
}
