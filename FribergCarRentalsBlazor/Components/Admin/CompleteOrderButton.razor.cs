using AutoMapper;
using FribergCarRentals.Shared.Models.Dto.Api;
using FribergCarRentals.Shared.Models.ViewModels.Order;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.AdminApi;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FribergCarRentalsBlazor.Components.Admin
{
    /// <summary>
    /// A component that shows a complete order button and handles the deletion process.
    /// </summary>
    public partial class CompleteOrderButton : ComponentBase
    {
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
        /// The order to complete. 
        /// </summary>
        [Parameter]
        public OrderViewModel Order { get; set; } = new();

        /// <summary>
        /// The injected JavaScript runtime. 
        /// </summary>
        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

        /// <summary>
        /// Event callback for when an order has been completed. 
        /// </summary>
        [Parameter]
        public EventCallback<CompleteOrderEventCallbackArgs> OnCompletedOrder { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Complates an order.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents an async operation.</returns>
        private async Task CompleteOrder()
        {
            if (await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to complete this order?"))
            {
                var result = await AdminOrderApiService.CompleteOrderAsync(Order.CarOrderId);
                AutoMapper.Map(result.Value, Order);
                await OnCompletedOrder.InvokeAsync(new CompleteOrderEventCallbackArgs(Order, result));
            }
        }

        #endregion
    }

    /// <summary>
    /// Event callback storage class that contains the result of a complete order operation.
    /// </summary>
    public class CompleteOrderEventCallbackArgs
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="order">The order that was completed.</param>
        /// <param name="result">The API response for the operation.</param>
        public CompleteOrderEventCallbackArgs(OrderViewModel order, ApiResponseDto result)
        {
            Order = order;
            ApiResponse = result;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The order that was completed.
        /// </summary>
        public OrderViewModel Order { get; }
            
        /// <summary>
        /// The API response for the operation.
        /// </summary>
        public ApiResponseDto ApiResponse { get; }

        #endregion
    }
}
