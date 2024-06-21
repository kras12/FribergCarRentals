using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FribergCarRentals.DataAccess.Repositories;
using MvcRazorPages.Shared.Sessions;
using MvcRazorPages.Shared.Data;
using MvcRazorPages.Shared.Helpers;

namespace FribergCarRentals.Pages.Admin.Car
{
    /// <summary>
    /// Page model for deleting a car in the admin back office.
    /// </summary>
    public class DeleteModel : PageModel
    {
        #region Constants

        /// <summary>
        /// The key for the ID of the car that was deleted. 
        /// </summary>
        public const string DeletedCarIdTempDataKey = "AdminDeletedCarId";

        /// <summary>
        /// The key for the redirect data containing the page to redirect to after deleting a car.
        /// </summary>
        public const string RedirectToPageAfterDeleteTempDataKey = "AdminDeletedCarRedirectToPage";

        #endregion

        #region Fields

        /// <summary>
        /// The injected car repository.
        /// </summary>
        private readonly ICarRepository _carRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="carRepository">Injected car repository.</param>
        public DeleteModel(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        #endregion

        #region HandlerMethods        

        /// <summary>
        /// Handler for POST requests. 
        /// </summary>
        /// <param name="id">The ID of the car to delete.</param>
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
                var car = await _carRepository.GetByIdAsync(id);

                if (car!.Images.Count > 0)
                {
                    ImageHelper.DeleteImagesFromDisk(car!.Images.Select(x => x.FileName));
                }

                await _carRepository.DeleteAsync(id);
                TempDataHelper.Set(TempData, DeletedCarIdTempDataKey, id);

                if (TempDataHelper.TryGet(TempData, RedirectToPageAfterDeleteTempDataKey, out RedirectToPageData? data))
                {
                    return RedirectToPage(data.Page, data.RouteValues);
                }
                else
                {
                    return RedirectToPage("List");
                }
            }

            throw new Exception($"Failed to delete the car with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid} - IsAdminLoggedIn: {UserSessionHandler.IsAdminLoggedIn(HttpContext.Session)}");
        }

        #endregion

        #region OtherMethods

        /// <summary>
        /// Redirects to the login page and request a redirect to the car details page. 
        /// </summary>
        /// <param name="id">The ID of the car to delete.</param>
        /// <returns>A <see cref="IActionResult"/>.</returns>
        private IActionResult RedirectToLogin(int id)
        {
            TempDataHelper.Set(TempData, LoginModel.RedirectToPageTempDataKey, new RedirectToPageData(
                    "Car/Details",
                    new RouteValueDictionary(new { id = id })));

            return RedirectToPage("../Login");
        }

        #endregion
    }
}
