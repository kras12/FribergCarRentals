using FribergCarRentals.Data;
using FribergCarRentals.Models.Other;
using FribergCars.Shared.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Models.Components
{
	/// <summary>
	/// A view model class designed to be used with the image slideshow component <see cref="CarImageSlideShow"/>.
	/// </summary>
	public class SlideShowImageViewModel : ViewModelBase
    {
        #region Constructors

		/// <summary>
		/// A constructor
		/// </summary>
		/// <param name="url">The url for the image.</param>
		/// <param name="fileName">The filename for the image.</param>
		/// <param name="imageId">The ID for the image.</param>
		/// <param name="linksToPage">An optional link to another page.</param>
		/// <param name="imageCaption">An optional image caption.</param>
		public SlideShowImageViewModel(string url, string fileName = "", int? imageId = null, string? imageCaption = null, RedirectToPageData? linksToPage = null)
        {
            FileName = fileName;
            Url = url;
            ImageId = imageId;
            LinksToPage = linksToPage;
            ImageCaption = imageCaption;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The filename for the image.
        /// </summary>
        [DisplayName("File Name")]
        [BindNever]
        public string FileName { get; } = "";

        /// <summary>
        /// An optional image caption. 
        /// </summary>
        public string? ImageCaption { get; set; } = null;

        /// <summary>
        /// Returns true if there is an image caption.
        /// </summary>
        public bool HaveCaption
        {
            get
            {
                return !string.IsNullOrEmpty(ImageCaption);
            }
        }

        /// <summary>
        /// The ID for the image.
        /// </summary>
        [DisplayName("Image ID")]
        [BindNever]
        public int? ImageId { get; }

		/// <summary>
		/// An optional link to another page.
		/// </summary>
		[DisplayName("Link")]
		[BindNever]
		public RedirectToPageData? LinksToPage { get; set;  }

        /// <summary>
        /// The url for the image.
        /// </summary>
        [DisplayName("Url")]
        [BindNever]
        public string Url { get; } = "";


        #endregion
    }
}
