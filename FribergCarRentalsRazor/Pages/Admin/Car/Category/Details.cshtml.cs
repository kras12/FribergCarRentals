using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FribergCarRentals.DataAccess.Repositories;
using FribergCarRentals.Helpers;
using FribergCarRentals.Sessions;
using FribergCarRentalsRazor.Helpers;
using FribergCarRentals.Models.CarCategory;
using MvcRazorPages.Shared.Data;

namespace FribergCarRentals.Pages.Admin.CarCategories
{
    /// <summary>
    /// Page model for showing details for a car category in the admin back office. 
    /// </summary>
    public class DetailsModel : PageModel
    {
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
        public DetailsModel(ICarCategoryRepository carCategoryRepository)
        {
            _carCategoryRepository = carCategoryRepository;
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
                    CarCategoryViewModel = new CarCategoryViewModel(category);

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

        #region OtherMethods

        /// <summary>
        /// Redirects to the login page and request a redirect back afterwards. 
        /// </summary>
        /// <param name="id">The ID of the car category to view.</param>
        /// <returns>A <see cref="IActionResult"/>.</returns>
        private IActionResult RedirectToLogin(int id)
        {
            TempDataHelper.Set(TempData, LoginModel.RedirectToPageTempDataKey, new RedirectToPageData(
                    "Car/Category/Details",
                    new RouteValueDictionary(new { id = id })));

            return RedirectToPage("../Login");
        }

        #endregion
    }
}
