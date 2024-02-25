using FribergCarRentals.Data;
using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Models.Other
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
        /// <param name="linksToAction">An optional link to another action.</param>
        public ImageViewModel(string url, string fileName = "", int? imageId = null, RedirectToActionData? linksToAction = null)
        {
            FileName = fileName;
            Url = url;
            ImageId = imageId;
            LinksToAction = linksToAction;
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
		/// An optional link to another page.
		/// </summary>
		[DisplayName("Link")]
		[BindNever]
		public RedirectToActionData? LinksToAction { get; set;  }

        /// <summary>
        /// The url for the image.
        /// </summary>
        [DisplayName("Url")]
        [BindNever]
        public string Url { get; } = "";


        #endregion
    }
}
