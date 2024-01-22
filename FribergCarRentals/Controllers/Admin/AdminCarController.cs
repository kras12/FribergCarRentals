using FribergCarRentals.Data;
using FribergCarRentals.Helpers;
using FribergCarRentals.Data.Car;
using FribergCarRentals.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.DataAccess.Repositories;

namespace FribergCarRentals.Controllers.Admin
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


        // GET: AdminCarController/Create
        public ActionResult Create()
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                TempDataHelper.Set(TempData, AdminController.RedirectToActionTempDataKey, new LoginRedirectActionWithId(
                    nameof(Create),
                    ControllerHelper.GetControllerName<AdminCarController>()));

                return RedirectToAction(nameof(AdminController.Login), ControllerHelper.GetControllerName<AdminController>());

            }

            return View();
        }

        // POST: AdminCarController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CarViewModel carViewModel)
        {
            if (ModelState.Count > 0 && ModelState.IsValid && UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                if (DataTransferHelper.TryTransferData(carViewModel, out CarEntity car))
                {
                    await _carRepository.Add(car);
                    return RedirectToAction(nameof(List));
                }
            }

            return View();
        }

        // POST: AdminCarController/Delete/5
        [ActionName(nameof(Delete))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            if (ModelState.Count > 0 && ModelState.IsValid && id > 0 && UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                if (await _carRepository.Delete(id))
                {
                    return RedirectToAction(nameof(List));
                }                
            }

            return StatusCode(500);
        }

        // GET: AdminCarController/Details/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Details(int id)
        {
            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
                {
                    TempDataHelper.Set(TempData, AdminController.RedirectToActionTempDataKey, new LoginRedirectActionWithId(
                        nameof(Details),
                        ControllerHelper.GetControllerName<AdminCarController>(),
                        new RouteValueDictionary(new { id = id })));

                    return RedirectToAction(nameof(AdminController.Login), ControllerHelper.GetControllerName<AdminController>());
                }

                var car = await _carRepository.GetById(id);

                if (car is not null)
                {
                    return View(new CarViewModel(car) { IsRequestFromAnotherController = IsRequestFromAnotherController(CurrentControllerRoutePart) });
                }
            }

            return StatusCode(500);
        }

        // GET: AdminCarController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
                {
                    TempDataHelper.Set(TempData, AdminController.RedirectToActionTempDataKey, new LoginRedirectActionWithId(
                        nameof(Edit),
                        ControllerHelper.GetControllerName<AdminCarController>(),
                        new RouteValueDictionary(new { id = id })));

                    return RedirectToAction(nameof(AdminController.Login), ControllerHelper.GetControllerName<AdminController>());
                }

                var car = await _carRepository.GetById(id);

                if (car is not null)
                {
                    return View(new CarViewModel(car));
                }
            }

            return StatusCode(500);
        }

        // POST: AdminCarController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int carId, CarViewModel carViewModel)
        {
            if (ModelState.IsValid && carId > 0 && carId == carViewModel.CarId && UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                if (DataTransferHelper.TryTransferData(carViewModel, out CarEntity car))
                {
                    await _carRepository.Update(car);
                    return RedirectToAction(nameof(List));
                }

            }
            return View();
        }

        // GET: AdminCarController/List
        public async Task<ActionResult> List()
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                TempDataHelper.Set(TempData, AdminController.RedirectToActionTempDataKey, new LoginRedirectActionWithId(
                    nameof(List),
                    ControllerHelper.GetControllerName<AdminCarController>()));

                return RedirectToAction(nameof(AdminController.Login), ControllerHelper.GetControllerName<AdminController>());
            }

            return View((await _carRepository.GetAll()).Select(x => new CarViewModel(x)));
        }
        #endregion
    }
}
