using FribergCarRentalsBlazor.Services.Authentication;
using Microsoft.AspNetCore.Components;

namespace FribergCarRentalsBlazor.Pages.Admin
{
    /// <summary>
    /// A component for logging out an admin.
    /// </summary>
    public partial class Logout : AdminPageComponentBase
    {
		#region Constants

		/// <summary>
		/// The url for the page. 
		/// </summary>
		public const string PageUrl = "/admin/logout";

		#endregion

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
            NavigationManager.NavigateTo(Pages.Home.PageUrl);
        }

        #endregion
    }
}
