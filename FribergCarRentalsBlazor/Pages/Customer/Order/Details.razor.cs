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
    public partial class Details : CustomerPageComponentBase
    {
        #region Constants

        /// <summary>
        /// The base url for the page. 
        /// </summary>
        public const string PageBaseUrl = "/customer/order/details";

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
        /// The order ID to show details for.
        /// </summary>
        [Parameter]
        public int Id { get; set; }

        /// <summary>
        /// The injected JavaScript runtime. 
        /// </summary>
        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

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
        /// <inheritdoc/>
        /// </summary>
        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (Id < 0)
            {
                throw new Exception($"Invalid ID: {Id}");
            }

            var result = await CustomerOrderApiService.GetOrderAsync(Id);

            if (result.Success)
            {
                _order = AutoMapper.Map<OrderViewModel>(result.Value!);
                
                if (await SessionStorageService.ContainKeyAsync(Confirm.IsNewOrderTempDataKey))
                {
                    _order.IsNewOrder = await SessionStorageService.GetItemAsync<bool>(Confirm.IsNewOrderTempDataKey);
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
