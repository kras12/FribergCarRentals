using FribergCarRentals.DataAccess.Repositories;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.Controllers
{
    [Route($"{CurrentControllerRoutePart}/[action]")]
    public class CarBookingController : ViewControllerBase
    {
        #region Constants

        private const string CurrentControllerRoutePart = "Booking";

        #endregion

        #region Fields

        private readonly ICarOrderRepository _carOrderRepository;

        #endregion

        #region Constructors

        public CarBookingController(ICarOrderRepository carOrderRepository)
        {
            _carOrderRepository = carOrderRepository;
        }

        #endregion

        #region Actions

        // GET: CarBookingController
        public ActionResult Index()
        {
            return View();
        }

        // GET: CarBookingController
        public async Task<ActionResult> ListFutureBookings()
        {
            return View((await _carOrderRepository.GetOrdersWithFutureBookings()).Select(x => new CarOrderViewModel(x)).ToList());
        }

        // GET: CarBookingController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CarBookingController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CarBookingController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CarBookingController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CarBookingController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CarBookingController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CarBookingController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        #endregion
    }
}
