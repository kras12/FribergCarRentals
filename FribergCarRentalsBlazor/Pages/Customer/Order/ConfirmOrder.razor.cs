using AutoMapper;
using FribergCarRentals.Shared.Models.Dto.Order;
using FribergCarRentals.Shared.Models.ViewModels.Message;
using FribergCarRentals.Shared.Models.ViewModels.Order;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.CustomerApi;
using Microsoft.AspNetCore.Components;

namespace FribergCarRentalsBlazor.Pages.Customer.Order
{
    /// <summary>
    /// The page component for confirming a car order.
    /// </summary>
    public partial class ConfirmOrder : CustomerPageComponentBase
    {
        #region Constants

        /// <summary>
        /// The key for the created order flag stored in temp storage.
        /// </summary>
        public const string IsNewOrderTempDataKey = "CustomerOrderIsNewOrder";

		/// <summary>
		/// The url template for the page. 
		/// </summary>
		private const string PageUrlTemplate = "/customer/order/confirm";

        #endregion

        #region Fields

        /// <summary>
        /// A collection of validation errors returned from the API.
        /// </summary>
        private List<MessageViewModel> _apiValidationErrors = new();

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
                await RedirectToLogin(BookCar.GetPageUrl());
            }

            var result = await CustomerOrderApiService.CreateOrderAsync(AutoMapper.Map<CreateCarOrderDto>(_createOrderViewModel));
            
            if (result.Success)
            {
                await SessionStorageService.RemoveItemAsync(BookCar.PendingOrderTempDataKey);
                await SessionStorageService.SetItemAsync(IsNewOrderTempDataKey, true);
                NavigationManager.NavigateTo(OrderDetails.GetPageUrl(result.Value!.CarOrderId));
            }
            else
            {
                _apiValidationErrors = result.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList();
            }
        }

		/// <summary>
		/// Gets the page URL.
		/// </summary>
		/// <returns>A <see cref="string"/> that contains the URL of the page.</returns>
		public static string GetPageUrl()
		{
			return PageUrlTemplate;
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
                await RedirectToLogin(BookCar.GetPageUrl());
            }

            _createOrderViewModel = await SessionStorageService.GetItemAsync<CreateOrderViewModel>(BookCar.PendingOrderTempDataKey);

            if (_createOrderViewModel == null)
            {
                throw new Exception($"Failed to retrieve the pending order from storage.");
            }
        }

        #endregion
    }
}
