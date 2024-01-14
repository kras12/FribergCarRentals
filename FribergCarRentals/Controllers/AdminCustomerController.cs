using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.Controllers
{
    public class AdminCustomerController : Controller
    {
        // GET: AdminCustomerController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AdminCustomerController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminCustomerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminCustomerController/Create
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

        // GET: AdminCustomerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminCustomerController/Edit/5
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

        // GET: AdminCustomerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminCustomerController/Delete/5
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
