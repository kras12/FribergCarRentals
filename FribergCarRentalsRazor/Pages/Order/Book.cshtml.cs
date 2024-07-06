using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.Data;
using MvcRazorPages.Shared.Helpers;
using FribergCarRentals.Pages.Customer;
using MvcRazorPages.Shared.ViewModels.Order;
using FribergFastigheter.Server.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FribergFastigheter.Shared.Constants;
using MvcRazorPages.Shared.ViewModels.Car;
using MvcRazorPages.Shared.Services;

namespace FribergCarRentals.Pages.Order
{
    /// <summary>
    /// Page model for booking a car and creating an order. 
    /// </summary>
    public class BookModel : PageModelBase
    {
        #region Constants

        /// <summary>
        /// The key for the created order flag stored in temp storage.
        /// </summary>
        public const string IsNewOrderTempDataKey = "CustomerBookModelIsNewOrder";

        /// <summary>
        /// The key for storing the pending order to be confirmed by the customer.
        /// </summary>
        public const string PendingOrderTempDataKey = "CustomerOrderPendingOrder";

        #endregion

        #region Fields

        /// <summary>
        /// Injected car category repository. 
        /// </summary>
        private readonly ICarCategoryRepository _carCategoryRepository;

        /// <summary>
        /// Injected car repository. 
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
        /// <param name="carRepository">Injected car repository. </param>
        /// <param name="carCategoryRepository">Injected car category repository.</param>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="signInManager">The injected signin manager.</param>
        /// <param name="imageUploadService">The injected image upload service</param>
        public BookModel(ICarRepository carRepository, ICarCategoryRepository carCategoryRepository, IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager, IImageUploadService imageUploadService) : base(authorizationService, signInManager)
        {
            _carRepository = carRepository;
            _carCategoryRepository = carCategoryRepository;
            _imageUploadService = imageUploadService;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The view model used to book a car. 
        /// </summary>
        [BindProperty]
        public BookCarViewModel BookCarViewModel { get; set; } = new();

        #endregion

        #region HandlerMethods

        public async Task<IActionResult> OnPostPrepare(CreateOrderViewModel createOrderViewModel)
        {
            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (createOrderViewModel.PickupDateLocalTime.Date < DateTime.Now.Date)
                {
                    throw new Exception("The pickup date can't be in the past.");
                }
                else if (createOrderViewModel.ReturnDateLocalTime.Date < createOrderViewModel.PickupDateLocalTime.Date)
                {
                    throw new Exception("The return date can't occur before the pickup date.");
                }

                TempDataHelper.Set(TempData, PendingOrderTempDataKey, createOrderViewModel);

                if (!await IsCustomerLoggedIn())
                {
                    return RedirectToLogin(page: "../Order/Confirm");
                }

                return RedirectToPage("Confirm");
            }

            throw new Exception($"Failed to prepare order for the car with id: {createOrderViewModel.CarId} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        /// <summary>
        /// Handler for GET requests. 
        /// </summary>
        /// <param name="carId">The ID of the car to create an order for.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing an <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnGetAsync(int? carCategoryId)
        {
            if (carCategoryId < 0)
            {
                throw new Exception($"Invalid ID: {carCategoryId}");
            }

            BookCarViewModel = new BookCarViewModel(availableCarCategoryFilters: (await _carCategoryRepository.GetAllAsync()).ToList(), havePerformedCarSearch: false);

            if (carCategoryId != null)
            {
                BookCarViewModel.SelectedCarCategoryFilter = carCategoryId.Value;
            }

            return Page();
        }

        /// <summary>
        /// Handler for POST requests. 
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> containing an <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (BookCarViewModel.PickupDateLocalTime is null || BookCarViewModel.PickupDateLocalTime <= DateTime.Now)
                {
                    ModelState.AddModelError($"{nameof(BookCarViewModel.PickupDateLocalTime)}",
                        "The pickup date must be at least one day into the future.");
                }
                else if (BookCarViewModel.ReturnDateLocalTime is null || BookCarViewModel.ReturnDateLocalTime.Value.Date < BookCarViewModel.PickupDateLocalTime.Value.Date)
                {
                    ModelState.AddModelError($"{nameof(BookCarViewModel.ReturnDateLocalTime)}",
                        "The return date can't occur before the pickup date.");
                }
                else
                {
                    CarCategoryEntity? carCategoryFilter = null;

                    if (BookCarViewModel.SelectedCarCategoryFilter > 0)
                    {
                        carCategoryFilter = await _carCategoryRepository.GetByIdAsync(BookCarViewModel.SelectedCarCategoryFilter);

                        if (carCategoryFilter is null)
                        {
                            throw new Exception("Failed to find the car category");
                        }
                    }

                    var cars = (await _carRepository.GetRentableCarsAsync(BookCarViewModel.PickupDateLocalTime.Value, BookCarViewModel.ReturnDateLocalTime.Value, carCategoryFilter)).ToList();
                    BookCarViewModel = new BookCarViewModel(
                        availableCarCategoryFilters: (await _carCategoryRepository.GetAllAsync()).ToList(),
                        havePerformedCarSearch: true,
                        availableCars: cars.Select(x => new CarViewModel(x, _imageUploadService)).ToList(),
                        pickupDateFilter: BookCarViewModel.PickupDateLocalTime,
                        returnDateFilter: BookCarViewModel.ReturnDateLocalTime,
                        carCategoryFilter: BookCarViewModel.SelectedCarCategoryFilter);

                    return Page();
                }
            }

            BookCarViewModel.SetAvailableCarCategoryFilters(await _carCategoryRepository.GetAllAsync());
            return Page();
        }

        #endregion

        #region OtherMethods

        /// <summary>
        /// Redirects to the login page and request a redirect afterwards. 
        /// </summary>
        /// <param name="page">The page to redirect to.</param>
        /// <returns><see cref="IActionResult"/>.</returns>
        private IActionResult RedirectToLogin(string page)
        {
            TempDataHelper.Set(TempData, AuthenticateModel.RedirectInstructionsTempDataKey, new RedirectToPageData(page));
            return RedirectToPage("../Customer/Authenticate");
        }

        #endregion
    }
}
