using AutoMapper;
using FribergCarRentals.Shared.Constants;
using FribergCarRentals.Shared.Models.ViewModels.Message;
using FribergCarRentals.Shared.Models.ViewModels.Order;
using FribergCarRentals.Shared.Models.ViewModels.Other;
using FribergCarRentalsBlazor.Components.Admin;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.AdminApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FribergCarRentalsBlazor.Pages.Admin.Order
{
    /// <summary>
    /// The page component class for listing cars.
    /// </summary>
    public partial class ListOrders : AdminPageComponentBase
    {
        #region Constants

        /// <summary>
        /// The url for the page. 
        /// </summary>
        public const string PageUrl = "/admin/order/list";

        #endregion

        #region Fields

        /// <summary>
        /// A collection of validation errors returned from the API.
        /// </summary>
        private List<MessageViewModel> _apiValidationErrors = new();

        /// <summary>
        /// The cars to list on the page. 
        /// </summary>
        private ListViewModel<OrderViewModel> _orders = default!;

        #endregion

        #region Properties

        /// <summary>
        /// The injected admin order API service.
        /// </summary>
        [Inject]
        private IAdminOrderApiService AdminOrderApiService { get; set; } = default!;

        /// <summary>
        /// The injected Auto Mapper service. 
        /// </summary>
        [Inject]
        private IMapper AutoMapper { get; set; } = default!;

        /// <summary>
        /// The injected JavaScript runtime. 
        /// </summary>
        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;
        #endregion

        #region Methods

        /// <summary>
        /// Event callback for the complete order button. 
        /// </summary>
        /// <param name="result">The result of the operation</param>
        /// <returns>A <see cref="Task"/> that represents an async operation.</returns>
        private void OnCompletedOrder(CompleteOrderEventCallbackArgs result)
        {
            if (result.ApiResponse.Success)
            {
                _orders!.Messages.Add(MessageViewModelHelper.CreateOrderCompletionSuccessMessage(result.Order.CarOrderId));
            }
            else
            {
                _apiValidationErrors = result.ApiResponse.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList();
            }
        }

        /// <summary>
        /// Event callback for the delete order button. 
        /// </summary>
        /// <param name="result">The result of the operation</param>
        /// <returns>A <see cref="Task"/> that represents an async operation.</returns>
        private void OnDeletedOrder(DeleteOrderEventCallbackArgs result)
        {
            if (result.ApiResponse.Success)
            {
                _orders!.Messages.Add(MessageViewModelHelper.CreateOrderDeletionSuccessMessage(result.Order.CarOrderId));
                _orders.Models.Remove(result.Order);
            }
            else
            {
                _apiValidationErrors = result.ApiResponse.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList();
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var authorizationResult = await AuthorizationService.AuthorizeAsync((await AuthenticationStateTask).User, ApplicationUserPolicies.Admin);

            if (authorizationResult.Succeeded)
            {
                var result = await AdminOrderApiService.GetOrdersAsync();

                if (result.Success)
                {
                    _orders = new ListViewModel<OrderViewModel>(AutoMapper.Map<List<OrderViewModel>>(result.Value!));

                    if (await SessionStorageService.ContainKeyAsync(DeleteOrderButton.DeletedOrderIdStorageDataKey))
                    {
                        var orderId = await SessionStorageService.GetItemAsync<int>(DeleteOrderButton.DeletedOrderIdStorageDataKey);
                        await SessionStorageService.RemoveItemAsync(DeleteOrderButton.DeletedOrderIdStorageDataKey);
                        _orders.Messages.Add(MessageViewModelHelper.CreateOrderDeletionSuccessMessage(orderId));
                    }
                }
                else
                {
                    _apiValidationErrors = result.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList();
                }
            }
        }        

        #endregion
    }
}
