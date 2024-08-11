using MvcRazorPages.Shared.Data;
using MvcRazorPages.Shared.Helpers;
using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Data.Repositories;
using FribergCarRentals.Helpers;
using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MvcRazorPages.Shared.Services;
using FribergCarRentals.Shared.Models.ViewModels.Order;
using FribergCarRentals.Shared.Models.ViewModels.Other;
using FribergCarRentals.Shared.Types.Enums;
using FribergCarRentals.Shared.Models.ViewModels.Image;
using AutoMapper;
using FribergCarRentals.Shared.Models.ViewModels.CarCategory;
using FribergCarRentals.Shared.Models.ViewModels.Car;
using FribergCarRentals.Shared.Constants;

namespace FribergCarRentals.Controllers.Customer
{
    [Route($"{CurrentControllerRoutePart}/[action]")]
    public class CustomerOrderController : CustomerControllerBase
    {
        #region Constants

        /// <summary>
        /// The key for the ID of the order that was canceled.
        /// </summary>
        public const string CanceledOrderIdTempDataKey = "CustomerCanceledOrderId";

        /// <summary>
        /// The key for the canceled order redirect data stored in temp storage.
        /// </summary>
        public const string CanceledOrderRedirectToPageTempDataKey = "CustomerCanceledOrderRedirectToPage";

        /// <summary>
        /// The key for the created order flag stored in temp storage.
        /// </summary>
        public const string IsNewOrderTempDataKey = "CustomerOrderControllerIsNewOrder";

        /// <summary>
        /// The key for storing the pending order to be confirmed by the customer.
        /// </summary>
        public const string PendingOrderTempDataKey = "CustomerOrderPendingOrder";

        /// <summary>
        /// The route part for this controller.
        /// </summary>
        private const string CurrentControllerRoutePart = "Order";

        #endregion

        #region Fields

        /// <summary>
        /// The injected car category repository.
        /// </summary>
        private readonly ICarCategoryRepository _carCategoryRepository;

        /// <summary>
        /// The injected car repository.
        /// </summary>
        private readonly ICarRepository _carRepository;

        /// <summary>
        /// The injected customer repository.
        /// </summary>
        private readonly ICustomerRepository _customerRepository;

        /// <summary>
        /// The injected image upload service.
        /// </summary>
        private readonly IImageUploadService _imageUploadService;

		// The injected Auto Mapper.
		private readonly IMapper _mapper;

		/// <summary>
		/// The injected order repository.
		/// </summary>
		private readonly ICarOrderRepository _orderRepository;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="orderRepository">The injected order repository.</param>
		/// <param name="customerRepository">The injected customer repository.</param>
		/// <param name="carRepository">The injected car repository.</param>
		/// <param name="carCategoryRepository">The injected car category repository.</param>
		/// <param name="authorizationService"></param>
		/// <param name="signInManager"></param>
		/// <param name="authorizationService">The injected authorization service.</param>
		/// <param name="signInManager">The injected signin manager.</param>
		/// <param name="imageUploadService">The injected image upload service.</param>
		/// <param name="mapper">The injected Auto Mapper.</param>
		public CustomerOrderController(ICarOrderRepository orderRepository, ICustomerRepository customerRepository,
			ICarRepository carRepository, ICarCategoryRepository carCategoryRepository, IAuthorizationService authorizationService,
			SignInManager<ApplicationUser> signInManager, IImageUploadService imageUploadService, IMapper mapper) : base(authorizationService, signInManager)
		{
			_orderRepository = orderRepository;
			_customerRepository = customerRepository;
			_carRepository = carRepository;
			_carCategoryRepository = carCategoryRepository;
			_imageUploadService = imageUploadService;
			_mapper = mapper;
		}

		#endregion

		#region Actions

		// GET: CustomerOrderController/Book
		[HttpGet]
        public async Task<IActionResult> Book(int? carCategoryId)
        {
            if (carCategoryId < 0)
            {
                throw new Exception($"Invalid ID: {carCategoryId}");
            }

            var viewModel = new BookCarViewModel(availableCarCategoryFilters: _mapper.Map<List<CarCategoryViewModel>>(await _carCategoryRepository.GetAllAsync()), havePerformedCarSearch: false);

            if (carCategoryId != null)
            {
                viewModel.SelectedCarCategoryFilter = carCategoryId.Value;
            }

            return View(viewModel);
        }

        // POST: CustomerOrderController/Book
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(BookCarViewModel bookCarViewModel)
        {
            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (!ValidatePickupDate(bookCarViewModel.PickupDateLocalTime))
                {
                    ModelState.AddModelError($"{nameof(BookCarViewModel.PickupDateLocalTime)}",
                        ValidationMessages.PickupDateMustBeInFutureErrorMessage);
                }
                else if (!ValidateReturnDate(bookCarViewModel.PickupDateLocalTime, bookCarViewModel.ReturnDateLocalTime))
                {
                    ModelState.AddModelError($"{nameof(BookCarViewModel.ReturnDateLocalTime)}",
                        ValidationMessages.ReturnDateOccursBeforePickupDateErrorMessage);
                }
                else
                {
                    int? selectedCarCategoryFilter = null;

                    if (bookCarViewModel.SelectedCarCategoryFilter > 0)
                    {
                        if (!await _carCategoryRepository.CategoryExists(bookCarViewModel.SelectedCarCategoryFilter))
                        {
                            throw new Exception("Failed to find the car category");
                        }

                        selectedCarCategoryFilter = bookCarViewModel.SelectedCarCategoryFilter;
                    }

					var cars = (await _carRepository.GetRentableCarsAsync(bookCarViewModel.PickupDateLocalTime, bookCarViewModel.ReturnDateLocalTime, selectedCarCategoryFilter)).ToList();
					List<CarViewModel> availableCars = _mapper.Map<List<CarViewModel>>(cars);
					SetImageUrls(availableCars.SelectMany(x => x.Images).ToList());

                    return View(new BookCarViewModel(
                        availableCarCategoryFilters: _mapper.Map<List<CarCategoryViewModel>>(await _carCategoryRepository.GetAllAsync()),
                        havePerformedCarSearch: true,
                        availableCars: availableCars,
                        pickupDateFilter: bookCarViewModel.PickupDateLocalTime,
                        returnDateFilter: bookCarViewModel.ReturnDateLocalTime,
                        carCategoryFilter: bookCarViewModel.SelectedCarCategoryFilter));
                }
            }

            bookCarViewModel.SetAvailableCarCategoryFilters(_mapper.Map<List<CarCategoryViewModel>>(await _carCategoryRepository.GetAllAsync()));
			return View(bookCarViewModel);
        }

        // POST: CustomerOrderController/Cancel/(5)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            if (!await IsCustomerLoggedIn())
            {
                return RedirectToLogin(new RedirectToActionData(nameof(Cancel), ControllerHelper.GetControllerName<CustomerOrderController>(), new RouteValueDictionary(new { id })));
            }

            if (id < 0)
            {
                throw new Exception($"Invalid ID: {id}");
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (!await _orderRepository.Exists(id))
                {
                    throw new Exception($"Order was not found.");
                }

                if (await _orderRepository.TryCancelOrderAsync(id))
                {
                    TempDataHelper.Set(TempData, CanceledOrderIdTempDataKey, id);

                    if (TempDataHelper.TryGet(TempData, CanceledOrderRedirectToPageTempDataKey, out RedirectToActionData? data))
                    {
                        return RedirectToAction(data.Action, data.Controller, data.RouteValues);
                    }
                    else
                    {
                        return RedirectToAction(nameof(Details), new { id = id });
                    }
                }

                throw new Exception($"Failed to cancel order with id: {id}");
            }

            throw new Exception($"Model validation failed: UserId: {User.FindFirst(x => x.Type == ApplicationUserClaims.UserId)!.Value} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }
        // GET: CustomerOrderController/Confirm
        [HttpGet]
        public async Task<IActionResult> Confirm()
        {
            if (!await IsCustomerLoggedIn())
            {
                return RedirectToLogin(new RedirectToActionData(nameof(Book), ControllerHelper.GetControllerName<CustomerOrderController>()));
            }

            if (TempDataHelper.TryGet(TempData, PendingOrderTempDataKey, out CreateOrderViewModel? createOrderViewModel))
            {
                // Customer may refresh the page, and we need it in order to complete the order.
                TempDataHelper.Set(TempData, PendingOrderTempDataKey, createOrderViewModel);

                return View(createOrderViewModel);
            }

            throw new Exception($"Failed to retrieve the pending order from temp storage.");
        }

        // GET: CustomerOrderController/Create
        [HttpPost]
        public async Task<IActionResult> Create()
        {
            if (!await IsCustomerLoggedIn())
            {
                return RedirectToLogin(new RedirectToActionData(nameof(Book), ControllerHelper.GetControllerName<CustomerOrderController>()));
            }

            if (TempDataHelper.TryGet(TempData, PendingOrderTempDataKey, out CreateOrderViewModel? createOrderViewModel))
            {
                var userId = User.FindFirst(x => x.Type == ApplicationUserClaims.UserId)!.Value;
                var customer = await _customerRepository.GetByUserIdAsync(userId);
                var car = await _carRepository.GetByIdAsync(createOrderViewModel.CarId);

                if (customer == null)
                {
                    throw new Exception("Customer was not found.");
                }

                if (car == null)
                {
                    throw new Exception("Car was not found.");
                }

                car.RentalStatus = CarRentalStatusEntity.CreateFromType(RentalCarStatus.PendingPickup);
                var order = new CarOrderEntity(customer);
                order.CarBookings.Add(
                    new CarBookingEntity(order, car, pickupDateUTC: createOrderViewModel.PickupDateLocalTime.Date,
                        returnDateUTC: createOrderViewModel.ReturnDateLocalTime.Date));

                await _orderRepository.AddAsync(order);
                TempDataHelper.Set(TempData, IsNewOrderTempDataKey, true);
                return RedirectToAction(nameof(Details), new { id = order.CarOrderId });
            }

            throw new Exception($"Failed to retrieve the pending order from temp storage.");
        }

        // GET: CustomerOrderController
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if (!await IsCustomerLoggedIn())
            {
                return RedirectToLogin(new RedirectToActionData(nameof(Details), ControllerHelper.GetControllerName<CustomerOrderController>(), new RouteValueDictionary(new { id })));
            }

            if (id < 0)
            {
                throw new Exception($"Invalid ID: {id}");
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var order = await _orderRepository.GetByIdAsync(id);

                if (order is not null)
                {
                    TempDataHelper.TryGet(TempData, IsNewOrderTempDataKey, out bool orderWasCreated);
                    OrderViewModel orderViewModel = _mapper.Map<OrderViewModel>(order);
                    orderViewModel.IsNewOrder = orderWasCreated;
                    SetImageUrls(orderViewModel.CarBooking.Car.Images);
                    
                    SaveRedirectBackInstructionsForCancelOrderAction(nameof(Details), id);

                    if (TempDataHelper.TryGet(TempData, CanceledOrderIdTempDataKey, out int canceledOrderId))
                    {
                        orderViewModel.Messages.Add(UserMesssageHelper.CreateOrderCancellationSuccessMessage(canceledOrderId));
                    }

                    return View(orderViewModel);
                }

                throw new Exception($"Failed to retrieve the order from the database. - OrderID: {id}");
            }

            throw new Exception($"Model validation failed: - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        // GET: CustomerOrderController
        [HttpGet]
        public async Task<IActionResult> List()
        {
            if (!await IsCustomerLoggedIn())
            {
                return RedirectToLogin(new RedirectToActionData(nameof(List), ControllerHelper.GetControllerName<CustomerOrderController>()));
            }

            var userId = User.FindFirst(x => x.Type == ApplicationUserClaims.UserId)!.Value;
            var customer = await _customerRepository.GetByUserIdAsync(userId) ?? throw new Exception($"Failed to find customer with user ID: {userId}");

			List<OrderViewModel> orderViewModels = _mapper.Map<List<OrderViewModel>>(customer.Orders).OrderByDescending(x => x.CarOrderId).ToList();
			SetImageUrls(orderViewModels.SelectMany(x => x.CarBooking.Car.Images).ToList());
			ListViewModel<OrderViewModel> orderListViewModel = new ListViewModel<OrderViewModel>(orderViewModels);

            if (TempDataHelper.TryGet(TempData, CanceledOrderIdTempDataKey, out int canceledOrderId))
            {
                orderListViewModel.Messages.Add(UserMesssageHelper.CreateOrderCancellationSuccessMessage(canceledOrderId));
            }

            SaveRedirectBackInstructionsForCancelOrderAction(nameof(List));
            return View(orderListViewModel);
        }


        // POST: CustomerOrderController/Prepare
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Prepare(CreateOrderViewModel createOrderViewModel)
        {
            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (!ValidatePickupDate(createOrderViewModel.PickupDateLocalTime))
                {
                    throw new Exception(ValidationMessages.PickupDateMustBeInFutureErrorMessage);
                }
                else if (!ValidateReturnDate(createOrderViewModel.PickupDateLocalTime, createOrderViewModel.ReturnDateLocalTime))
                {
                    throw new Exception(ValidationMessages.ReturnDateOccursBeforePickupDateErrorMessage);
                }

                TempDataHelper.Set(TempData, PendingOrderTempDataKey, createOrderViewModel);

                if (!await IsCustomerLoggedIn())
                {
                    return RedirectToLogin(new RedirectToActionData(nameof(Confirm), ControllerHelper.GetControllerName<CustomerOrderController>()));
                }

                return RedirectToAction(nameof(Confirm));
            }

            throw new Exception($"Failed to prepare order for the car with id: {createOrderViewModel.CarId} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        #endregion

        #region OtherMethods

        /// <summary>
        /// Saves data for redirecting back to an action after an order has been cancelled.
        /// </summary>
        /// <param name="redirectToAction">The action to redirect to.</param>
        /// <param name="id">An optional ID for the order.</param>
        private void SaveRedirectBackInstructionsForCancelOrderAction(string redirectToAction, int? id = null)
        {
            RouteValueDictionary? routeValues = id is not null ? new RouteValueDictionary(new { id = id }) : null;
            TempDataHelper.Set(TempData, CanceledOrderRedirectToPageTempDataKey, new RedirectToActionData(
                    redirectToAction, ControllerHelper.GetControllerName<CustomerOrderController>(), routeValues: routeValues));
        }

		/// <summary>
		/// Sets the image urls for image view models.
		/// </summary>
		/// <param name="imageViewModels">A collection of image view models to process.</param>
		private void SetImageUrls(List<ImageViewModel> imageViewModels)
		{
			imageViewModels.ForEach(x => x.Url = _imageUploadService.GetImageUrl(x.FileName));
		}

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
