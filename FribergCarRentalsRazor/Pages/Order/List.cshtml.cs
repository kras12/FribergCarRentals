using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FribergCarRentals.DataAccess.Repositories;
using MvcRazorPages.Shared.Data;
using FribergCarRentals.Sessions;
using FribergCarRentals.Pages.Customer;
using MvcRazorPages.Shared.Helpers;
using FribergCarRentals.Models.Other;
using FribergCarRentals.Models.Orders;

namespace FribergCarRentals.Pages.Order
{
    /// <summary>
    /// Page model for listing orders in the customer back office.
    /// </summary>
    public class ListModel : PageModel
    {
        #region Fields

        /// <summary>
        /// The injected order repository.
        /// </summary>
        ICarOrderRepository _orderRepository;

        #endregion

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="orderRepository">Injected order repository.</param>
        public ListModel(ICarOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        #region Properties

        /// <summary>
        /// View model used for listing customer orders. 
        /// </summary>
        public ListViewModel<OrderViewModel> OrderListViewModel { get; private set; } = new();

        #endregion

        #region HandlerMethods

        /// <summary>
        /// Handler for GET requests. 
        /// </summary>
        /// <returns><see cref="Task{TResult}"/> containing an <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnGetAsync()
        {
            if (!UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin("../Order/List");
            }

            OrderListViewModel = new ListViewModel<OrderViewModel>(
                (await _orderRepository.GetAllByCustomer(UserSessionHandler.GetUserData(HttpContext.Session).UserId))
                    .Select(x => new OrderViewModel(x))
                    .OrderByDescending(x => x.CarOrderId));            

            if (TempDataHelper.TryGet(TempData, CancelModel.CanceledOrderIdTempDataKey, out int canceledOrderId))
            {
                OrderListViewModel.Messages.Add(UserMesssageHelper.CreateOrderCancellationSuccessMessage(canceledOrderId));
            }

            SaveRedirectToPageDataRelativeToCancelOrderPage();
            return Page();
        }


        #endregion

        #region OtherMethods

        /// <summary>
        /// Redirects to the login page and request a redirect afterwards. 
        /// </summary>
        /// <param name="page">The page to redirect to.</param>
        /// <returns><see cref="IActionResult"/>.</returns>
        private IActionResult RedirectToLogin(string page)
        {
            TempDataHelper.Set(TempData, AuthenticateModel.RedirectInstructionsTempDataKey, new RedirectToPageData(page));
            return RedirectToPage("../Customer/Authenticate");
        }

        /// <summary>
        /// Saves redirect data for this page relative to the cancel order page.
        /// </summary>
        private void SaveRedirectToPageDataRelativeToCancelOrderPage()
        {
            TempDataHelper.Set(TempData, CancelModel.CanceledOrderRedirectToPageTempDataKey, new RedirectToPageData(
                    "List"));
        }

        #endregion
    }
}
