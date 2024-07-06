using MvcRazorPages.Shared.Data;
using MvcRazorPages.Shared.Helpers;
using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Data.Repositories;
using FribergCarRentals.Data.Types;
using MvcRazorPages.Shared.ViewModels.Order;
using MvcRazorPages.Shared.ViewModels.Other;
using FribergCarRentals.Helpers;
using FribergFastigheter.Server.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FribergFastigheter.Shared.Constants;
using MvcRazorPages.Shared.ViewModels.Car;
using MvcRazorPages.Shared.Services;

namespace FribergCarRentals.Controllers.Customer
{
    [Route($"{CurrentControllerRoutePart}/[action]")]
    public class CustomerOrderController : ViewControllerBase
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
        public CustomerOrderController(ICarOrderRepository orderRepository, ICustomerRepository customerRepository,
            ICarRepository carRepository, ICarCategoryRepository carCategoryRepository, IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager, IImageUploadService imageUploadService) : base(authorizationService, signInManager)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _carRepository = carRepository;
            _carCategoryRepository = carCategoryRepository;
            _imageUploadService = imageUploadService;
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

            var viewModel = new BookCarViewModel(availableCarCategoryFilters: (await _carCategoryRepository.GetAllAsync()).ToList(), havePerformedCarSearch: false);

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
                if (bookCarViewModel.PickupDateLocalTime is null || bookCarViewModel.PickupDateLocalTime <= DateTime.Now)
                {
                    ModelState.AddModelError($"{nameof(BookCarViewModel.PickupDateLocalTime)}",
                        "The pickup date must be at least one day into the future.");
                }
                else if (bookCarViewModel.ReturnDateLocalTime is null || bookCarViewModel.ReturnDateLocalTime.Value.Date < bookCarViewModel.PickupDateLocalTime.Value.Date)
                {
                    ModelState.AddModelError($"{nameof(BookCarViewModel.ReturnDateLocalTime)}",
                        "The return date can't occur before the pickup date.");
                }
                else
                {
                    CarCategoryEntity? carCategoryFilter = null;

                    if (bookCarViewModel.SelectedCarCategoryFilter > 0)
                    {
                        carCategoryFilter = await _carCategoryRepository.GetByIdAsync(bookCarViewModel.SelectedCarCategoryFilter);

                        if (carCategoryFilter is null)
                        {
                            throw new Exception("Failed to find the car category");
                        }
                    }

                    var cars = (await _carRepository.GetRentableCarsAsync(bookCarViewModel.PickupDateLocalTime.Value, bookCarViewModel.ReturnDateLocalTime.Value, carCategoryFilter)).ToList();

                    return View(new BookCarViewModel(
                        availableCarCategoryFilters: (await _carCategoryRepository.GetAllAsync()).ToList(),
                        havePerformedCarSearch: true,
                        availableCars: cars.Select(x => new CarViewModel(x, _imageUploadService)).ToList(),
                        pickupDateFilter: bookCarViewModel.PickupDateLocalTime,
                        returnDateFilter: bookCarViewModel.ReturnDateLocalTime,
                        carCategoryFilter: bookCarViewModel.SelectedCarCategoryFilter));
                }
            }

            bookCarViewModel.SetAvailableCarCategoryFilters(await _carCategoryRepository.GetAllAsync());
            return View(bookCarViewModel);
        }

        // POST: CustomerOrderController/Cancel/(5)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            if (!await IsCustomerLoggedIn())
            {
                return RedirectToLogin(nameof(Cancel), id);
            }

            if (id < 0)
            {
                throw new Exception($"Invalid ID: {id}");
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
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
                return RedirectToLogin(nameof(Book));
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
                return RedirectToLogin(nameof(Book));
            }

            if (TempDataHelper.TryGet(TempData, PendingOrderTempDataKey, out CreateOrderViewModel? createOrderViewModel))
            {
                var userId = User.FindFirst(x => x.Type == ApplicationUserClaims.UserId)!.Value;
                var customer = await _customerRepository.GetByUserIdAsync(userId);
                var car = await _carRepository.GetByIdAsync(createOrderViewModel.CarId);

                if (customer is not null && car is not null)
                {
                    car.RentalStatus = CarRentalStatusEntity.CreateFromType(RentalCarStatus.PendingPickup);
                    var order = new CarOrderEntity(customer);
                    order.CarBookings.Add(
                        new CarBookingEntity(order, car, pickupDateUTC: createOrderViewModel.PickupDateLocalTime.Date,
                            returnDateUTC: createOrderViewModel.ReturnDateLocalTime.Date));

                    await _orderRepository.AddAsync(order);
                    TempDataHelper.Set(TempData, IsNewOrderTempDataKey, true);
                    return RedirectToAction(nameof(Details), new { id = order.CarOrderId });
                }

                throw new Exception($"Failed to retrieve car and/or customer from the database. - CarID: {createOrderViewModel.CarId} - UserId: {User.FindFirst(x => x.Type == ApplicationUserClaims.UserId)!.Value}");
            }

            throw new Exception($"Failed to retrieve the pending order from temp storage.");
        }

        // GET: CustomerOrderController
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if (!await IsCustomerLoggedIn())
            {
                return RedirectToLogin(nameof(Details), id);
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
                    OrderViewModel viewModel = new OrderViewModel(order, _imageUploadService, isNewOrder: orderWasCreated);
                    SaveRedirectBackInstructionsForCancelOrderAction(nameof(Details), id);

                    if (TempDataHelper.TryGet(TempData, CanceledOrderIdTempDataKey, out int canceledOrderId))
                    {
                        viewModel.Messages.Add(UserMesssageHelper.CreateOrderCancellationSuccessMessage(canceledOrderId));
                    }

                    return View(viewModel);
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
                return RedirectToLogin(nameof(List));
            }

            var userId = User.FindFirst(x => x.Type == ApplicationUserClaims.UserId)!.Value;
            var customer = await _customerRepository.GetByUserIdAsync(userId) ?? throw new Exception($"Failed to find customer with user ID: {userId}");
            ListViewModel<OrderViewModel> orderListViewModel = new ListViewModel<OrderViewModel>(
                customer.Orders
                    .Select(x => new OrderViewModel(x, _imageUploadService))
                    .OrderByDescending(x => x.CarOrderId));

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
                    return RedirectToLogin(nameof(Confirm));
                }

                return RedirectToAction(nameof(Confirm));
            }

            throw new Exception($"Failed to prepare order for the car with id: {createOrderViewModel.CarId} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        #endregion

        #region OtherMethods

        /// <summary>
        /// Redirects to the login page and request a redirect back afterwards. 
        /// </summary>
        /// <param name="action">The controller action to redirect to.</param>
        /// <param name="orderId">An optional order ID.</param>
        /// <returns><see cref="IActionResult"/>.</returns>
        private IActionResult RedirectToLogin(string action, int? orderId = null)
        {
            RouteValueDictionary? routeValues = orderId is not null ? new RouteValueDictionary(new { id = orderId }) : null;

            TempDataHelper.Set(TempData, CustomerController.RedirectInstructionsTempDataKey, new RedirectToActionData(
                    action, ControllerHelper.GetControllerName<CustomerOrderController>(), routeValues: routeValues));

            return RedirectToAction(nameof(CustomerController.Authenticate), ControllerHelper.GetControllerName<CustomerController>());
        }

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

        #endregion
    }
}
