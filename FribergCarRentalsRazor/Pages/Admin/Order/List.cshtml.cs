using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FribergCarRentals.DataAccess.Repositories;
using FribergCarRentals.Data;
using FribergCarRentals.Helpers;
using FribergCarRentals.Sessions;
using FribergCarRentalsRazor.Helpers;
using FribergCarRentals.Models.Orders;
using FribergCarRentals.Models.Other;

namespace FribergCarRentals.Pages.Admin.Order
{
    /// <summary>
    /// Page model for listing customer orders in the admin back office. 
    /// </summary>
    public class ListModel : PageModel
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
        public ListModel(ICarOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The view model used for listing customer orders. 
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
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin();
            }

            OrderListViewModel = new ListViewModel<OrderViewModel>((await _orderRepository.GetAllAsync()).Select(x => new OrderViewModel(x)).OrderByDescending(x => x.CarOrderId));
            SaveRedirectToPageDataRelativeToCompleteOrderPage();
            SaveRedirectToPageDataRelativeToDeleteOrderPage();

            if (TempDataHelper.TryGet(TempData, CompleteModel.CompletedOrderIdTempDataKey, out int completedOrderId))
            {
                OrderListViewModel.Messages.Add(UserMesssageHelper.CreateOrderCompletionSuccessMessage(completedOrderId));
            }

            if (TempDataHelper.TryGet(TempData, DeleteModel.DeletedOrderIdTempDataKey, out int deletedOrderId))
            {
                OrderListViewModel.Messages.Add(UserMesssageHelper.CreateOrderDeletionSuccessMessage(deletedOrderId));
            }
            
            return Page();
        }

        #endregion

        #region OtherMethods

        /// <summary>
        /// Redirects to the login page and request a redirect back afterwards. 
        /// </summary>
        /// <returns><see cref="IActionResult"/>.</returns>
        private IActionResult RedirectToLogin()
        {
            TempDataHelper.Set(TempData, CompleteModel.RedirectToPageAfterOrderCompletionTempDataKey, new RedirectToPageData(
                    "Order/List"));

            return RedirectToPage("../login");
        }

        /// <summary>
        /// Saves redirect data for this page relative to the complete order page.
        /// </summary>
        private void SaveRedirectToPageDataRelativeToCompleteOrderPage()
        {
            TempDataHelper.Set(TempData, CompleteModel.RedirectToPageAfterOrderCompletionTempDataKey, new RedirectToPageData(
                    "List"));
        }

        /// <summary>
        /// Saves redirect data for this page relative to the delete order page.
        /// </summary>
        private void SaveRedirectToPageDataRelativeToDeleteOrderPage()
        {
            TempDataHelper.Set(TempData, DeleteModel.RedirectToPageAfterDeleteTempDataKey, new RedirectToPageData(
                    "List"));
        }

        #endregion
    }
}
