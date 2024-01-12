using FribergCarRentals.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult> Index()
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminCarController/Edit/5
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
