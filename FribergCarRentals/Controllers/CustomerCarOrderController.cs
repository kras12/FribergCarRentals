using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.DataAccess.Repositories;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using System.Linq;

namespace FribergCarRentals.Controllers
{
    [Route($"{CurrentControllerRoutePart}/[action]")]
    public class CustomerCarOrderController : ViewControllerBase
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

        public CustomerCarOrderController(ICarOrderRepository carOrderRepository, ICustomerRepository customerRepository, ICarRepository carRepository)
        {
            _carOrderRepository = carOrderRepository;
            _customerRepository = customerRepository;
            _carRepository = carRepository;
        }

        #endregion

        #region Actions

        // POST: CarBookingController/Cancel/(5)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Cancel(int id)
        {
            if (ModelState.IsValid && await _carOrderRepository.CancelOrder(id))
            {
                return RedirectToAction(nameof(List));
            }

            return NotFound();
        }

        // POST: CarBookingController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CustomerOrderFormInputViewModel orderData)
        {
            bool isLoggedIn = true;

            if (ModelState.IsValid)
            {
                if (isLoggedIn)
                {
                    var customer = await _customerRepository.GetById(orderData.CustomerId) ?? throw new InvalidOperationException("Could not find the customer.");
                    var car = await _carRepository.GetById(orderData.CarId) ?? throw new InvalidOperationException("Could not find the car."); ;
                    var order = new CarOrderEntity(customer);
                    order.CarBookings.Add(
                        new CarBookingEntity(order, car, pickupDate: DateTime.Parse(orderData.PickupDateString),
                            returnDate: DateTime.Parse(orderData.ReturnDateString)));

                    var finishedOrder = await _carOrderRepository.Add(order);

                    return View("OrderConfirmation", new CarOrderViewModel(finishedOrder));
                }
                else
                {
                    return RedirectToAction($"{nameof(HomeController.Cars)}", ControllerHelper.GetControllerNameUnsuffixed<HomeController>());
                }
            }

            return RedirectToAction($"{nameof(HomeController.Cars)}", ControllerHelper.GetControllerNameUnsuffixed<HomeController>());
        }

        // GET: CarBookingController
        public async Task<ActionResult> Details(int id)
        {
            if (ModelState.IsValid)
            {
                var order = await _carOrderRepository.GetById(id);

                if (order is not null)
                {
                    return View(new CarOrderViewModel(order));
                }                
            }

            return NotFound();
        }

        // GET: CarBookingController
        public async Task<ActionResult> List()
        {
            return View((await _carOrderRepository.GetAll()).Select(x => new CarOrderViewModel(x)).ToList());
        }

        // POST: CarBookingController/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> New(int id)
        {
            bool isLoggedIn = true;

            if (ModelState.IsValid)
            {
                if (isLoggedIn)
                {
                    var car = await _carRepository.GetById(id) ?? throw new Exception("The car was not found");
                    var randomCustomer = (await _customerRepository.GetAll()).First();
                    return View(new CustomerOrderFormInputViewModel(randomCustomer.UserId, car, DateTime.Now, DateTime.Now.AddDays(1)));
                }
                else
                {
                    return RedirectToAction($"{nameof(HomeController.Cars)}", ControllerHelper.GetControllerNameUnsuffixed<HomeController>());
                }
            }

            return RedirectToAction($"{nameof(HomeController.Cars)}", ControllerHelper.GetControllerNameUnsuffixed<HomeController>());
        }
        #endregion
    }
}
