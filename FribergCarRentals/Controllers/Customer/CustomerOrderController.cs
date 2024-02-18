using FribergCarRentals.Data;
using FribergCarRentals.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using System.Linq;
using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.DataAccess.Repositories;
using FribergCarRentals.DataAccess.Types;
using FribergCarRentals.Models.Order;
using FribergCarRentals.Sessions;
using FribergCarRentals.Controllers.Admin;
using FribergCarRentalsRazor.Helpers;
using FribergCarRentals.Models.Other;

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
        /// The route part for this controller.
        /// </summary>
        private const string CurrentControllerRoutePart = "Order";

        /// <summary>
        /// The key for the created order flag stored in temp storage.
        /// </summary>
        public const string IsNewOrderTempDataKey = "CustomerOrderControllerIsNewOrder";

        /// <summary>
        /// The key for the redirect data containing the page to redirect to after cancelling an order.
        /// </summary>
        public const string RedirectToPageAfterOrderCancellationTempDataKey = "CustomerCancelOrderRedirectToPage";

        #endregion

        #region Fields

        private readonly ICarOrderRepository _orderRepository;
        private readonly ICarRepository _carRepository;
        private readonly ICustomerRepository _customerRepository;

        #endregion

        #region Constructors

        public CustomerOrderController(ICarOrderRepository orderRepository, ICustomerRepository customerRepository, ICarRepository carRepository)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _carRepository = carRepository;
        }

        #endregion

        #region Actions

        // GET: CustomerOrderController/Book
        [HttpGet("{carId}")]
        public async Task<IActionResult> Book(int carId)
        {
            if (!UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Book), carId);
            }

            if (carId < 0)
            {
                throw new Exception($"Invalid ID: {carId}");
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var car = await _carRepository.GetByIdAsync(carId, CarRentalStatusEntity.CreateFromType(RentalCarStatus.Rentable));
                var customer = await _customerRepository.GetByIdAsync(UserSessionHandler.GetUserData(HttpContext.Session).UserId);

                if (car is not null && customer is not null)
                {
                    CreateOrderViewModel viewModel = new CreateOrderViewModel(customer.UserId, car, pickupDate: DateTime.Now, returnDate: DateTime.Now.AddDays(1));
                    return View(viewModel);
                }

                throw new Exception($"Failed to retrieve car and/or customer from the database. - CarID: {carId} - CustomerID: {UserSessionHandler.GetUserData(HttpContext.Session).UserId}");
            }

            throw new Exception($"Failed to present booking form for car with id: {carId} - CustomerID: {UserSessionHandler.GetUserData(HttpContext.Session).UserId} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        // POST: CustomerOrderController/Cancel/(5)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            if (!UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
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

                throw new Exception($"Failed to cancel order with id: {id} - CustomerID: {UserSessionHandler.GetUserData(HttpContext.Session).UserId}");
            }

            throw new Exception($"Model validation failed: CustomerID: {UserSessionHandler.GetUserData(HttpContext.Session).UserId} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        // POST: CustomerOrderController/Book
        [HttpPost("{carId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(CreateOrderViewModel createOrderViewModel)
        {
            if (!UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Book));
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (createOrderViewModel.PickupDateLocalTime.Date < DateTime.Now.Date)
                {
                    ModelState.AddModelError($"{nameof(CreateOrderViewModel)}.{nameof(CreateOrderViewModel.PickupDateLocalTime)}",
                        "The pickup date can't be in the past.");
                    return View(createOrderViewModel);
                }
                else if (createOrderViewModel.ReturnDateLocalTime.Date < createOrderViewModel.PickupDateLocalTime.Date)
                {
                    ModelState.AddModelError($"{nameof(CreateOrderViewModel)}.{nameof(createOrderViewModel.ReturnDateLocalTime)}",
                        "The return date can't occur before the pickup date.");
                    return View(createOrderViewModel);
                }

                var customer = await _customerRepository.GetByIdAsync(createOrderViewModel.CustomerId);
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

                throw new Exception($"Failed to retrieve car and/or customer from the database. - CarID: {createOrderViewModel.CarId} - CustomerID: {UserSessionHandler.GetUserData(HttpContext.Session).UserId}");
            }

            throw new Exception($"Failed to create an order for the car with id: {createOrderViewModel.CarId} - CustomerID: {UserSessionHandler.GetUserData(HttpContext.Session).UserId} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        // GET: CustomerOrderController
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if (!UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
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
                    OrderViewModel viewModel = new OrderViewModel(order, isNewOrder: orderWasCreated);
                    SaveRedirectBackInstructionsForCancelOrderAction(nameof(Details), id);

                    if (TempDataHelper.TryGet(TempData, CanceledOrderIdTempDataKey, out int canceledOrderId))
                    {
                        viewModel.Messages.Add(UserMesssageHelper.CreateOrderCancellationSuccessMessage(canceledOrderId));
                    }

                    return View(viewModel);
                }

                throw new Exception($"Failed to retrieve the order from the database. - OrderID: {id} - CustomerID: {UserSessionHandler.GetUserData(HttpContext.Session).UserId}");
            }

            throw new Exception($"Model validation failed: - CustomerID: {UserSessionHandler.GetUserData(HttpContext.Session).UserId} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        // GET: CustomerOrderController
        [HttpGet]
        public async Task<IActionResult> List()
        {
            if (!UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(List));
            }

            ListViewModel<OrderViewModel> orderListViewModel = new ListViewModel<OrderViewModel>(
                (await _orderRepository.GetAllByCustomer(UserSessionHandler.GetUserData(HttpContext.Session).UserId))
                    .Select(x => new OrderViewModel(x))
                    .OrderByDescending(x => x.CarOrderId));

            if (TempDataHelper.TryGet(TempData, CanceledOrderIdTempDataKey, out int canceledOrderId))
            {
                orderListViewModel.Messages.Add(UserMesssageHelper.CreateOrderCancellationSuccessMessage(canceledOrderId));
            }

            SaveRedirectBackInstructionsForCancelOrderAction(nameof(List));
            return View(orderListViewModel);
        }

        #endregion

        #region OtherMethods

        /// <summary>
        /// Redirects to the login page and request a redirect back afterwards. 
        /// </summary>
        /// <param name="action">The action to redirect to.</param>
        /// <param name="id">An optional ID for the car.</param>
        /// <returns><see cref="IActionResult"/>.</returns>
        private IActionResult RedirectToLogin(string action, int? id = null)
        {
            RouteValueDictionary? routeValues = id is not null ? new RouteValueDictionary(new { id = id }) : null;

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
            TempDataHelper.Set(TempData, RedirectToPageAfterOrderCancellationTempDataKey, new RedirectToActionData(
                    redirectToAction, ControllerHelper.GetControllerName<CustomerOrderController>(), routeValues: routeValues));
        }

        #endregion
    }
}
