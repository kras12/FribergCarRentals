using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.DataAccess.Repositories;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;

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

        // GET: CarBookingController
        //public ActionResult Index()
        //{
        //    return View();
        //}

        // GET: CarBookingController/Cancel/5
        public async Task<ActionResult> Cancel(int id)
        {
            var carBooking = await _carOrderRepository.GetCarBookingById(id);

            if (carBooking is not null)
            {
                return View(new CarBookingViewModel(carBooking));
            }

            return NotFound();
        }

        // POST: CarBookingController/Cancel/(5)
        [ActionName(nameof(Cancel))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CancelPost(int carBookingId)
        {
            var result = await _carOrderRepository.CancelCarBookingOrOrder(carBookingId);

            if (result == CancelCarBookingResult.BookingCanceled || result == CancelCarBookingResult.BookingAndOrderCanceled)
            {
                return RedirectToAction(nameof(ListFutureBookings));
            }


            return NotFound();
        }

        // POST: CarBookingController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CustomerCarOrderFormInputViewModel orderData)
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
        public async Task<ActionResult> ListFutureBookings()
        {
            return View((await _carOrderRepository.GetFutureCarBookings()).Select(x => new CarBookingViewModel(x)).ToList());
        }

        // GET: CarBookingController
        public async Task<ActionResult> ListPastBookings()
        {
            return View((await _carOrderRepository.GetPastCarBookings()).Select(x => new CarBookingViewModel(x)).ToList());
        }

        // POST: CarBookingController/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> New(int carId, IFormCollection collection)
        {
            bool isLoggedIn = true;

            if (ModelState.IsValid)
            {
                if (isLoggedIn)
                {
                    // TODO - Include the car id in the http post to here
                    var car = await _carRepository.GetById(carId) ?? throw new Exception("The car was not found");
                    var randomCustomer = (await _customerRepository.GetAll()).First();
                    return View(new CustomerCarOrderFormInputViewModel(randomCustomer.UserId, car, DateTime.Now, DateTime.Now.AddDays(1)));
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
