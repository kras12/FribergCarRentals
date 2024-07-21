using FribergCarRentals.Data.EntityClasses;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        /// <param name="url">The url for the image.</param>
        public ImageViewModel(ImageEntity carImage, string url)
            : this(url, carImage.FileName, carImage.ImageId)
        {

        }

        /// <summary>
        /// A constructor
        /// </summary>
        /// <param name="url">The url for the image.</param>
        /// <param name="fileName">The filename for the image.</param>
        /// <param name="imageId">The ID for the image.</param>
        public ImageViewModel(string url, string fileName = "", int? imageId = null)
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
        /// The url for the image.
        /// </summary>
        [DisplayName("Url")]
        [BindNever]
        public string Url { get; } = "";

        #endregion
    }
}
