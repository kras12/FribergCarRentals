using FribergCarRentals.Data;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Data.Repositories;
using FribergCarRentals.Helpers;
using FribergCarRentals.Models;
using FribergCarRentals.Models.Order;
using FribergCarRentals.Session;
using FribergCarRentals.Data.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using System.Linq;

namespace FribergCarRentals.Controllers.Customer
{
    [Route($"{CurrentControllerRoutePart}/[action]")]
    public class CustomerOrderController : ViewControllerBase
    {
        #region Constants

        private const string CurrentControllerRoutePart = "Order";

        #endregion

        #region Fields

        private readonly ICarOrderRepository _carOrderRepository;
        private readonly ICarRepository _carRepository;
        private readonly ICustomerRepository _customerRepository;

        #endregion

        #region Constructors

        public CustomerOrderController(ICarOrderRepository carOrderRepository, ICustomerRepository customerRepository, ICarRepository carRepository)
        {
            _carOrderRepository = carOrderRepository;
            _customerRepository = customerRepository;
            _carRepository = carRepository;
        }

        #endregion

        #region Actions

        // GET: CustomerOrderController/Book
        [HttpGet("{carId}")]
        public async Task<ActionResult> Book(int carId)
        {
            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (!UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
                {
                    TempDataHelper.Set(TempData, CustomerController.RedirectToActionTempDataKey, new LoginRedirectActionWithId(
                        nameof(Book),
                        ControllerHelper.GetControllerName<CustomerOrderController>(),
                        new RouteValueDictionary(new { carId = carId })));

                    return RedirectToAction(nameof(CustomerController.RegisterOrLogin), ControllerHelper.GetControllerName<CustomerController>());
                }

                var car = await _carRepository.GetById(carId, CarRentalStatusEntity.CreateSeedObject(CarRentalStatus.Available));
                var customer = await _customerRepository.GetById(UserSessionHandler.GetUserData(HttpContext.Session).UserId);

                if (car is not null && customer is not null)
                {
                    return View(new CreateOrderViewModel(customer.UserId, car, pickupDate: DateTime.Now, returnDate: DateTime.Now.AddDays(1)));
                }
            }

            return StatusCode(500);
        }

        // POST: CustomerOrderController/Cancel/(5)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Cancel(int id)
        {
            if (ModelState.IsValid && UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                if (await _carOrderRepository.CancelOrder(id))
                {
                    return RedirectToAction(nameof(List));
                }
            }

            return StatusCode(500);
        }

        // POST: CustomerOrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateOrderViewModel orderData)
        {
            if (ModelState.IsValid && UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                var customer = await _customerRepository.GetById(orderData.CustomerId);
                var car = await _carRepository.GetById(orderData.CarId);

                if (customer is not null && car is not null)
                {
                    car.RentalStatus = CarRentalStatusEntity.CreateSeedObject(CarRentalStatus.Rented);
                    var order = new CarOrderEntity(customer);
                    order.CarBookings.Add(
                        new CarBookingEntity(order, car, pickupDate: DateTime.Parse(orderData.PickupDateString),
                            returnDate: DateTime.Parse(orderData.ReturnDateString)));

                    var finishedOrder = await _carOrderRepository.Add(order);

                    return View("OrderConfirmation", new OrderViewModel(finishedOrder));
                }
            }

            return StatusCode(500);
        }

        // GET: CustomerOrderController
        [HttpGet("{id}")]
        public async Task<ActionResult> Details(int id)
        {
            if (ModelState.IsValid)
            {
                if (!UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
                {
                    TempDataHelper.Set(TempData, CustomerController.RedirectToActionTempDataKey, new LoginRedirectActionWithId(
                        nameof(Details),
                        ControllerHelper.GetControllerName<CustomerOrderController>(),
                        new RouteValueDictionary(new { orderId = id })));

                    return RedirectToAction(nameof(CustomerController.RegisterOrLogin), ControllerHelper.GetControllerName<CustomerController>());
                }

                var order = await _carOrderRepository.GetById(id);

                if (order is not null)
                {
                    return View(new OrderViewModel(order));
                }
            }

            return StatusCode(500);
        }

        // GET: CustomerOrderController
        [HttpGet]
        public async Task<ActionResult> List()
        {
            if (!UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                TempDataHelper.Set(TempData, CustomerController.RedirectToActionTempDataKey, new LoginRedirectActionWithId(
                    nameof(List), 
                    ControllerHelper.GetControllerName<CustomerOrderController>()));

                return RedirectToAction(nameof(CustomerController.RegisterOrLogin), ControllerHelper.GetControllerName<CustomerController>());
            }

            return View((await _carOrderRepository.GetAll()).Select(x => new OrderViewModel(x)).OrderByDescending(x => x.CarOrderId).ToList());
        }

        #endregion
    }
}
