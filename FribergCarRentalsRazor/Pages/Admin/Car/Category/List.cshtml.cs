using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FribergCarRentals.DataAccess.Repositories;
using MvcRazorPages.Shared.Sessions;
using MvcRazorPages.Shared.Helpers;
using FribergCarRentals.Models.Other;
using FribergCarRentals.Models.CarCategory;
using MvcRazorPages.Shared.Data;

namespace FribergCarRentals.Pages.Admin.CarCategories
{
    /// <summary>
    /// Page model class for listing car categories in the admin back office.
    /// </summary>
    public class ListModel : PageModel
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
        public ListModel(ICarCategoryRepository carCategoryRepository)
        {
            _carCategoryRepository = carCategoryRepository;
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
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin();
            }

            CarCategoryListViewModel = new((await _carCategoryRepository.GetCategoryStatistics()).Select(x => new CarCategoryViewModel(x)));
            SaveRedirectBackInstructionsForDeleteCarCategoryAction("List");

            if (TempDataHelper.TryGet(TempData, DeleteModel.DeletedCarCategoryIdTempDataKey, out int deletedCarCategoryId))
            {
                CarCategoryListViewModel.Messages.Add(UserMesssageHelper.CreateCarCategoryDeletionSuccessMessage(deletedCarCategoryId));
            }

            return Page();
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
                    "Car/Category/List"));

            return RedirectToPage("../Login");
        }

        /// <summary>
        /// Saves data for redirecting back to an action after a car category has been deleted. 
        /// </summary>
        /// <param name="redirectToAction">The action to redirect to.</param>
        private void SaveRedirectBackInstructionsForDeleteCarCategoryAction(string redirectToAction)
        {
            TempDataHelper.Set(TempData, DeleteModel.RedirectToPageAfterDeleteTempDataKey, new RedirectToPageData(
                    "List"));
        }

        #endregion
    }
}
