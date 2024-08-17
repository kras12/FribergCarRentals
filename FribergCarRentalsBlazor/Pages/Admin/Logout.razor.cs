using FribergCarRentalsBlazor.Services.Authentication;
using Microsoft.AspNetCore.Components;

namespace FribergCarRentalsBlazor.Pages.Admin
{
    /// <summary>
    /// A component for logging out an admin.
    /// </summary>
    public partial class LogoutAdmin : ComponentBase
    {
        #region Properties

        /// <summary>
        /// The injected admin authentication service. 
        /// </summary>
        [Inject]
        private IAdminAuthenticationService AdminAuthenticationService { get; set; } = default!;

        #endregion

        #region Methods

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await AdminAuthenticationService.Logout();
        }

        #endregion
    }
}
