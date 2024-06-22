using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.Data;
using FribergCarRentals.Pages.Customer;
using MvcRazorPages.Shared.Helpers;
using MvcRazorPages.Shared.ViewModels.Other;
using MvcRazorPages.Shared.ViewModels.Order;
using FribergFastigheter.Server.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FribergFastigheter.Shared.Constants;

namespace FribergCarRentals.Pages.Order
{
    /// <summary>
    /// Page model for listing orders in the customer back office.
    /// </summary>
    public class ListModel : PageModelBase
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
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="signInManager">The injected signin manager.</param>
        public ListModel(ICarOrderRepository orderRepository, IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager) : base(authorizationService, signInManager) 
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
            if (!await IsCustomerLoggedIn())
            {
                return RedirectToLogin("../Order/List");
            }

            var customerId = int.Parse(User.Claims.Single(x => x.Type == ApplicationUserClaims.CustomerId).Value);
            OrderListViewModel = new ListViewModel<OrderViewModel>(
                (await _orderRepository.GetAllByCustomer(customerId))
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
