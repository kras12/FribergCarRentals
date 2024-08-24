namespace FribergCarRentals.Shared.Models.Dto.Car
{
    /// <summary>
	/// A DTO class that stores a collection of IDs for the car images to delete. 
	/// </summary>
    public class DeleteCarImagesDto
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public DeleteCarImagesDto()
        {
            
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="imageId">The ID of the image to delete.</param>
        public DeleteCarImagesDto(int imageId) : this(new List<int>() { imageId })
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="imageIds">A collection of IDs for the car images to delete.</param>
        public DeleteCarImagesDto(List<int> imageIds)
        {
            #region Checks

            if (imageIds == null)
            {
                throw new ArgumentNullException(nameof(imageIds));
            }

            #endregion

            ImageIds = imageIds;
        }

        #endregion

        #region Properties

        /// <summary>
        /// A collection of IDs for the car images to delete. 
        /// </summary>
        public List<int> ImageIds { get; set; } = new();        

        #endregion
    }
}