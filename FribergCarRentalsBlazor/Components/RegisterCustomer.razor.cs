using AutoMapper;
using FribergCarRentals.Shared.Models.Dto.Customer;
using FribergCarRentals.Shared.Models.ViewModels.Customer;
using FribergCarRentalsBlazor.Services.Authentication;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi;
using Microsoft.AspNetCore.Components;

namespace FribergCarRentalsBlazor.Components
{
    /// <summary>
    /// A component that shows a register form and handles the customer creation
    /// </summary>
    public partial class RegisterCustomer : ComponentBase
    {
        #region Fields

        /// <summary>
        /// A collection of validation errors returned from the API.
        /// </summary>
        private List<string> _apiValidationErrors = new List<string>();

        #endregion

        #region Events

        /// <summary>
        /// An event that triggers when the user failed to login.
        /// </summary>
        [Parameter]
        public EventCallback OnLoginFailed { get; set; }

        /// <summary>
        /// An event that triggers when the user logged in successfully. 
        /// </summary>
        [Parameter]
        public EventCallback OnLoginSuccessful { get; set; }

        /// <summary>
        /// An event that triggers when the customer registration failed.
        /// </summary>
        [Parameter]
        public EventCallback OnRegistrationFailed { get; set; }

        /// <summary>
        /// An event that triggers when the customer registration was successful.
        /// </summary>
        [Parameter]
        public EventCallback OnRegistrationSuccessful { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// The data binding property for the login form. 
        /// </summary>
        [SupplyParameterFromForm]
        public RegisterCustomerViewModel FormInput { get; set; } = new();

        /// <summary>
        /// True if the user should be logged in after a successful registration.
        /// </summary>
        [Parameter]
        public bool LoginAfterRegistration { get; set; }

        /// <summary>
        /// An optional URL to redirect the user to after a successful login.
        /// </summary>
        [Parameter]
        public string? RedirectUrl { get; set; } = null;

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
        /// The injected customer authentication service. 
        /// </summary>
        [Inject]
        private ICustomerAuthenticationService CustomerAuthenticationService { get; set; } = default!;

        /// <summary>
        /// The injected navigation manager. 
        /// </summary>
        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        #endregion

        #region Methods

        /// <summary>
        /// Logins in the new customer.
        /// </summary>
        /// <param name="createdCustomerDto">The new customer to login.</param>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        private async Task Login(CreatedCustomerDto createdCustomerDto)
        {
            _apiValidationErrors.Clear();
            var response = await CustomerAuthenticationService.LoginCustomer(AutoMapper.Map<LoginCustomerDto>(createdCustomerDto));

            if (response.Success)
            {
                await OnLoginSuccessful.InvokeAsync();
            }
            else
            {
                _apiValidationErrors = response.Errors.Select(x => x.Value).ToList();
                await OnLoginFailed.InvokeAsync();
            }
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
                await OnRegistrationSuccessful.InvokeAsync();

                if (LoginAfterRegistration)
                {
                    await Login(response.Value!);
                }

                if (!string.IsNullOrEmpty(RedirectUrl))
                {
                    NavigationManager.NavigateTo(RedirectUrl);
                }

                // TODO - Check how to update the application state if not redirected.
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
