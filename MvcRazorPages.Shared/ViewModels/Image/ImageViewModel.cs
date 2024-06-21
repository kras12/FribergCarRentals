using FribergCarRentals.DataAccess.EntityClasses;
using MvcRazorPages.Shared.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MvcRazorPages.Shared.Data;
using System.ComponentModel;
using MvcRazorPages.Shared.ViewModels.Other;

namespace MvcRazorPages.Shared.ViewModels.Image
{
    /// <summary>
    /// A view model class that handles data for an image. 
    /// </summary>
    public class ImageViewModel : ViewModelBase
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="carImage">The car image to model.</param>
        public ImageViewModel(ImageEntity carImage)
            : this(ImageHelper.GetImageFileUrl(carImage), carImage.FileName, carImage.ImageId)
        {

        }

        /// <summary>
        /// A constructor
        /// </summary>
        /// <param name="url">The url for the image.</param>
        /// <param name="fileName">The filename for the image.</param>
        /// <param name="imageId">The ID for the image.</param>
        /// <param name="linksToPage">An optional link to another page.</param>
        public ImageViewModel(string url, string fileName = "", int? imageId = null, RedirectToActionData? linksToPage = null) : this(url, fileName, imageId)
        {
            LinksToControllerAction = linksToPage;
        }

        /// <summary>
        /// A constructor
        /// </summary>
        /// <param name="url">The url for the image.</param>
        /// <param name="fileName">The filename for the image.</param>
        /// <param name="imageId">The ID for the image.</param>
        /// <param name="linksToPage">An optional link to another page.</param>
        public ImageViewModel(string url, string fileName = "", int? imageId = null, RedirectToPageData? linksToPage = null) : this(url, fileName, imageId)
        {
            LinksToRazorPage = linksToPage;
        }

        /// <summary>
        /// A constructor
        /// </summary>
        /// <param name="url">The url for the image.</param>
        /// <param name="fileName">The filename for the image.</param>
        /// <param name="imageId">The ID for the image.</param>
        private ImageViewModel(string url, string fileName = "", int? imageId = null)
        {
            FileName = fileName;
            Url = url;
            ImageId = imageId;
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
        /// The ID for the image.
        /// </summary>
        [DisplayName("Image ID")]
        [BindNever]
        public int? ImageId { get; }

		/// <summary>
		/// An optional link to another page in form of an controller action.
		/// </summary>
		[DisplayName("Link")]
		[BindNever]
		public RedirectToActionData? LinksToControllerAction { get; set;  }

        /// <summary>
		/// An optional link to another page in form of a Razor Page.
		/// </summary>
		[DisplayName("Link")]
        [BindNever]
        public RedirectToPageData? LinksToRazorPage { get; set; }

        /// <summary>
        /// The url for the image.
        /// </summary>
        [DisplayName("Url")]
        [BindNever]
        public string Url { get; } = "";

        #endregion
    }
}
