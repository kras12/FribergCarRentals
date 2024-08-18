using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;

namespace FribergCarRentalsBlazor.Pages.Admin
{
    /// <summary>
    /// A page component for admin login.
    /// </summary>
    public partial class Login : ComponentBase
    {
        #region Constants

        /// <summary>
        /// The url for the page. 
        /// </summary>
        public const string PageUrl = "/admin/login";

        /// <summary>
        /// The key where the component looks for redirect url targets after logging in. 
        /// </summary>
        public const string RedirectUrlStorageKey = "AdminLoginRedirectToPage";

        #endregion

        #region Fields

        /// <summary>
        /// An optional URL to redirect the user to after a successful login/registration.
        /// </summary>
        public string? _redirectUrl = null;

        #endregion

        #region Properties

        /// <summary>
        /// The injected session storage service.
        /// </summary>
        [Inject]
        private ISessionStorageService SessionStorageService { get; set; } = default!;

        #endregion

        #region Methods

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (await SessionStorageService.ContainKeyAsync(RedirectUrlStorageKey))
            {
                string redirectToUrl = await SessionStorageService.GetItemAsStringAsync(RedirectUrlStorageKey);
                await SessionStorageService.RemoveItemAsync(RedirectUrlStorageKey);
                _redirectUrl = redirectToUrl;
            }
        }

        #endregion
    }
}
