using Blazored.SessionStorage;
using FribergCarRentals.Shared.Models.Dto.Api;
using FribergCarRentals.Shared.Models.ViewModels.Order;
using FribergCarRentalsBlazor.Pages.Admin.Order;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.AdminApi;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FribergCarRentalsBlazor.Components.Admin
{
    /// <summary>
    /// A component that shows a delete order button and handles the deletion process.
    /// </summary>
    public partial class DeleteOrderButton : ComponentBase
    {
        #region Constants

        /// <summary>
        /// The key for the ID of the order that was deleted. 
        /// </summary>
        public const string DeletedOrderIdStorageDataKey = "DeletedOrderId";

        #endregion

        #region Properties

        /// <summary>
        /// Event callback for when an order has been deleted. 
        /// </summary>
        [Parameter]
        public EventCallback<DeleteOrderEventCallbackArgs> OnDeletedOrder { get; set; }

        /// <summary>
        /// The order to delete. 
        /// </summary>
        [Parameter]
        public OrderViewModel Order { get; set; } = new();

        /// <summary>
        /// Set to true if the ID of the deleted order should be stored in local storage. 
        /// </summary>
        [Parameter]
        public bool StoreDeletedOrderIdInLocalStorage { get; set; }

        /// <summary>
        /// The injected admin order API service.
        /// </summary>
        [Inject]
        private IAdminOrderApiService AdminOrderApiService { get; set; } = default!;

        /// <summary>
        /// The injected JavaScript runtime. 
        /// </summary>
        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

        /// <summary>
        /// The injected session storage service.
        /// </summary>
        [Inject]
        private ISessionStorageService SessionStorageService { get; set; } = default!;

        #endregion

        #region Methods

        /// <summary>
        /// Deletes an order.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents an async operation.</returns>
        private async Task DeleteOrder()
        {
            if (await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to delete this order?"))
            {
                var result = await AdminOrderApiService.DeleteOrderAsync(Order.CarOrderId);
                await OnDeletedOrder.InvokeAsync(new DeleteOrderEventCallbackArgs(Order, result));

                if (StoreDeletedOrderIdInLocalStorage)
                {
                    await SessionStorageService.SetItemAsStringAsync(DeletedOrderIdStorageDataKey, Order.CarOrderId.ToString());
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// Event callback storage class that contains the result of a delete order operation.
    /// </summary>
    public class DeleteOrderEventCallbackArgs
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="order">The order that was deleted.</param>
        /// <param name="result">The API response for the operation.</param>
        public DeleteOrderEventCallbackArgs(OrderViewModel order, ApiResponseDto result)
        {
            Order = order;
            ApiResponse = result;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The API response for the operation.
        /// </summary>
        public ApiResponseDto ApiResponse { get; }

        /// <summary>
        /// The order that was deleted.
        /// </summary>
        public OrderViewModel Order { get; }
        #endregion
    }
}
