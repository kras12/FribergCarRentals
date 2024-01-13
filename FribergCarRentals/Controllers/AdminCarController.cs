using FribergCarRentals.DataAccess.Repositories;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;

namespace FribergCarRentals.Controllers
{
    public class AdminCarController : Controller
    {

        private readonly ICarRepository _carRepository;

        public AdminCarController(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        // GET: AdminCarController
        public async Task<ActionResult> List()
        {
            return View(await _carRepository.GetAll());
        }

        // GET: AdminCarController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminCarController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminCarController/Create
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

        // GET: AdminCarController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var car = await _carRepository.GetById(id);

            if (car is not null)
            {
                return View(car);
            }
            else
            {
                return RedirectToAction(nameof(List));
            }            
        }

        // POST: AdminCarController/Edit/5
        [ActionName(nameof(Edit))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPost(int carId)
        {
            try
            {
                if (ModelState.IsValid && carId > 0)
                {
                    var car = new CarEntity();

                    if (car is not null && await TryUpdateModelAsync(car))
                    {
                        await _carRepository.Update(car);
                        return RedirectToAction(nameof(List));
                    }                    
                }
            }
            catch (Exception)
            {

            }

            return View();
        }

        // GET: AdminCarController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminCarController/Delete/5
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
    }
}
