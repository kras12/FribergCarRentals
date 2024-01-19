using FribergCarRentals.DataAccess.Repositories;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.Controllers
{
    [Route($"{CurrentControllerRoutePart}/[action]")]
    public class AdminCarOrderController : ViewControllerBase
    {
        #region Constants

        private const string CurrentControllerRoutePart = "Admin/Orders";

        #endregion

        #region Fields

        private readonly ICarOrderRepository _carOrderRepository;

        #endregion

        #region Constructors

        public AdminCarOrderController(ICarOrderRepository carOrderRepository)
        {
            _carOrderRepository = carOrderRepository;
        }

        #endregion

        #region Actions

        // GET: AdminCarOrder/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
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

            return NotFound();
        }

        // POST: AdminCarOrder/Delete/5
        [ActionName(nameof(Delete))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeletePost(int carOrderId)
        {
            if (ModelState.IsValid && carOrderId > 0)
            {
                await _carOrderRepository.DeleteOrder(carOrderId);
                return RedirectToAction(nameof(List));
            }

            return View();
        }

        // GET: AdminCarOrder/Details/5
        public async Task<ActionResult> Details(int id)
        {
            if (ModelState.IsValid)
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

            return NotFound();
        }

        // GET: AdminCarOrder
        public async Task<ActionResult> List()
        {
            return View((await _carOrderRepository.GetAll()).Select(x => new CarOrderViewModel(x)).ToList());
        }
        #endregion
    }
}
