using AutoMapper;
using FribergCarRentals.Shared.Models.Dto.Customer;
using FribergCarRentals.Shared.Models.ViewModels.Customer;
using FribergCarRentals.Shared.Models.ViewModels.Message;
using FribergCarRentalsBlazor.Components.Admin;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.AdminApi;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentalsBlazor.Pages.Admin.Customer
{
    /// <summary>
    /// The page component class for editing a customer.
    /// </summary>
    public partial class EditCustomer : AdminPageComponentBase
    {
        #region Constants

        /// <summary>
        /// The url for the page. 
        /// </summary>
        public const string PageUrl = PageUrlBase + "/{CustomerId:int}";

        /// <summary>
        /// The base URL for the page without the customer ID.
        /// </summary>
        public const string PageUrlBase = "/admin/customer/edit";
        #endregion

        #region Fields

        /// <summary>
        /// A collection of validation errors returned from the API.
        /// </summary>
        private List<MessageViewModel> _apiValidationErrors = new();

        // TODO - Find a cleaner solution to avoid conflicts and sources for future bugs. 
        /// <summary>
        /// The complete viewmodel of the customer that is being edited. 
        /// This property is used to support the delete customer button. 
        /// </summary>
        private CustomerViewModel _customer = default!;

        #endregion

        #region Properties

        /// <summary>
        /// The ID of the customer. 
        /// </summary>
        [Parameter]
        public int CustomerId { get; set; }

        /// <summary>
        /// The injected admin customer API service.
        /// </summary>
        [Inject]
        private IAdminCustomerApiService AdminCustomerApiService { get; set; } = default!;

        /// <summary>
        /// The injected Auto Mapper service. 
        /// </summary>
        [Inject]
        private IMapper AutoMapper { get; set; } = default!;

        /// <summary>
        /// The view model for editing the customer.
        /// </summary>
        [FromForm]
        private EditCustomerViewModel EditCustomerViewModel { get; set; } = default!;

        #endregion

        #region Methods

        /// <summary>
        /// Edits a customer.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents an async operation.</returns>
        private async Task OnEditCustomer()
        {
            var result = await AdminCustomerApiService.EditCustomerAsync(EditCustomerViewModel.AccountId, AutoMapper.Map<EditCustomerDto>(EditCustomerViewModel));

            if (result.Success)
            {
                _customer = AutoMapper.Map<CustomerViewModel>(result.Value);
                EditCustomerViewModel = AutoMapper.Map<EditCustomerViewModel>(result.Value);
                EditCustomerViewModel.Messages.Add(MessageViewModelHelper.CreateCustomerUpdateSuccessMessage(EditCustomerViewModel.AccountId));
            }
            else
            {
                _apiValidationErrors = result.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList();
            }
        }

        /// <summary>
        /// Gets the page URL for a customer.
        /// </summary>
        /// <param name="customerId">The ID of the customer.</param>
        /// <returns>A <see cref="string"/> that contains the URL of the page.</returns>
        public static string GetPageUrl(int customerId)
        {
            return $"{PageUrlBase}/{customerId}";
        }

        /// <summary>
        /// Event callback for a delete customer operation.
        /// </summary>
        /// <param name="result">The result of the operation</param>
        /// <returns>A <see cref="Task"/> that represents an async operation.</returns>
        private async Task OnDeletedCustomer(DeleteCustomerEventCallbackArgs result)
        {
            if (result.ApiResponse.Success)
            {
                await SessionStorageService.SetItemAsStringAsync(ListCustomers.DeletedCustomerIdStorageDataKey, EditCustomerViewModel.AccountId.ToString());
                NavigationManager.NavigateTo(ListCustomers.PageUrl);
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
            var result = await AdminCustomerApiService.GetCustomerByIdAsync(CustomerId);

            if (result.Success)
            {
                _customer = AutoMapper.Map<CustomerViewModel>(result.Value!);
                EditCustomerViewModel = AutoMapper.Map<EditCustomerViewModel>(result.Value!);
            }
            else
            {
                _apiValidationErrors = result.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList();
            }
        }

        #endregion
    }
}
