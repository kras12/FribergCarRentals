using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;

namespace FribergCarRentalsBlazor.Pages.Customer
{
    /// <summary>
    /// Page component base class for customers.
    /// </summary>
    public abstract class CustomerPageComponentBase : PageComponentBase
    {
        #region Properties

        /// <summary>s
		/// The injected navigation manager.
		/// </summary>
		[Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        /// <summary>
        /// The injected session storage service.
        /// </summary>
        [Inject]
        protected ISessionStorageService SessionStorageService { get; set; } = default!;

        #endregion

        #region Methods

        /// <summary>
        /// Redirects to the login page and request a redirect back afterwards. 
        /// </summary>
        /// <remarks>Area is handled by this controller class.</remarks>
        /// <param name="redirectBackToUrl">An optional url to redirect back to afterwards.</param>
        /// <returns><see cref="IActionResult"/>.</returns>
        protected async Task RedirectToLogin(string? redirectBackToUrl = null)
        {
            if (!string.IsNullOrEmpty(redirectBackToUrl))
            {
                await SessionStorageService.SetItemAsStringAsync(Authenticate.RedirectUrlStorageKey, redirectBackToUrl);
            }

            NavigationManager.NavigateTo("/customer/login");
        }

        #endregion
    }
}
