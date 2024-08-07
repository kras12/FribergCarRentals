using FribergCarRentals.Shared.Models.ViewModels.Image;

namespace FribergCarRentalsBlazor.ViewModels
{
	/// <summary>
	/// View model for an image used in a slide show. 
	/// </summary>
    public class BlazorSlideShowImageViewModel : SlideShowImageViewModel
	{
		#region Constructors

		/// <summary>
		/// A constructor
		/// </summary>
		/// <param name="url">The url for the image.</param>
		/// <param name="fileName">The filename for the image.</param>
		/// <param name="imageId">The ID for the image.</param>
		/// <param name="imageCaption">An optional image caption.</param>
		/// <param name="linksToPage">An optional link to another page.</param>
		public BlazorSlideShowImageViewModel(string url, string fileName = "", int? imageId = null, string? imageCaption = null, string? linksToPage = null) 
			: base(url, fileName, imageId, imageCaption, linksToPage)
		{

		}

		#endregion

		#region Properties

		/// <summary>
		/// Returns true if the image is currently being shown.
		/// </summary>
		public bool IsActive { get; set; } = false;

		#endregion
	}
}
