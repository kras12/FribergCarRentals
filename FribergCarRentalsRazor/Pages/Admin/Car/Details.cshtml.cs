using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FribergCarRentals.DataAccess.Repositories;
using FribergCarRentals.Helpers;
using FribergCarRentals.Sessions;
using FribergCarRentalsRazor.Helpers;
using FribergCarRentals.Models.Car;
using MvcRazorPages.Shared.Data;

namespace FribergCarRentals.Pages.Admin.Car
{
    /// <summary>
    /// Page model for showing details for a car in the admin back office. 
    /// </summary>
    public class DetailsModel : PageModel
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
        public DetailsModel(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The view model used for showing car details. 
        /// </summary>
        public CarViewModel CarViewModel { get; set; } = null!;

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
                var car = await _carRepository.GetByIdAsync(id);

                if (car is not null)
                {
                    CarViewModel = new CarViewModel(car);

                    if (TempDataHelper.TryGet(TempData, CreateModel.CreatedCarIdTempDataKey, out int carId))
                    {
                        CarViewModel.Messages.Add(UserMesssageHelper.CreateCarCreationSuccessMessage(carId));
                    }

                    return Page();
                }
            }

            throw new Exception($"Failed to show the car with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        #endregion

        #region OtherMethods

        /// <summary>
        /// Redirects to the login page and request a redirect back afterwards. 
        /// </summary>
        /// <param name="id">The ID of the car to view.</param>
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
