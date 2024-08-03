using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.Helpers;
using MvcRazorPages.Shared.Data;
using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace FribergCarRentals.Areas.Admin.Pages.Order
{
    /// <summary>
    /// Page model for deleting a customer order in the admin back office. 
    /// </summary>
    public class DeleteModel : AdminPageModelBase
    {
        #region Constants

        /// <summary>
        /// The key for the ID of the order that was deleted. 
        /// </summary>
        public const string DeletedOrderIdTempDataKey = "AdminDeletedOrderId";

        /// <summary>
        /// The key for the redirect data containing the page to redirect to after deleting an order.
        /// </summary>
        public const string RedirectToPageAfterDeleteTempDataKey = "AdminDeletedOrderRedirectToPage";

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
        public DeleteModel(ICarOrderRepository orderRepository, IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager) : base(authorizationService, signInManager) 
        {
            _orderRepository = orderRepository;
        }

        #endregion

        #region HandlerMethods

        /// <summary>
        /// Handler for POST requests. 
        /// </summary>
        /// <param name="id">The ID of the order to delete.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(new RedirectToPageData("Order/Details", new RouteValueDictionary(new { id = id }), area: Area));
            }

            if (id < 0)
            {
                throw new Exception($"Invalid ID: {id}");
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                await _orderRepository.DeleteOrderAsync(id);
                TempDataHelper.Set(TempData, DeletedOrderIdTempDataKey, id);

                if (TempDataHelper.TryGet(TempData, RedirectToPageAfterDeleteTempDataKey, out RedirectToPageData? data))
                {
                    return RedirectToPage(data.Page, data.RouteValues);
                }
                else
                {
                    return RedirectToPage("List");
                }
            }

            throw new Exception($"Failed to delete the order with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        #endregion
    }
}
