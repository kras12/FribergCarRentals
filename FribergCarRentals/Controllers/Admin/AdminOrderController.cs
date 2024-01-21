using FribergCarRentals.Data;
using FribergCarRentals.DataAccess.Repositories;
using FribergCarRentals.Helpers;
using FribergCarRentals.Models;
using FribergCarRentals.Models.Order;
using FribergCarRentals.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.Controllers.Admin
{
    [Route($"{CurrentControllerRoutePart}/[action]")]
    public class AdminOrderController : ViewControllerBase
    {
        #region Constants

        private const string CurrentControllerRoutePart = "Admin/Orders";

        #endregion

        #region Fields

        private readonly ICarOrderRepository _carOrderRepository;

        #endregion

        #region Constructors

        public AdminOrderController(ICarOrderRepository carOrderRepository)
        {
            _carOrderRepository = carOrderRepository;
        }

        #endregion

        #region Actions

        // POST: AdminOrder/Delete/5
        [ActionName(nameof(Delete))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int carOrderId)
        {
            if (ModelState.IsValid && carOrderId > 0 && UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                if (await _carOrderRepository.DeleteOrder(carOrderId))
                {
                    return RedirectToAction(nameof(List));
                }
            }

            return StatusCode(500);
        }

        // GET: AdminOrder/Details/5
        public async Task<ActionResult> Details(int id)
        {
            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
                {
                    TempDataHelper.Set(TempData, AdminController.RedirectToActionTempDataKey, new LoginRedirectActionWithId(
                        nameof(Details),
                        ControllerHelper.GetControllerName<AdminOrderController>(),
                        new RouteValueDictionary(new { id = id })));

                    return RedirectToAction(nameof(AdminController.Login), ControllerHelper.GetControllerName<AdminController>());
                }

                var order = await _carOrderRepository.GetById(id);

                if (order is not null)
                {
                    return View(new OrderViewModel(order));
                }
            }

            return StatusCode(500);
        }

        // GET: AdminOrder
        public async Task<ActionResult> List()
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                TempDataHelper.Set(TempData, AdminController.RedirectToActionTempDataKey, new LoginRedirectActionWithId(
                    nameof(List),
                    ControllerHelper.GetControllerName<AdminOrderController>()));

                return RedirectToAction(nameof(AdminController.Login), ControllerHelper.GetControllerName<AdminController>());
            }

            return View((await _carOrderRepository.GetAll()).Select(x => new OrderViewModel(x)).ToList());
        }

        #endregion
    }
}
