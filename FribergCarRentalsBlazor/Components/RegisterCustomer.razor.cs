using AutoMapper;
using FribergCarRentals.Shared.Models.Dto.Customer;
using FribergCarRentals.Shared.Models.Dto.User;
using FribergCarRentals.Shared.Models.ViewModels.Customer;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.CustomerApi;
using Microsoft.AspNetCore.Components;

namespace FribergCarRentalsBlazor.Components
{
    /// <summary>
    /// A component that shows a register form and handles the customer creation
    /// </summary>
    public partial class RegisterCustomer : ComponentBase
    {
        #region Enums

        /// <summary>
        /// Statuses for a registration.
        /// </summary>
        public enum RegistrationStatus
        {
            ConfirmEmail,
            Completed
        }

        #endregion

        #region Fields

        /// <summary>
        /// A collection of validation errors returned from the API.
        /// </summary>
        private List<string> _apiValidationErrors = new List<string>();

        /// <summary>
        /// Contains data to confirm the email account for a new customer.
        /// </summary>
        /// <remarks>Showing the link is needed because this demo application does not support sending emails.</remarks>
        private ConfirmEmailDto? _confirmEmailData = null;

        #endregion

        #region Events

        /// <summary>
        /// An event that triggers when the customer registration failed.
        /// </summary>
        [Parameter]
        public EventCallback OnRegistrationFailed { get; set; }

        /// <summary>
        /// An event that triggers when the customer registration was successful.
        /// </summary>
        [Parameter]
        public EventCallback<RegistrationStatus> OnRegistrationSuccessful { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// The data binding property for the login form. 
        /// </summary>
        [SupplyParameterFromForm]
        public RegisterCustomerViewModel FormInput { get; set; } = new();

        /// <summary>
        /// Returns true if the email account needs to be confirmed.
        /// </summary>
        public bool HaveConfirmEmailData
        {
            get
            {
                return _confirmEmailData != null;
            }
        }

        /// <summary>
        /// The injected Auto Mapper service. 
        /// </summary>
        [Inject]
        private IMapper AutoMapper { get; set; } = default!;

        /// <summary>
        /// The injected customer API service.
        /// </summary>
        [Inject]
        private ICustomerApiService CustomerApiService { get; set; } = default!;

        /// <summary>
        /// The injected navigation manager. 
        /// </summary>
        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        #endregion

        #region Methods

        /// <summary>
        /// Event handler for when the user clicks the confirm email link. 
        /// </summary>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        private async Task OnConfirmEmailLinkClicked()
        {
            await OnRegistrationSuccessful.InvokeAsync(RegistrationStatus.Completed);
            IReadOnlyDictionary<string, object?> parameters = new Dictionary<string, object?>()
            {
                { "code", _confirmEmailData!.Code },
                { "email", _confirmEmailData.Email }
            };

			NavigationManager.NavigateTo(NavigationManager.GetUriWithQueryParameters("/customer/confirm-email", parameters));
        }

        /// <summary>
        /// Event handler for the form submit button. 
        /// </summary>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        private async Task OnValidSubmit()
        {
            _apiValidationErrors.Clear();
            var response = await CustomerApiService.CreateCustomer(AutoMapper.Map<CreateCustomerDto>(FormInput));

            if (response.Success)
            {
                _confirmEmailData = response.Value!.ConfirmEmailData;
                await OnRegistrationSuccessful.InvokeAsync(HaveConfirmEmailData ? RegistrationStatus.ConfirmEmail : RegistrationStatus.Completed);
            }
            else
            {
                _apiValidationErrors = response.Errors.Select(x => x.Value).ToList();
                await OnRegistrationFailed.InvokeAsync();
            }
        }

        #endregion
    }
}
