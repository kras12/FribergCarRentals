using AutoMapper;
using FribergCarRentals.Shared.Models.ViewModels.Message;
using FribergCarRentals.Shared.Models.ViewModels.Order;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.CustomerApi;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FribergCarRentalsBlazor.Pages.Customer.Order
{
    /// <summary>
    /// Page component for showing order details. 
    /// </summary>
    public partial class OrderDetails : CustomerPageComponentBase
    {
        #region Constants

        /// <summary>
        /// The base URL for the page without the order ID.
        /// </summary>
        private const string PageUrlBase = "/customer/order/details";

        /// <summary>
        /// The url for the page. 
        /// </summary>
        private const string PageUrlTemplate = PageUrlBase + "/{OrderId:int}";

        #endregion

        #region Fields

        /// <summary>
        /// A collection of validation errors returned from the API.
        /// </summary>
        private List<MessageViewModel> _apiValidationErrors = new();

        /// <summary>
        /// The model for the car order.
        /// </summary>
        private OrderViewModel _order = default!;

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

        /// <summary>
        /// The order ID to show details for.
        /// </summary>
        [Parameter]
        public int OrderId { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Cancels the order.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents an async operation.</returns>
        private async Task CancelOrder()
        {
            if (await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to cancel this order?"))
            {
                var result = await CustomerOrderApiService.CancelOrderAsync(_order.CarOrderId);

                if (result.Success)
                {
                    _order = AutoMapper.Map<OrderViewModel>(result.Value!);
                    _order.Messages.Add(MessageViewModelHelper.CreateOrderCancellationSuccessMessage(_order.CarOrderId));
                }
                else
                {
                    _apiValidationErrors = result.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList();
                }
            }
        }

        /// <summary>
        /// Gets the page URL for an order.
        /// </summary>
        /// <param name="orderId">The ID of the order.</param>
        /// <returns>A <see cref="string"/> that contains the URL of the page.</returns>
        public static string GetPageUrl(int orderId)
        {
            return $"{PageUrlBase}/{orderId}";
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (OrderId < 0)
            {
                throw new Exception($"Invalid ID: {OrderId}");
            }

            var result = await CustomerOrderApiService.GetOrderAsync(OrderId);

            if (result.Success)
            {
                _order = AutoMapper.Map<OrderViewModel>(result.Value!);
                
                if (await SessionStorageService.ContainKeyAsync(ConfirmOrder.IsNewOrderTempDataKey))
                {
                    _order.IsNewOrder = await SessionStorageService.GetItemAsync<bool>(ConfirmOrder.IsNewOrderTempDataKey);
                }
            }
            else
            {
                _apiValidationErrors = result.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList();
            }            
        }

        #endregion
    }
}
