using FribergCarRentals.Shared.Models.ViewModels.Other;
using FribergCarRentalsBlazor.ViewModels;
using Microsoft.AspNetCore.Components;

namespace FribergCarRentalsBlazor.Pages
{
    /// <summary>
    /// Component for the home page.
    /// </summary>
    public partial class Home : ComponentBase
    {
        #region Properties

        /// <summary>
        /// A collection of image view models for the image slide. 
        /// </summary>
        private ListViewModel<BlazorSlideShowImageViewModel> ImageSlides { get; set; } = new();

		#endregion

		#region Methods

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
		protected override Task OnInitializedAsync()
		{
			return base.OnInitializedAsync();
		}

		#endregion
	}
}
