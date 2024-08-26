using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.Repositories;
using FribergCarRentals.Shared.Mvc.Data;
using FribergCarRentals.Shared.Mvc.Helpers;
using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace FribergCarRentals.Areas.Admin.Pages.Order
{
    /// <summary>
    /// Page model for completing a customer order in the admin back office. 
    /// </summary>
    public class CompleteModel : AdminPageModelBase
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
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="signInManager">The injected signin manager.</param>
        public CompleteModel(ICarOrderRepository orderRepository, IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager) : base(authorizationService, signInManager) 
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
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(new RedirectToPageData("Order/List", area: Area));
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
    }
}
