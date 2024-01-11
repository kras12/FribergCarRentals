using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing.Drawing2D;

namespace FribergCarRentals.Models
{
    /// <summary>
    /// A class that represents an image.
    /// </summary>
    [Table("Images")]
    public class ImageEntity
    {
        #region Constructors

        /// <summary>
        /// A constructor mainly intended for EF Core.
        /// </summary>
        /// <param name="imageId">The ID for the image. Can't be a negative value</param>
        /// <param name="filePath">The filepath for the image. Can't be null or empty.</param>
        private ImageEntity(int imageId, string filePath)
        {
            #region Checks

            if (imageId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(imageId), "The ID can't be a negative value.");
            }

            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath), "The filepath can't be null or empty.");
            }

            #endregion

            ImageId = imageId;
            FilePath = filePath;
        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="filePath">The filepath for the image. Can't be null or empty.</param>
        public ImageEntity(string filePath) : this(imageId: 0, filePath)
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// The ID for the image.
        /// </summary>
        [Key]
        public int ImageId { get; private set; }

        /// <summary>
        /// The filepath for the image.
        /// </summary>
        public string FilePath { get; private set; } = "";

        #endregion
    }
}
