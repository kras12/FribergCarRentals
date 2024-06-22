using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing.Drawing2D;
using static System.Net.Mime.MediaTypeNames;

namespace FribergCarRentals.Data.EntityClasses
{
    /// <summary>
    /// An entity class that represents an image.
    /// </summary>
    [Table("Images")]
    public class ImageEntity
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public ImageEntity()
        {

        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="fileName">The file name for the image.</param>
        /// <param name="car">The ID for the car the image belongs to.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ImageEntity(string fileName, CarEntity? car = null)
        {
            #region Checks

            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(fileName), $"The value of parameter '{fileName}' can't be null or empty.");
            }

            #endregion

            ImageId = 0;
            FileName = fileName;
            Car = car;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The car the image belongs to. 
        /// </summary>
        [Required]
        public CarEntity? Car {  get; set; }

        /// <summary>
        /// The filename for the image.
        /// </summary>
        public string FileName { get; set; } = "";

        /// <summary>
        /// The ID for the image.
        /// </summary>
        [Key]
        public int ImageId { get; set; }

        #endregion
    }
}