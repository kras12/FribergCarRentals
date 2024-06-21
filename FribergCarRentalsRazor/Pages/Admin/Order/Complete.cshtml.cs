using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FribergCarRentals.DataAccess.Repositories;
using MvcRazorPages.Shared.Sessions;
using MvcRazorPages.Shared.Data;
using MvcRazorPages.Shared.Helpers;

namespace FribergCarRentals.Pages.Admin.Order
{
    /// <summary>
    /// Page model for completing a customer order in the admin back office. 
    /// </summary>
    public class CompleteModel : PageModel
    {
        #region Constants

        /// <summary>
        /// The key for the ID of the order that was completed. 
        /// </summary>
        public const string CompletedOrderIdTempDataKey = "AdminCompletedOrderId";

        /// <summary>
        /// The key for the redirect data containing the page to redirect to after completing an order.
        /// </summary>
        public const string RedirectToPageAfterOrderCompletionTempDataKey = "AdminCompletedOrderRedirectToPage";

        #endregion

        #region Fields

        /// <summary>
        /// The injected order repository. 
        /// </summary>
        private readonly ICarOrderRepository _orderRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="orderRepository">Injected order repository.</param>
        public CompleteModel(ICarOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        #endregion

        #region HandlerMethods

        /// <summary>
        /// Handler for POST requests. 
        /// </summary>
        /// <param name="id">The ID for the order to complete.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin();
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

                    if (TempDataHelper.TryGet(TempData, RedirectToPageAfterOrderCompletionTempDataKey, out RedirectToPageData? data))
                    {
                        return RedirectToPage(data.Page, data.RouteValues);
                    }
                    else
                    {
                        return RedirectToPage("Details", new { id = id });
                    }                    
                }
            }

            throw new Exception($"Failed to complete the order with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        #endregion

        #region OtherMethods

        /// <summary>
        /// Redirects to the login page and request a redirect back afterwards. 
        /// </summary>
        /// <returns>An <see cref="IActionResult"/>.</returns>
        private IActionResult RedirectToLogin()
        {
            TempDataHelper.Set(TempData, LoginModel.RedirectToPageTempDataKey, new RedirectToPageData(
                    "Order/List"));

            return RedirectToPage("../Login");
        }

        #endregion
    }
}
