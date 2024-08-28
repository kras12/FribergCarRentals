using AutoMapper;
using FribergCarRentals.Shared.Models.ViewModels.Car;
using FribergCarRentals.Shared.Models.ViewModels.Message;
using FribergCarRentals.Shared.Models.ViewModels.Order;
using FribergCarRentalsBlazor.Components.Admin;
using FribergCarRentalsBlazor.Pages.Admin.Car;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.AdminApi;
using Microsoft.AspNetCore.Components;

namespace FribergCarRentalsBlazor.Pages.Admin.Order
{
    /// <summary>
    /// The page component class for showing order details.
    /// </summary>
    public partial class OrderDetails : AdminPageComponentBase
    {
        #region Constants

        /// <summary>
        /// The base URL for the page without the order ID.
        /// </summary>
        private const string PageUrlBase = "/admin/order/details";

		/// <summary>
		/// The url template for the page. 
		/// </summary>
		private const string PageUrlTemplate = PageUrlBase + "/{OrderId:int}";

        #endregion

        #region Fields

        /// <summary>
        /// A collection of validation errors returned from the API.
        /// </summary>
        private List<MessageViewModel> _apiValidationErrors = new();

        #endregion

        #region Properties

        /// <summary>
        /// The ID of the order. 
        /// </summary>
        [Parameter]
        public int OrderId { get; set; }

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
        /// The viewmodel for the order to show.
        /// </summary>
        private OrderViewModel? Order { get; set; } = null;

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
                Order!.Messages.Add(MessageViewModelHelper.CreateOrderCompletionSuccessMessage(result.Order.CarOrderId));
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
                NavigationManager.NavigateTo(ListOrders.GetPageUrl());
            }
            else
            {
                _apiValidationErrors = result.ApiResponse.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList();
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
        protected override async Task OnInitializedAsync()
        {
            var result = await AdminOrderApiService.GetOrderByIdAsync(OrderId);

            if (result.Success)
            {
                Order = AutoMapper.Map<OrderViewModel>(result.Value!);
            }
            else
            {
                _apiValidationErrors = result.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList();
            }
        }

        #endregion
    }
}
