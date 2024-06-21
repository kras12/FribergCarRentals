using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FribergCarRentals.DataAccess.Repositories;
using MvcRazorPages.Shared.Data;
using MvcRazorPages.Shared.Sessions;
using MvcRazorPages.Shared.Helpers;
using MvcRazorPages.Shared.ViewModels.Other;
using MvcRazorPages.Shared.ViewModels.Car;

namespace FribergCarRentals.Pages.Admin.Car
{
    /// <summary>
    /// Page model class for listing cars in the admin back office.
    /// </summary>
    public class ListModel : PageModel
    {
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
        public ListModel(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        #endregion

        #region Properties

        /// <summary>
        /// A view model used to present a list of cars. 
        /// </summary>
        public ListViewModel<CarViewModel> CarListViewModel { get; private set; } = new();

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

            CarListViewModel = new ListViewModel<CarViewModel>((await _carRepository.GetAllAsync()).Select(x => new CarViewModel(x)));
            SaveRedirectBackInstructionsForDeleteCarAction();

            if (TempDataHelper.TryGet(TempData, DeleteModel.DeletedCarIdTempDataKey, out int deletedCarId))
            {
                CarListViewModel.Messages.Add(UserMesssageHelper.CreateCarDeletionSuccessMessage(deletedCarId));
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
                    "Car/List"));

            return RedirectToPage("../Login");
        }

        /// <summary>
        /// Saves data for redirecting back to this page after a car has been deleted by the delete page.
        /// </summary>
        private void SaveRedirectBackInstructionsForDeleteCarAction()
        {
            TempDataHelper.Set(TempData, DeleteModel.RedirectToPageAfterDeleteTempDataKey, new RedirectToPageData(
                    "List"));
        }

        #endregion
    }
}
