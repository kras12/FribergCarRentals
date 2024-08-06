namespace FribergCarRentals.Shared.Models.Dto.Car
{
    /// <summary>
	/// A DTO class that stores a collection of IDs for the car images to delete. 
	/// </summary>
    public class DeleteCarImagesDto
    {
        #region Properties

        /// <summary>
        /// A collection of IDs for the car images to delete. 
        /// </summary>
        public List<int> ImageIds { get; set; } = new();

        #endregion
    }
}