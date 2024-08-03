using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.Helpers;
using MvcRazorPages.Shared.Data;
using MvcRazorPages.Shared.ViewModels.Car;
using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MvcRazorPages.Shared.Services;

namespace FribergCarRentals.Areas.Admin.Pages.Car
{
    /// <summary>
    /// Page model for showing details for a car in the admin back office. 
    /// </summary>
    public class DetailsModel : AdminPageModelBase
    {
        #region Constants

        /// <summary>
        /// The page URL relative to the login page
        /// </summary>
        public const string PageUrlRelativeToLoginPage = "Car/Details";

        #endregion

        #region Fields

        /// <summary>
        /// The injected car repository.
        /// </summary>
        private readonly ICarRepository _carRepository;

        /// <summary>
        /// The injected image upload service.
        /// </summary>
        private readonly IImageUploadService _imageUploadService;

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="carRepository">Injected car repository.</param>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="signInManager">The injected signin manager.</param>
        /// <param name="imageUploadService">The injected image upload service.</param>
        public DetailsModel(ICarRepository carRepository, IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager, IImageUploadService imageUploadService) : base(authorizationService, signInManager)
        {
            _carRepository = carRepository;
            _imageUploadService = imageUploadService;
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
                var car = await _carRepository.GetByIdAsync(id);

                if (car is not null)
                {
                    CarViewModel = new CarViewModel(car, _imageUploadService);

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
    }
}
