using FribergCarRentalsBlazor.Services.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FribergCarRentalsBlazor.Components
{
    /// <summary>
    /// A component that handles both login and logout functionality for customers.
    /// </summary>
    public partial class LoginCustomerButton : ComponentBase
    {
        #region Fields

        /// <summary>
        /// The id of the modal dialog. 
        /// </summary>
        private readonly string _modalDialogId = Guid.NewGuid().ToString();

        #endregion

        #region Properties

        /// <summary>
        /// HTML classes to use for the login button
        /// </summary>
        [Parameter]
        public string LoginButtonCssClasses { get; set; } = "";

        /// <summary>
        /// HTML classes to use for the logout button
        /// </summary>
        [Parameter]
        public string LogoutButtonCssClasses { get; set; } = "";

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

        /// <summary>
        /// The injected JavaScript runtime. 
        /// </summary>
        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

        #endregion

        #region Methods

        /// <summary>
        /// An event handler for the before login event in the <see cref="LoginCustomer"/> component.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        private async Task OnBeforeLogin()
        {
            await JSRuntime.InvokeVoidAsync("HideCustomerLoginModal", _modalDialogId);
        }

        /// <summary>
        /// Event handler for the login failed event in the <see cref="LoginCustomer"/> component.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        private async Task OnLoginFailed()
        {
            // Frameworks needs a bit time.
            await Task.Delay(200);
            await JSRuntime.InvokeVoidAsync("ShowCustomerLoginModal", _modalDialogId);
        }

        /// <summary>
        /// Event handler for when the logout button was clicked. 
        /// </summary>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        private async Task OnLogoutButtonClicked()
        {
            await CustomerAuthenticationService.Logout();
            NavigationManager.NavigateTo(Pages.Home.PageUrl);
        }        

        #endregion
    }
}
