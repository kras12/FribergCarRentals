using AutoMapper;
using FribergCarRentals.Shared.Models.ViewModels.Customer;
using FribergCarRentals.Shared.Models.ViewModels.Message;
using FribergCarRentalsBlazor.Components.Admin;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.AdminApi;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FribergCarRentalsBlazor.Pages.Admin.Customer
{
    /// <summary>
    /// The page component class for showing customer details.
    /// </summary>
    public partial class Details : AdminPageComponentBase
    {
        #region Constants

        /// <summary>
        /// The base URL for the page without the customer ID.
        /// </summary>
        public const string PageUrlBase = "/admin/customer/details";

        /// <summary>
        /// The url for the page. 
        /// </summary>
        public const string PageUrl = PageUrlBase + "/{CustomerId:int}";

        #endregion

        #region Fields

        /// <summary>
        /// A collection of validation errors returned from the API.
        /// </summary>
        private List<MessageViewModel> _apiValidationErrors = new();

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
        /// The view model for customers.
        /// </summary>
        private CustomerViewModel? Customer { get; set; } = null;

        /// <summary>
        /// The injected JavaScript runtime. 
        /// </summary>
        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

        #endregion

        #region Methods

        /// <summary>
        /// Event callback for a delete customer operation.
        /// </summary>
        /// <param name="result">The result of the operation</param>
        /// <returns>A <see cref="Task"/> that represents an async operation.</returns>
        private async Task OnDeleteCustomer(DeleteCustomerEventCallbackArgs result)
        {
            if (result.ApiResponse.Success)
            {
                await SessionStorageService.SetItemAsStringAsync(List.DeletedCustomerIdStorageDataKey, Customer!.CustomerId.ToString());
                NavigationManager.NavigateTo(List.PageUrl);
            }
            else
            {
                _apiValidationErrors = result.ApiResponse.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList();
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
        /// <inheritdoc/>
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var result = await AdminCustomerApiService.GetCustomerByIdAsync(CustomerId);

            if (result.Success)
            {
                Customer = AutoMapper.Map<CustomerViewModel>(result.Value!);

                if (await SessionStorageService.ContainKeyAsync(Create.CreatedCustomerIdTempDataKey))
                {
                    int? createdCustomerId = await SessionStorageService.GetItemAsync<int>(Create.CreatedCustomerIdTempDataKey);
                    Customer.Messages.Add(MessageViewModelHelper.CreateCustomerCreationSuccessMessage(createdCustomerId.Value));
                }
            }
            else
            {
                _apiValidationErrors = result.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList();
            }
        }

        /// <summary>
        /// Resends the confirm email link to the customer.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents an async operation.</returns>
        private async Task ResendConfirmEmailLink()
        {
            if (await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to send a new confirm email link?"))
            {
                // Since we haven't implemented code for sending emails to the customer, 
                // we will manually confirm the emails ourselves for now. 

                var getCodeResult = await AdminCustomerApiService.GetConfirmEmailCodeAsync(CustomerId);

                if (getCodeResult.Success)
                {
                    var confirmEmailResult = await AdminCustomerApiService.ConfirmEmailAsync(getCodeResult.Value!);

                    if (confirmEmailResult.Success)
                    {
                        Customer!.Messages.Add(MessageViewModelHelper.CreateResentConfirmEmailLinkToCustomerSuccessMessage(CustomerId));
                        return;
                    }
                }

                Customer!.Messages.Add(MessageViewModelHelper.CreateResentConfirmEmailLinkToCustomerFailureMessage(CustomerId));
                throw new Exception($"Failed to confirm email for user with ID: {CustomerId}");
            }
        }

        #endregion
    }
}
