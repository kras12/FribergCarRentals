using FribergCarRentals.Data;
using FribergCarRentals.DataAccess.Repositories;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;

namespace FribergCarRentals.Controllers
{
    [Route($"{CurrentControllerRoutePart}/[action]")]
    public class AdminCarController : ViewControllerBase
    {
        #region Constants

        private const string CurrentControllerRoutePart = "Admin/Cars";

        #endregion

        #region Fields

        private readonly ICarRepository _carRepository;

        #endregion

        #region Constructors

        public AdminCarController(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        #endregion

        #region Actions


        // GET: AdminCarController/List
        public async Task<ActionResult> List()
        {
            return View((await _carRepository.GetAll()).Select(x => new CarViewModel(x)));
        }

        // GET: AdminCarController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var car = await _carRepository.GetById(id);

            if (car is not null)
            {
                return View(new CarViewModel(car) { IsRequestFromAnotherController = IsRequestFromAnotherController(CurrentControllerRoutePart) });
            }
            else
            {
                return RedirectToAction(nameof(List));
            }
        }

        // GET: AdminCarController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminCarController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CarViewModel carViewModel)
        {
            if (ModelState.IsValid && DataTransferHelper.TryTransferData(carViewModel, out CarEntity car))
            {
                await _carRepository.Add(car);
                return RedirectToAction(nameof(List));
            }

            return View();
        }

        // GET: AdminCarController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var car = await _carRepository.GetById(id);

            if (car is not null)
            {
                return View(new CarViewModel(car));
            }
            else
            {
                return RedirectToAction(nameof(List));
            }
        }

        // POST: AdminCarController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int carId, CarViewModel carViewModel)
        {
            if (ModelState.IsValid && carId > 0 && carId == carViewModel.CarId &&
                DataTransferHelper.TryTransferData(carViewModel, out CarEntity car))
            {
                await _carRepository.Update(car);
                return RedirectToAction(nameof(List));
            }
            return View();
        }

        // GET: AdminCarController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var car = await _carRepository.GetById(id);

            if (car is not null)
            {
                return View(new CarViewModel(car));
            }
            else
            {
                return RedirectToAction(nameof(List));
            }
        }

        // POST: AdminCarController/Delete/5
        [ActionName(nameof(Delete))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeletePost(int carId)
        {
            if (ModelState.IsValid && carId > 0)
            {
                await _carRepository.Delete(carId);
                return RedirectToAction(nameof(List));
            }

            return View();
        }

        #endregion
    }
}
