using AutoMapper;
using Blazored.SessionStorage;
using FribergCarRentals.Shared.Models.Dto.Customer;
using FribergCarRentals.Shared.Models.ViewModels.Customer;
using FribergCarRentalsBlazor.Services.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;

namespace FribergCarRentalsBlazor.Components
{
    /// <summary>
    /// A component that handles both login and logout functionality for customers.
    /// </summary>
    public partial class CustomerLoginButton : ComponentBase
    {
        #region Constants

        /// <summary>
        /// The key where the component looks for redirect url targets after logging in. 
        /// </summary>
        public const string RedirectUrlStorageKey = "RedirectUrlAfterLoginKey";

        #endregion

        #region Fields

        /// <summary>
        /// A collection of validation errors returned from the API.
        /// </summary>
        private List<string> _apiValidationErrors = new List<string>();

        /// <summary>
        /// The id of the modal dialog. 
        /// </summary>
        private readonly string _modalDialogId = Guid.NewGuid().ToString();

        #endregion

        #region Properties

        /// <summary>
        /// The injected Auto Mapper service. 
        /// </summary>
        [Inject]
        private IMapper AutoMapper { get; set; } = default!;

        /// <summary>
        /// HTML classes to use for the login button
        /// </summary>
        [Parameter]
        public string LoginButtonCssClasses { get; set; } = "";

        /// <summary>
        /// The injected customer authentication service. 
        /// </summary>
        [Inject]
        private ICustomerAuthenticationService CustomerAuthenticationService { get; set; } = default!;

        /// <summary>
        /// The data binding property for the login form. 
        /// </summary>
        [SupplyParameterFromForm]
        public LoginCustomerViewModel FormInput { get; set; } = new();

        /// <summary>
        /// The injected JavaScript runtime. 
        /// </summary>
        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

        /// <summary>
        /// The injected navigation manager. 
        /// </summary>
        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        /// <summary>
        /// The injected session storage service.
        /// </summary>
        [Inject]
        private ISessionStorageService SessionStorageService { get; set; } = default!;

        #endregion

        #region Methods

        /// <summary>
        /// Event handler for when the logout button was clicked. 
        /// </summary>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        private async Task OnLogoutButtonClicked()
        {
            await CustomerAuthenticationService.Logout();
            NavigationManager.NavigateToLogout("/");
        }

        /// <summary>
        /// Event handler for the form submit button. 
        /// </summary>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        private async Task OnValidSubmit()
        {
            _apiValidationErrors.Clear();
            await JSRuntime.InvokeVoidAsync("HideCustomerLoginModal", _modalDialogId);
            var response = await CustomerAuthenticationService.Login(AutoMapper.Map<LoginCustomerDto>(FormInput));

            if (response.Success)
            {
                FormInput = new LoginCustomerViewModel();
                // TODO - Replace with customer back office url?
                string navigateToUrl = "/";

                if (await SessionStorageService.ContainKeyAsync(RedirectUrlStorageKey))
                {
                    string redirectToUrl = await SessionStorageService.GetItemAsStringAsync(RedirectUrlStorageKey);
                    await SessionStorageService.RemoveItemAsync(RedirectUrlStorageKey);
                    navigateToUrl = redirectToUrl;
                }

                NavigationManager.NavigateTo(navigateToUrl);
            }
            else
            {
                _apiValidationErrors = response.Errors.Select(x => x.Value).ToList();

                // The framework needs some time it seems
                await Task.Delay(200);
                await JSRuntime.InvokeVoidAsync("ShowCustomerLoginModal", _modalDialogId);
            }
        }

        #endregion
    }
}
