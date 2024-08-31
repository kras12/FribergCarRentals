using AutoMapper;
using FribergCarRentals.Shared.Models.Dto.Admin;
using FribergCarRentals.Shared.Models.ViewModels.Admin;
using FribergCarRentals.Shared.Models.ViewModels.Message;
using FribergCarRentalsBlazor.Services.Authentication;
using Microsoft.AspNetCore.Components;

namespace FribergCarRentalsBlazor.Components
{
    /// <summary>
    /// A component that shows a login form and handles the login for an admin.
    /// </summary>
    public partial class LoginAdmin : ComponentBase
    {
		#region Fields

		/// <summary>
		/// A collection of validation errors returned from the API.
		/// </summary>
		private List<MessageViewModel> _apiValidationErrors = new();

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
        /// An event that triggers when the user submits the login form and before the login process starts. 
        /// </summary>
        [Parameter]
        public EventCallback OnBeforeLogin { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// The data binding property for the login form. 
        /// </summary>
        [SupplyParameterFromForm]
        public LoginAdminViewModel FormInput { get; set; } = new();        

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
        /// The injected admin authentication service. 
        /// </summary>
        [Inject]
        private IAdminAuthenticationService AdminAuthenticationService { get; set; } = default!;

        /// <summary>
        /// The injected navigation manager. 
        /// </summary>
        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        #endregion

        #region Methods

        /// <summary>
        /// Event handler for a valid form submission.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        private async Task OnValidSubmit()
        {
            await OnBeforeLogin.InvokeAsync();
            _apiValidationErrors.Clear();
            var result = await AdminAuthenticationService.LoginAdmin(AutoMapper.Map<LoginAdminDto>(FormInput));

            if (result.Success)
            {
                await OnLoginSuccessful.InvokeAsync();

                if (!string.IsNullOrEmpty(RedirectUrl))
                {
                    NavigationManager.NavigateTo(RedirectUrl);
                }

                // TODO - Check how to update the application state if not redirected.
            }
            else
            {
                _apiValidationErrors = result.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList();
                await OnLoginFailed.InvokeAsync();
            }
        }

        #endregion
    }
}
