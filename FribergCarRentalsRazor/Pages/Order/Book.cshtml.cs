using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.Data;
using MvcRazorPages.Shared.Helpers;
using MvcRazorPages.Shared.ViewModels.Order;
using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MvcRazorPages.Shared.ViewModels.Car;
using MvcRazorPages.Shared.Services;

namespace FribergCarRentals.Pages.Order
{
    /// <summary>
    /// Page model for booking a car and creating an order. 
    /// </summary>
    public class BookModel : CustomerPageModelBase
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

        #region ErrorMessages

        /// <summary>
        /// Error message for when the pickup date is not in the future. 
        /// </summary>
        private const string PickupDateMustBeInFutureErrorMessage = "The pickup date must be at least one day into the future.";

        /// <summary>
        /// Error message for when the return date occurrs before the pickup date.
        /// </summary>
        private const string ReturnDateOccursBeforePickupDateErrorMessage = "The return date can't occur before the pickup date.";

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
                if (!ValidatePickupDate(createOrderViewModel.PickupDateLocalTime))
                {
                    throw new Exception(PickupDateMustBeInFutureErrorMessage);
                }
                else if (!ValidateReturnDate(createOrderViewModel.PickupDateLocalTime, createOrderViewModel.ReturnDateLocalTime))
                {
                    throw new Exception(ReturnDateOccursBeforePickupDateErrorMessage);
                }

                TempDataHelper.Set(TempData, PendingOrderTempDataKey, createOrderViewModel);

                if (!await IsCustomerLoggedIn())
                {
                    return RedirectToLogin(new RedirectToPageData("../Order/Confirm"));
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
                if (!ValidatePickupDate(BookCarViewModel.PickupDateLocalTime))
                {
                    ModelState.AddModelError($"{nameof(BookCarViewModel.PickupDateLocalTime)}",
                        PickupDateMustBeInFutureErrorMessage);
                }
                else if (!ValidateReturnDate(BookCarViewModel.PickupDateLocalTime, BookCarViewModel.ReturnDateLocalTime))
                {
                    ModelState.AddModelError($"{nameof(BookCarViewModel.ReturnDateLocalTime)}",
                        ReturnDateOccursBeforePickupDateErrorMessage);
                }
                else
                {
                    int? selectedCarCategoryFilter = null;

                    if (BookCarViewModel.SelectedCarCategoryFilter > 0)
                    {
                        if (!await _carCategoryRepository.CategoryExists(BookCarViewModel.SelectedCarCategoryFilter))
                        {
                            throw new Exception("Failed to find the car category");
                        }

                        selectedCarCategoryFilter = BookCarViewModel.SelectedCarCategoryFilter;
                    }

                    var cars = (await _carRepository.GetRentableCarsAsync(BookCarViewModel.PickupDateLocalTime, BookCarViewModel.ReturnDateLocalTime, selectedCarCategoryFilter)).ToList();
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
        /// Validates the pickup date for car rentals.
        /// </summary>
        /// <param name="pickupDate">The pickup date.</param>
        /// <returns>True if the date is valid.</returns>
        private bool ValidatePickupDate(DateTime pickupDate)
        {
            return pickupDate.Date > DateTime.Now.Date;
        }

        /// <summary>
        /// Validates the return date for car rentals.
        /// </summary>
        /// <param name="pickupDate">The pickup date.</param>
        /// <param name="returnDate">The return date.</param>
        /// <returns>True if the date is valid.</returns>
        private bool ValidateReturnDate(DateTime pickupDate, DateTime returnDate)
        {
            return returnDate >= pickupDate;
        }

        #endregion
    }
}
