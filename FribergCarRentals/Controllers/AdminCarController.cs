using FribergCarRentals.DataAccess.Repositories;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;

namespace FribergCarRentals.Controllers
{
    [Route("Admin/Cars/[action]")]
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
        public async Task<ActionResult> Details(int id)
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

        // GET: AdminCarController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminCarController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var car = new CarEntity();

                    if (await TryUpdateModelAsync(car))
                    {
                        await _carRepository.Add(car);
                        return RedirectToAction(nameof(List));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
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

                    if (await TryUpdateModelAsync(car))
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
            try
            {
                if (ModelState.IsValid && carId > 0)
                {
                    await _carRepository.Delete(carId);
                    return RedirectToAction(nameof(List));
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }

            return View();
        }
    }
}
