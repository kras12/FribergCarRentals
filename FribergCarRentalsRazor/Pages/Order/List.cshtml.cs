using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.Data;
using FribergCarRentals.Pages.Customer;
using MvcRazorPages.Shared.Helpers;
using MvcRazorPages.Shared.ViewModels.Other;
using MvcRazorPages.Shared.ViewModels.Order;
using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FribergFastigheter.Shared.Constants;
using MvcRazorPages.Shared.Services;

namespace FribergCarRentals.Pages.Order
{
    /// <summary>
    /// Page model for listing orders in the customer back office.
    /// </summary>
    public class ListModel : CustomerPageModelBase
    {
        #region Fields

        /// <summary>
        /// The injected customer repository.
        /// </summary>
        private readonly ICustomerRepository _customerRepository;

        /// <summary>
        /// The injected image upload service.
        /// </summary>
        private readonly IImageUploadService _imageUploadService;

        #endregion

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="signInManager">The injected signin manager.</param>
        /// <param name="customerRepository">The injected customer repository.</param>
        /// <param name="imageUploadService">The injected image upload service.</param>
        public ListModel(IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager, ICustomerRepository customerRepository, IImageUploadService imageUploadService) : base(authorizationService, signInManager)
        {
            _customerRepository = customerRepository;
            _imageUploadService = imageUploadService;
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
                return RedirectToLogin(new RedirectToPageData("../Order/List"));
            }

            var userId = User.FindFirst(x => x.Type == ApplicationUserClaims.UserId)!.Value;
            var customer = await _customerRepository.GetByUserIdAsync(userId) ?? throw new Exception($"Failed to find customer with user ID: {userId}");
            OrderListViewModel = new ListViewModel<OrderViewModel>(
                customer.Orders
                    .Select(x => new OrderViewModel(x, _imageUploadService))
                    .OrderByDescending(x => x.CarOrderId));            

            if (TempDataHelper.TryGet(TempData, CancelModel.CanceledOrderIdTempDataKey, out int canceledOrderId))
            {
                OrderListViewModel.Messages.Add(UserMesssageHelper.CreateOrderCancellationSuccessMessage(canceledOrderId));
            }

            TempDataHelper.Set(TempData, CancelModel.CanceledOrderRedirectToPageTempDataKey, new RedirectToPageData(
                    "List"));

            return Page();
        }


        #endregion
    }
}
