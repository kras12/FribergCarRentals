using FribergCarRentals.Helpers;
using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.DataAccess.Repositories;
using FribergCarRentals.Sessions;
using FribergCarRentals.Models.Orders;
using FribergCarRentalsRazor.Helpers;
using FribergCarRentals.Models.Other;
using MvcRazorPages.Shared.Data;

namespace FribergCarRentals.Controllers.Admin
{
    [Route($"{CurrentControllerRoutePart}/[action]")]
    public class AdminOrderController : ViewControllerBase
    {
        #region Constants

        /// <summary>
        /// The key for the ID of the order that was completed. 
        /// </summary>
        public const string CompletedOrderIdTempDataKey = "AdminCompletedOrderId";

        /// <summary>
        /// The route part for the controller.
        /// </summary>
        private const string CurrentControllerRoutePart = "Admin/Orders";

        /// <summary>
        /// The key for the ID of the order that was deleted. 
        /// </summary>
        public const string DeletedOrderIdTempDataKey = "AdminDeletedOrderId";

        /// <summary>
        /// The key for the redirect data containing the page to redirect to after deleting an order.
        /// </summary>
        public const string RedirectToPageAfterOrderDeletionTempDataKey = "AdminDeletedOrderRedirectToPage";

        /// <summary>
        /// The key for the redirect data containing the page to redirect to after completing an order.
        /// </summary>
        public const string RedirectToPageAfterOrderCompletionTempDataKey = "AdminCompletedOrderRedirectToPage";

        #endregion

        #region Fields

        private readonly ICarOrderRepository _orderRepository;

        #endregion

        #region Constructors

        public AdminOrderController(ICarOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        #endregion

        #region Actions

        // POST: AdminOrder/Complete/5
        [ActionName(nameof(Complete))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(int id)
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Complete));
            }

            if (id < 0)
            {
                throw new Exception($"Invalid ID: {id}");
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (await _orderRepository.TryCompleteOrderAsync(id))
                {
                    TempDataHelper.Set(TempData, CompletedOrderIdTempDataKey, id);

                    if (TempDataHelper.TryGet(TempData, RedirectToPageAfterOrderCompletionTempDataKey, out RedirectToActionData? data))
                    {
                        return RedirectToAction(data.Action, data.Controller, data.RouteValues);
                    }
                    else
                    {
                        return RedirectToAction(nameof(Details), new { id = id });
                    }
                }
            }

            throw new Exception($"Failed to complete the order with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        // POST: AdminOrder/Delete/5
        [ActionName(nameof(Delete))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Delete), id);
            }

            if (id < 0)
            {
                throw new Exception($"Invalid ID: {id}");
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                await _orderRepository.DeleteOrderAsync(id);
                TempDataHelper.Set(TempData, DeletedOrderIdTempDataKey, id);

                if (TempDataHelper.TryGet(TempData, RedirectToPageAfterOrderDeletionTempDataKey, out RedirectToActionData? data))
                {
                    return RedirectToAction(data.Action, data.Controller, data.RouteValues);
                }
                else
                {
                    return RedirectToAction(nameof(List));
                }
            }

            throw new Exception($"Failed to delete the order with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid} - IsAdminLoggedIn: {UserSessionHandler.IsAdminLoggedIn(HttpContext.Session)}");
        }

        // GET: AdminOrder/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Details), id);
            }

            if (id < 0)
            {
                throw new Exception($"Invalid ID: {id}");
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var order = await _orderRepository.GetByIdAsync(id);

                if (order is not null)
                {
                    OrderViewModel viewModel = new OrderViewModel(order);
                    SaveRedirectBackInstructionsForCompleteOrderAction(nameof(Details), id);

                    if (TempDataHelper.TryGet(TempData, CompletedOrderIdTempDataKey, out int completedOrderId))
                    {
                        viewModel.Messages.Add(UserMesssageHelper.CreateOrderCompletionSuccessMessage(completedOrderId));
                    }

                    return View(viewModel);
                }
            }

            throw new Exception($"Failed to show the order with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        // GET: AdminOrder
        public async Task<IActionResult> List()
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(List));
            }

            ListViewModel<OrderViewModel> viewModel = new ((await _orderRepository.GetAllAsync()).Select(x => new OrderViewModel(x)).OrderByDescending(x => x.CarOrderId));
            SaveRedirectBackInstructionsForCompleteOrderAction(nameof(List));
            SaveRedirectBackInstructionsForDeleteOrderAction(nameof(List));

            if (TempDataHelper.TryGet(TempData, CompletedOrderIdTempDataKey, out int completedOrderId))
            {
                viewModel.Messages.Add(UserMesssageHelper.CreateOrderCompletionSuccessMessage(completedOrderId));
            }

            if (TempDataHelper.TryGet(TempData, DeletedOrderIdTempDataKey, out int deletedOrderId))
            {
                viewModel.Messages.Add(UserMesssageHelper.CreateOrderDeletionSuccessMessage(deletedOrderId));
            }

            return View(viewModel);
        }

        #endregion

        #region OtherMethods

        /// <summary>
        /// Redirects to the login page and request a redirect back afterwards. 
        /// </summary>
        /// <param name="action">The action to redirect to.</param>
        /// <param name="id">An optional ID for the order.</param>
        /// <returns><see cref="IActionResult"/>.</returns>
        private IActionResult RedirectToLogin(string action, int? id = null)
        {
            RouteValueDictionary? routeValues = id is not null ? new RouteValueDictionary(new { id = id }) : null;

            TempDataHelper.Set(TempData, AdminController.RedirectToPageTempDataKey, new RedirectToActionData(
                    action, ControllerHelper.GetControllerName<AdminOrderController>(), routeValues: routeValues));

            return RedirectToAction(nameof(AdminController.Login), ControllerHelper.GetControllerName<AdminController>());
        }

        /// <summary>
        /// Saves data for redirecting back to an action after an order has been completed.
        /// </summary>
        /// <param name="redirectToAction">The action to redirect to.</param>
        /// <param name="id">An optional ID for the order.</param>
        private void SaveRedirectBackInstructionsForCompleteOrderAction(string redirectToAction, int? id = null)
        {
            RouteValueDictionary? routeValues = id is not null ? new RouteValueDictionary(new { id = id }) : null;
            TempDataHelper.Set(TempData, RedirectToPageAfterOrderCompletionTempDataKey, new RedirectToActionData(
                    redirectToAction, ControllerHelper.GetControllerName<AdminOrderController>(), routeValues: routeValues));
        }

        /// <summary>
        /// Saves data for redirecting back to an action after an order has been deleted. 
        /// </summary>
        /// <param name="redirectToAction">The action to redirect to.</param>
        private void SaveRedirectBackInstructionsForDeleteOrderAction(string redirectToAction)
        {
            TempDataHelper.Set(TempData, RedirectToPageAfterOrderDeletionTempDataKey, new RedirectToActionData(
                    redirectToAction, ControllerHelper.GetControllerName<AdminOrderController>()));
        }

        #endregion
    }
}
