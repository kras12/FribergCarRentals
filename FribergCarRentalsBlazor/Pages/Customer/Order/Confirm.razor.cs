using AutoMapper;
using FribergCarRentals.Shared.Models.Dto.Order;
using FribergCarRentals.Shared.Models.ViewModels.Order;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.CustomerApi;
using Microsoft.AspNetCore.Components;

namespace FribergCarRentalsBlazor.Pages.Customer.Order
{
    /// <summary>
    /// The page component for confirming a car order.
    /// </summary>
    public partial class Confirm : CustomerPageComponentBase
    {
        #region Constants

        /// <summary>
        /// The key for the created order flag stored in temp storage.
        /// </summary>
        public const string IsNewOrderTempDataKey = "CustomerOrderIsNewOrder";

        #endregion

        #region Fields

        /// <summary>
        /// A collection of validation errors returned from the API.
        /// </summary>
        private List<string> _apiValidationErrors = new List<string>();

        /// <summary>
        /// The view model used to bind order creation data. 
        /// </summary>
        private CreateOrderViewModel? _createOrderViewModel = null;

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

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new car order.
        /// </summary>
        /// <returns>A <see cref="Task"/> representating an async operation.</returns>
        private async Task CreateOrder()
        {
            if (!await IsCustomerLoggedIn())
            {
                await RedirectToLogin("/customer/order/booking");
            }

            var result = await CustomerOrderApiService.CreateOrderAsync(AutoMapper.Map<CreateCarOrderDto>(_createOrderViewModel));
            
            if (result.Success)
            {
                await SessionStorageService.RemoveItemAsync(Book.PendingOrderTempDataKey);
                await SessionStorageService.SetItemAsync(IsNewOrderTempDataKey, true);
                NavigationManager.NavigateTo($"{Details.PageBaseUrl}/{result.Value!.CarOrderId}");
            }
            else
            {
                _apiValidationErrors = result.Errors.Select(x => x.Value).ToList();
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <exception cref="Exception"></exception>
        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (!await IsCustomerLoggedIn())
            {
                await RedirectToLogin("/customer/order/booking");
            }

            _createOrderViewModel = await SessionStorageService.GetItemAsync<CreateOrderViewModel>(Book.PendingOrderTempDataKey);

            if (_createOrderViewModel == null)
            {
                throw new Exception($"Failed to retrieve the pending order from storage.");
            }
        }

        #endregion
    }
}
