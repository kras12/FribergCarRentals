using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.DataAccess.Repositories;
using MvcRazorPages.Shared.Helpers;
using FribergCarRentals.Sessions;
using FribergCarRentals.Models.CarCategory;
using MvcRazorPages.Shared.Data;

namespace FribergCarRentals.Pages.Admin.CarCategories
{
    /// <summary>
    /// A page model for creating car categories in the admin back office.
    /// </summary>
    public class CreateModel : PageModel
    {
        #region Constants

        /// <summary>
        /// The key for the ID of the car category that was created.
        /// </summary>
        public const string CreatedCarCategoryIdTempDataKey = "AdminCreatedCarCategoryId";

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
        public CreateModel(ICarCategoryRepository carCategoryRepository)
        {
            _carCategoryRepository = carCategoryRepository;
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
        public IActionResult OnGetAsync()
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin();
            }

            return Page();
        }

        /// <summary>
        /// Handler for POST requests.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin();
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (!DataTransferHelper.TryTransferData(CreateCarCategoryViewModel, out CarCategoryEntity category))
                {
                    throw new Exception("Failed to transfer data from the view model to the entity");
                }

                await _carCategoryRepository.AddAsync(category);
                TempDataHelper.Set(TempData, CreatedCarCategoryIdTempDataKey, category.CarCategoryId);
                return RedirectToPage("Details", new { id = category.CarCategoryId });
            }

            return Page();
        }

        #endregion

        #region OtherMethods

        /// <summary>
        /// Redirects to the login page and request a redirect back afterwards. 
        /// </summary>
        /// <returns></returns>
        private IActionResult RedirectToLogin()
        {
            TempDataHelper.Set(TempData, LoginModel.RedirectToPageTempDataKey, new RedirectToPageData(
                    "Car/Category/Create/"));

            return RedirectToPage("../Login");
        }

        #endregion
    }
}
