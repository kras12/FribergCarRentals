using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.Data;
using MvcRazorPages.Shared.Helpers;
using FribergFastigheter.Server.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FribergFastigheter.Shared.Constants;
using FribergCarRentals.Pages.Customer;

namespace FribergCarRentals.Pages.Order
{
    /// <summary>
    /// Page model for cancelling an order in the customer back office. 
    /// </summary>
    public class CancelModel : PageModelBase
    {
        #region Constants

        /// <summary>
        /// The key for the ID of the order that was canceled.
        /// </summary>
        public const string CanceledOrderIdTempDataKey = "CustomerCanceledOrderId";

        /// <summary>
        /// The key for the canceled order redirect data stored in temp storage.
        /// </summary>
        public const string CanceledOrderRedirectToPageTempDataKey = "CustomerCanceledOrderRedirectToPage";

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
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="signInManager">The injected signin manager.</param>
        public CancelModel(ICarOrderRepository orderRepository, IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager) : base(authorizationService, signInManager)
        {
            _orderRepository = orderRepository;
        }

        #endregion

        #region HandlerMethods        

        /// <summary>
        /// Handler for POST requests.
        /// </summary>
        /// <param name="id">The ID of the order to cancel.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing an <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!await IsCustomerLoggedIn())
            {
                return RedirectToLogin(id);
            }

            if (id < 0)
            {
                throw new Exception($"Invalid ID: {id}");
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (await _orderRepository.TryCancelOrderAsync(id))
                {
                    TempDataHelper.Set(TempData, CanceledOrderIdTempDataKey, id);

                    if (TempDataHelper.TryGet(TempData, CanceledOrderRedirectToPageTempDataKey, out RedirectToPageData? data))
                    {
                        return RedirectToPage(data.Page, data.RouteValues);
                    }
                    else
                    {
                        return RedirectToPage("Details", new { id = id });
                    }
                }

                throw new Exception($"Failed to cancel order with id: {id}");
            }

            throw new Exception($"Model validation failed: UserId: {User.FindFirst(x => x.Type == ApplicationUserClaims.UserId)!.Value} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        #endregion

        #region OtherMethods

        /// <summary>
        /// Redirects to the login page and request a redirect to the order details page.
        /// </summary>
        /// <param name="id">The ID of the order to cancel.</param>
        /// <returns>An <see cref="IActionResult"/>.</returns>
        private IActionResult RedirectToLogin(int id)
        {
            TempDataHelper.Set(TempData, AuthenticateModel.RedirectInstructionsTempDataKey, new RedirectToPageData(
                        "Order/Details",
                        new RouteValueDictionary(new { id = id })));

            return RedirectToPage("../Login");
        }

        #endregion
    }
}
