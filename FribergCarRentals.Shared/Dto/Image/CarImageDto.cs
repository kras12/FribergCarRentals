namespace FribergCarRentals.Shared.Dto.Image
{
    /// <summary>
    /// A DTO class for far images.
    /// </summary>
    public class CarImageDto
    {
        #region Properties

        /// <summary>
        /// The car the image belongs to. 
        /// </summary>
        public int CarId { get; set; }

        /// <summary>
        /// The filename for the image.
        /// </summary>
        public string FileName { get; set; } = "";

        /// <summary>
        /// The ID for the image.
        /// </summary>
        public int ImageId { get; set; }

        /// <summary>
        /// The url for the image.
        /// </summary>
        public string Url { get; set;  } = "";

        #endregion
    }
}