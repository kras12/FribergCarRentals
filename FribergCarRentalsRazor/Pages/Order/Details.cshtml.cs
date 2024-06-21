using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FribergCarRentals.DataAccess.Repositories;
using MvcRazorPages.Shared.Data;
using FribergCarRentals.Helpers;
using FribergCarRentals.Sessions;
using FribergCarRentals.Pages.Customer;
using FribergCarRentalsRazor.Helpers;
using FribergCarRentals.Models.Orders;

namespace FribergCarRentals.Pages.Order
{
    /// <summary>
    /// Page model for showing order details in the customer back office. 
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
        /// The view model to use when presenting order details. 
        /// </summary>
        public OrderViewModel OrderViewModel { get; set; } 

        #endregion

        #region HandlerMethods

        /// <summary>
        /// Handler for GET requests.
        /// </summary>
        /// <param name="id">The ID for the order to view.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing an <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (!UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
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
                    TempDataHelper.TryGet(TempData, BookModel.IsNewOrderTempDataKey, out bool orderWasCreated);
                    OrderViewModel = new OrderViewModel(order, isNewOrder: orderWasCreated);
                    SaveRedirectToPageDataRelativeToCancelOrderPage(id);

                    if (TempDataHelper.TryGet(TempData, CancelModel.CanceledOrderIdTempDataKey, out int canceledOrderId))
                    {
                        OrderViewModel.Messages.Add(UserMesssageHelper.CreateOrderCancellationSuccessMessage(canceledOrderId));
                    }
                    
                    return Page();
                }

                throw new Exception($"Failed to retrieve the order from the database. - OrderID: {id} - CustomerID: {UserSessionHandler.GetUserData(HttpContext.Session).UserId}");
            }

            throw new Exception($"Model validation failed: - CustomerID: {UserSessionHandler.GetUserData(HttpContext.Session).UserId} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
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
            TempDataHelper.Set(TempData, AuthenticateModel.RedirectInstructionsTempDataKey, new RedirectToPageData(
                        "../Order/Details",
                        new RouteValueDictionary(new { id = id })));

            return RedirectToPage("/Customer/Authenticate");
        }

        /// <summary>
        /// Saves redirect data for this page relative to the cancel order page.
        /// </summary>
        /// <param name="id">The ID of the order to view.</param>
        private void SaveRedirectToPageDataRelativeToCancelOrderPage(int id)
        {
            TempDataHelper.Set(TempData, CancelModel.CanceledOrderRedirectToPageTempDataKey, new RedirectToPageData(
                        "Details",
                        new RouteValueDictionary(new { id = id })));
        }

        #endregion
    }
}
