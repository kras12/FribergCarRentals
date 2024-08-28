using AutoMapper;
using FribergCarRentals.Shared.Models.ViewModels.Customer;
using FribergCarRentals.Shared.Models.ViewModels.Message;
using FribergCarRentals.Shared.Models.ViewModels.Other;
using FribergCarRentalsBlazor.Components.Admin;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.AdminApi;
using Microsoft.AspNetCore.Components;

namespace FribergCarRentalsBlazor.Pages.Admin.Customer
{
    /// <summary>
    /// The page component class for listing customers.
    /// </summary>
    public partial class ListCustomers : AdminPageComponentBase
    {
        #region Constants

        // TODO - Find a better solution for storing this ID, as this page does not have the primary responsibility for creating a customer. 
        /// <summary>
        /// The key for the ID of the customer that was created. 
        /// </summary>
        public const string CreatedCustomerIdStorageDataKey = "AdminCreatedCustomerId";

        // TODO - Find a better solution for storing this ID, as this page does not have the primary responsibility for deleting a customer. 
        /// <summary>
        /// The key for the ID of the customer that was deleted. 
        /// </summary>
        public const string DeletedCustomerIdStorageDataKey = "AdminDeletedCustomerId";

		/// <summary>
		/// The url template for the page. 
		/// </summary>
		private const string PageUrlTemplate = "/admin/customer/list";

        #endregion

        #region Fields

        /// <summary>
        /// A collection of validation errors returned from the API.
        /// </summary>
        private List<MessageViewModel> _apiValidationErrors = new();

        /// <summary>
        /// The customers to list on the page. 
        /// </summary>
        private ListViewModel<CustomerViewModel> _customers = default!;

        #endregion

        #region Properties

        /// <summary>
        /// The injected Auto Mapper service. 
        /// </summary>
        [Inject]
        private IMapper AutoMapper { get; set; } = default!;

        /// <summary>
        /// The injected admin customer API service.
        /// </summary>
        [Inject]
        private IAdminCustomerApiService AdminCustomerApiService { get; set; } = default!;

		#endregion

		#region Methods

		/// <summary>
		/// Gets the page URL.
		/// </summary>
		/// <returns>A <see cref="string"/> that contains the URL of the page.</returns>
		public static string GetPageUrl()
		{
			return PageUrlTemplate;
		}

		/// <summary>
		/// Event callback for a delete customer operation.
		/// </summary>
		/// <param name="result">The result of the operation</param>
		/// <returns>A <see cref="Task"/> that represents an async operation.</returns>
		private void OnDeleteCustomer(DeleteCustomerEventCallbackArgs result)
        {
            if (result.ApiResponse.Success)
            {
                _customers.Messages.Add(MessageViewModelHelper.CreateCustomerDeletionSuccessMessage(result.Customer.CustomerId));
                _customers.Models.Remove(result.Customer);
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
            var result = await AdminCustomerApiService.GetCustomersAsync();

            if (result.Success)
            {
                _customers = new ListViewModel<CustomerViewModel>(AutoMapper.Map<List<CustomerViewModel>>(result.Value!));

                if (await SessionStorageService.ContainKeyAsync(CreatedCustomerIdStorageDataKey))
                {
                    var customerId = await SessionStorageService.GetItemAsync<int>(CreatedCustomerIdStorageDataKey);
                    await SessionStorageService.RemoveItemAsync(CreatedCustomerIdStorageDataKey);
                    _customers.Messages.Add(MessageViewModelHelper.CreateCustomerCreationSuccessMessage(customerId));
                }

                if (await SessionStorageService.ContainKeyAsync(DeletedCustomerIdStorageDataKey))
                {
                    var customerId = await SessionStorageService.GetItemAsync<int>(DeletedCustomerIdStorageDataKey);
                    await SessionStorageService.RemoveItemAsync(DeletedCustomerIdStorageDataKey);
                    _customers.Messages.Add(MessageViewModelHelper.CreateCustomerDeletionSuccessMessage(customerId));
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
