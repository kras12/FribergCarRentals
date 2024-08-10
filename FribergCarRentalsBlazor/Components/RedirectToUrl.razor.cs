using Microsoft.AspNetCore.Components;

namespace FribergCarRentalsBlazor.Components
{
    /// <summary>
    /// A component that redirects the user to an url 
    /// </summary>
    public partial class RedirectToUrl : ComponentBase
    {
        #region Properties

        /// <summary>
        /// The injected navigation manager. 
        /// </summary>
        [Inject]
        NavigationManager NavigationManager { get; set; } = default!;

        /// <summary>
        /// The url to redirect to.
        /// </summary>
        [Parameter]
        public string? Url { get; set; } = "";

        #endregion

        #region Methods

        /// <summary>
        /// Method invoked when the component is ready to start, having received its initial
        /// parameters from its parent in the render tree.
        /// </summary>
        protected override void OnInitialized()
        {
            if (string.IsNullOrEmpty(Url))
            {
                throw new ArgumentException("The URL parameter was not set.", nameof(Url));
            }

            NavigationManager.NavigateTo(Url);
        }

        #endregion
    }
}
