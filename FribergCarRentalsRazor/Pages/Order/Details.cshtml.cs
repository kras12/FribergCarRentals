using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.Data;
using FribergCarRentals.Pages.Customer;
using MvcRazorPages.Shared.Helpers;
using MvcRazorPages.Shared.ViewModels.Order;
using FribergFastigheter.Server.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MvcRazorPages.Shared.Services;

namespace FribergCarRentals.Pages.Order
{
    /// <summary>
    /// Page model for showing order details in the customer back office. 
    /// </summary>
    public class DetailsModel : CustomerPageModelBase
    {
        #region Fields

        /// <summary>
        /// The injected order repository.
        /// </summary>
        private readonly ICarOrderRepository _orderRepository;

        /// <summary>
        /// The injected image upload service.
        /// </summary>
        private readonly IImageUploadService _imageUploadService;

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor. 
        /// </summary>
        /// <param name="orderRepository">Injected order repository.</param>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="signInManager">The injected signin manager.</param>
        /// <param name="imageUploadService">The injected image upload service.</param>
        public DetailsModel(ICarOrderRepository orderRepository, IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager, IImageUploadService imageUploadService) : base(authorizationService, signInManager)
        {
            _orderRepository = orderRepository;
            _imageUploadService = imageUploadService;
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
            if (!await IsCustomerLoggedIn())
            {
                return RedirectToLogin(new RedirectToPageData(
                        "../Order/Details",
                        new RouteValueDictionary(new { id = id })));
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
                    OrderViewModel = new OrderViewModel(order, _imageUploadService, isNewOrder: orderWasCreated);

                    TempDataHelper.Set(TempData, CancelModel.CanceledOrderRedirectToPageTempDataKey, new RedirectToPageData(
                        "Details",
                        new RouteValueDictionary(new { id = id })));

                    if (TempDataHelper.TryGet(TempData, CancelModel.CanceledOrderIdTempDataKey, out int canceledOrderId))
                    {
                        OrderViewModel.Messages.Add(UserMesssageHelper.CreateOrderCancellationSuccessMessage(canceledOrderId));
                    }
                    
                    return Page();
                }

                throw new Exception($"Failed to retrieve the order from the database. - OrderID: {id}");
            }

            throw new Exception($"Model validation failed: - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        #endregion
    }
}
