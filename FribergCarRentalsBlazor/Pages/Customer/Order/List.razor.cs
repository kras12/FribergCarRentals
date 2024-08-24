using AutoMapper;
using FribergCarRentals.Shared.Models.ViewModels.Message;
using FribergCarRentals.Shared.Models.ViewModels.Order;
using FribergCarRentals.Shared.Models.ViewModels.Other;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.CustomerApi;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FribergCarRentalsBlazor.Pages.Customer.Order
{
    /// <summary>
    /// The page component class for listing customer orders.
    /// </summary>
    public partial class List : CustomerPageComponentBase
    {
        #region Constants

        /// <summary>
        /// The url for the page. 
        /// </summary>
        public const string PageUrl = "/customer/order/list";

        #endregion

        #region Fields

        /// <summary>
        /// A collection of validation errors returned from the API.
        /// </summary>
        private List<MessageViewModel> _apiValidationErrors = new();

        /// <summary>
        /// The orders to list on the page. 
        /// </summary>
        private ListViewModel<OrderViewModel> _orders = default!;

        #endregion

        #region Properties

        /// <summary>
        /// The injected Auto Mapper service. 
        /// </summary>
        [Inject]
        private IMapper AutoMapper { get; set; } = default!;

        /// <summary>
        /// The injected customer order API service.
        /// </summary>
        [Inject]
        private ICustomerOrderApiService CustomerOrderApiService { get; set; } = default!;

        /// <summary>
        /// The injected JavaScript runtime. 
        /// </summary>
        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

        #endregion

        #region Methods

        /// <summary>
        /// Cancels an order.
        /// </summary>
        /// <param name="order">The order to cancel.</param>
        /// <returns>A <see cref="Task"/> that represents an async operation.</returns>
        private async Task CancelOrder(OrderViewModel order)
        {
            if (await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to cancel this order?"))
            {
                var result = await CustomerOrderApiService.CancelOrderAsync(order.CarOrderId);

                if (result.Success)
                {
                    AutoMapper.Map(result.Value!, order);
                    _orders.Messages.Add(MessageViewModelHelper.CreateOrderCancellationSuccessMessage(order.CarOrderId));
                }
                else
                {
                    _apiValidationErrors = result.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList();
                }
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var result = await CustomerOrderApiService.GetOrdersAsync();

            if (result.Success)
            {
                _orders = new ListViewModel<OrderViewModel>(AutoMapper.Map<List<OrderViewModel>>(result.Value!));
            }
            else
            {
                _apiValidationErrors = result.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList();
            }
        }

        #endregion
    }
}
