using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FribergCarRentals.DataAccess.Repositories;
using FribergCarRentals.Sessions;
using MvcRazorPages.Shared.Helpers;
using MvcRazorPages.Shared.Data;

namespace FribergCarRentals.Pages.Admin.CarCategories
{
    /// <summary>
    /// Page model for deleting a car category in the admin back office.
    /// </summary>
    public class DeleteModel : PageModel
    {
        #region Constants

        /// <summary>
        /// The key for the ID of the car category that was deleted. 
        /// </summary>
        public const string DeletedCarCategoryIdTempDataKey = "AdminDeletedCarCategoryId";

        /// <summary>
        /// The key for the deleted car category redirect data stored in temp storage.
        /// </summary>
        public const string RedirectToPageAfterDeleteTempDataKey = "AdminDeletedCarCategoryRedirectToPage";

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
        public DeleteModel(ICarCategoryRepository carCategoryRepository)
        {
            _carCategoryRepository = carCategoryRepository;
        }

        #endregion

        #region HandlerMethods        

        /// <summary>
        /// Handler for POST requests. 
        /// </summary>
        /// <param name="id">The ID of the car category to delete.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnPostAsync(int id)
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
                await _carCategoryRepository.DeleteAsync(id);
                TempDataHelper.Set(TempData, DeletedCarCategoryIdTempDataKey, id);

                if (TempDataHelper.TryGet(TempData, RedirectToPageAfterDeleteTempDataKey, out RedirectToPageData? data))
                {
                    return RedirectToPage(data.Page, data.RouteValues);
                }
                else
                {
                    return RedirectToPage("List");
                }
            }

            throw new Exception($"Failed to delete the car category with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid} - IsAdminLoggedIn: {UserSessionHandler.IsAdminLoggedIn(HttpContext.Session)}");
        }

        #endregion

        #region OtherMethods

        /// <summary>
        /// Redirects to the login page and request a redirect back afterwards. 
        /// </summary>
        /// <param name="id">The ID of the car category to delete.</param>
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
