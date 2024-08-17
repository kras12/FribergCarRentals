using FribergCarRentals.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace FribergCarRentalsBlazor.Pages
{
    /// <summary>
    /// Page component base class with support for basic authorization.
    /// </summary>
    public abstract class PageComponentBase : ComponentBase
    {
        #region Properties

        /// <summary>
        /// The cascaded authentication state task.
        /// </summary>
        [CascadingParameter]
        protected Task<AuthenticationState> AuthenticationStateTask { get; set; } = default!;

        /// <summary>
        /// The injected authorization service.
        /// </summary>
        [Inject]
        protected IAuthorizationService AuthorizationService { get; set; } = default!;

        #endregion

        #region Methods

        /// <summary>
        /// Checks whether the current user is a logged in admin. 
        /// </summary>
        /// <returns>True if the user is a logged in admin.</returns>
        protected async Task<bool> IsAdminLoggedIn()
        {
            var authorizationResult = await AuthorizationService.AuthorizeAsync((await AuthenticationStateTask).User, ApplicationUserPolicies.Admin);

            if (authorizationResult.Succeeded)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks whether the current user is a logged in customer. 
        /// </summary>
        /// <returns>True if the user is a logged in customer.</returns>
        protected async Task<bool> IsCustomerLoggedIn()
        {
            var authorizationResult = await AuthorizationService.AuthorizeAsync((await AuthenticationStateTask).User, ApplicationUserPolicies.Customer);

            if (authorizationResult.Succeeded)
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}
