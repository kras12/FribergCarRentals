using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FribergCarRentals.DataAccess.Repositories;
using MvcRazorPages.Shared.Data;
using FribergCarRentals.Sessions;
using MvcRazorPages.Shared.Helpers;
using FribergCarRentals.Models.Orders;

namespace FribergCarRentals.Pages.Admin.Order
{
    /// <summary>
    /// Page model class for presenting order details in the admin back office. 
    /// </summary>
    public class DetailsModel : PageModel
    {
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
        public DetailsModel(ICarOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The view model used for presenting order details in the admin back office. 
        /// </summary>
        public OrderViewModel OrderViewModel { get; set; }

        #endregion

        #region HandlerMethods

        /// <summary>
        /// Handler for GET requests.
        /// </summary>
        /// <param name="id">The ID of ther order to view.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(id);
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
                    OrderViewModel = new OrderViewModel(order);
                    SaveRedirectToPageDataRelativeToCompleteOrderPage(id);

                    if (TempDataHelper.TryGet(TempData, CompleteModel.CompletedOrderIdTempDataKey, out int completedOrderId))
                    {
                        OrderViewModel.Messages.Add(UserMesssageHelper.CreateOrderCompletionSuccessMessage(completedOrderId));
                    }
                    
                    return Page();
                }
            }

            throw new Exception($"Failed to show the order with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        #endregion

        #region OtherMethods

        /// <summary>
        /// Redirects to the login page and request a redirect back afterwards. 
        /// </summary>
        /// <param name="id">The ID of the order to view.</param>
        /// <returns>A <see cref="IActionResult"/>.</returns>
        private IActionResult RedirectToLogin(int id)
        {
            TempDataHelper.Set(TempData, CompleteModel.RedirectToPageAfterOrderCompletionTempDataKey, new RedirectToPageData(
                        "Order/Details",
                        new RouteValueDictionary(new { id = id })));

            return RedirectToPage("../Login");
        }

        /// <summary>
        /// Saves redirect data for this page relative to the complete order page.
        /// </summary>
        /// <param name="id">The ID of the order to view.</param>
        private void SaveRedirectToPageDataRelativeToCompleteOrderPage(int id)
        {
            TempDataHelper.Set(TempData, CompleteModel.RedirectToPageAfterOrderCompletionTempDataKey, new RedirectToPageData(
                        "Details",
                        new RouteValueDictionary(new { id = id})));
        }

        #endregion
    }
}
