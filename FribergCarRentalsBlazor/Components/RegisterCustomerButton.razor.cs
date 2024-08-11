using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using static FribergCarRentalsBlazor.Components.RegisterCustomer;

namespace FribergCarRentalsBlazor.Components
{
    /// <summary>
    /// A component that handles both registration functionality for customers.
    /// </summary>
    public partial class RegisterCustomerButton : ComponentBase
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
        public string RegisterButtonCssClasses { get; set; } = "";

        /// <summary>
        /// The injected JavaScript runtime. 
        /// </summary>
        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

        #endregion

        #region Methods

        /// <summary>
        /// Event handler for the registration successful event in the <see cref="RegisterCustomer"/> component.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        private async Task OnRegistrationSuccessful(RegistrationStatus registrationStatus)
        {
            if (registrationStatus == RegistrationStatus.Completed)
            {
				await JSRuntime.InvokeVoidAsync("HideCustomerRegistrationModal", _modalDialogId);
			}
        }      

        #endregion
    }
}
