namespace FribergCarRentals.Shared.Models.Dto.CarCategory
{
    /// <summary>
    /// A DTO class that contains statistics for a car category. 
    /// </summary>
    public class CarCategoryStatisticsDto : CarCategoryDto
    {
        #region Properties

        /// <summary>
        /// The number of cars using this category. 
        /// </summary>
        public int CarCount { get; set; }

        #endregion
    }
}
