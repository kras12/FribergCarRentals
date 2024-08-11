using Blazored.SessionStorage;
using FribergCarRentalsBlazor.Pages.Customer;
using Microsoft.AspNetCore.Components;

namespace FribergCarRentalsBlazor.Components
{
    /// <summary>
    /// A component that redirects the user to the login page. 
    /// </summary>
    public partial class RedirectToLogin : ComponentBase
    {
        #region Properties

        /// <summary>
        /// The injected navigation manager. 
        /// </summary>
        [Inject]
        NavigationManager NavigationManager { get; set; } = default!;

        /// <summary>
        /// The injected session storage service.
        /// </summary>
        [Inject]
        private ISessionStorageService SessionStorageService { get; set; } = default!;

        /// <summary>
        /// The url to return to after login. 
        /// </summary>
        [Parameter]
        public string? ReturnToUrl { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Method invoked when the component is ready to start, having received its initial
        /// parameters from its parent in the render tree.
        /// </summary>
        protected async override Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(ReturnToUrl))
            {
                await SessionStorageService.SetItemAsStringAsync(Authenticate.RedirectUrlStorageKey, ReturnToUrl);
            }

            if (NavigationManager.Uri.Contains("/admin"))
            {
                NavigationManager.NavigateTo("/admin/login");
            }
            else if (NavigationManager.Uri.Contains("/customer"))
            {
                NavigationManager.NavigateTo("/customer/login");
            }
            else
            {
                NavigationManager.NavigateTo("/");
            }
        }

        #endregion
    }
}
