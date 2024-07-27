using FribergCarRentals.Shared.Dto.Car;

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
        public CarDto Car { get; set; }

        /// <summary>
        /// The filename for the image.
        /// </summary>
        public string FileName { get; set; } = "";

        /// <summary>
        /// The ID for the image.
        /// </summary>
        public int ImageId { get; set; }

        #endregion
    }
}