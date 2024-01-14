using FribergCarRentals.DataAccess.Repositories;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.Controllers
{
    [Route("Admin/Orders/[action]")]
    public class AdminCarOrderController : Controller
    {
        #region Fields

        private readonly ICarOrderRepository _carOrderRepository;

        #endregion

        #region Constructors

        public AdminCarOrderController(ICarOrderRepository carOrderRepository)
        {
            _carOrderRepository = carOrderRepository;
        }

        #endregion

        // GET: AdminCarOrder
        public async Task<ActionResult> List()
        {
            return View((await _carOrderRepository.GetAll()).Select(x => new CarOrderViewModel(x)).ToList());
        }

        // GET: AdminCarOrder/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // POST: AdminCarOrder/Create
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

        // GET: AdminCarOrder/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var order = await _carOrderRepository.GetById(id);

            if (order is not null)
            {
                return View(new CarOrderViewModel(order));
            }
            else
            {
                return RedirectToAction(nameof(List));
            }
        }

        // POST: AdminCarOrder/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int carOrderId, IFormCollection collection)
        {
            try
            {
                if (ModelState.IsValid && carOrderId > 0)
                {
                    await _carOrderRepository.Delete(carOrderId);
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
