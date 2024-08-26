using System.ComponentModel;
using FribergCarRentals.Shared.Models.ViewModels.Other;

namespace FribergCarRentals.Shared.Models.ViewModels.Image
{
    /// <summary>
    /// A view model class that handles data for an image. 
    /// </summary>
    public class ImageViewModel : ViewModelBase
    {
        #region Properties

        /// <summary>
        /// The filename for the image.
        /// </summary>
        [DisplayName("File Name")]
        public string FileName { get; set; } = "";

        /// <summary>
        /// The ID for the image.
        /// </summary>
        [DisplayName("Image ID")]
        public int? ImageId { get; set; }

        /// <summary>
        /// The url for the image.
        /// </summary>
        [DisplayName("Url")]
        public string Url { get; set; } = "";

        #endregion
    }
}
