using FribergCarRentals.Shared.Models.Dto.Api;
using FribergCarRentals.Shared.Models.ViewModels.Customer;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.AdminApi;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FribergCarRentalsBlazor.Components.Admin
{
    /// <summary>
    /// A component that shows a delete customer button and handles the deletion process.
    /// </summary>
    public partial class DeleteCustomerButton : ComponentBase
    {
        #region Properties

        /// <summary>
        /// The injected admin customer API service.
        /// </summary>
        [Inject]
        private IAdminCustomerApiService AdminCustomerApiService { get; set; } = default!;

        /// <summary>
        /// The customer to delete.
        /// </summary>
        [Parameter]
        public CustomerViewModel Customer { get; set; } = default!;

        /// <summary>
        /// The injected JavaScript runtime. 
        /// </summary>
        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

        /// <summary>
        /// Event callback for when a customer has been deleted. 
        /// </summary>
        [Parameter]
        public EventCallback<DeleteCustomerEventCallbackArgs> OnDeletedCustomer { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a customer.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents an async operation.</returns>
        private async Task DeleteCustomer()
        {
            if (await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to delete this customer?"))
            {
                var result = await AdminCustomerApiService.DeleteCustomerAsync(Customer.CustomerId);
                await OnDeletedCustomer.InvokeAsync(new DeleteCustomerEventCallbackArgs(Customer, result));
            }
        }

        #endregion
    }

    /// <summary>
    /// Event callback storage class that contains the result of a delete customer operation.
    /// </summary>
    public class DeleteCustomerEventCallbackArgs
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="customer">The customer that was deleted.</param>
        /// <param name="result">The API response for the operation.</param>
        public DeleteCustomerEventCallbackArgs(CustomerViewModel customer, ApiResponseDto result)
        {
            Customer = customer;
            ApiResponse = result;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The customer that was deleted.
        /// </summary>
        public CustomerViewModel Customer {  get; }
            
        /// <summary>
        /// The API response for the operation.
        /// </summary>
        public ApiResponseDto ApiResponse { get; }

        #endregion
    }
}
