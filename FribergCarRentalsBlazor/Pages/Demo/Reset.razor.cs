using FribergCarRentals.Shared.Models.ViewModels.Message;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.DemoApi;
using Microsoft.AspNetCore.Components;

namespace FribergCarRentalsBlazor.Pages.Demo
{
    /// <summary>
    /// Page component for resetting the demo data. 
    /// </summary>
    public partial class Reset : ComponentBase
    {
		#region Constants

		/// <summary>
		/// The url template for the page. 
		/// </summary>
		private const string PageUrlTemplate = "/demo/reset";

        #endregion

        #region Fields

        /// <summary>
        /// A collection of validation errors returned from the API.
        /// </summary>
        private List<MessageViewModel> _apiValidationErrors = new();

        /// <summary>
        /// True when the component have been initialized.
        /// </summary>
        private bool _isInitialized = false;

        #endregion

        #region Properties

        /// <summary>
        /// The injected demo API service.
        /// </summary>
        [Inject]
        private IDemoApiService DemoApiService { get; set; } = default!;

        #endregion

        #region Methods

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var result = await DemoApiService.ResetDemo();

            if (!result.Success)
            {
                _apiValidationErrors = result.Errors.Select(x => new MessageViewModel(MessageType.Error, x.ErrorMessage, title: x.ErrorType)).ToList();
            }

            _isInitialized = true;
        }

        #endregion
    }
}
