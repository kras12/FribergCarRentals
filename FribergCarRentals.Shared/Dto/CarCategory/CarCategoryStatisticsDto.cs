namespace FribergCarRentals.Shared.Dto.CarCategory
{
    /// <summary>
    /// A DTO class that handles data for creation of a car category.
    /// </summary>
    public class CarCategoryStatisticsDto
    {
        #region Properties

        /// <summary>
        /// The car category.
        /// </summary>
        public CarCategoryDto CarCategory { get; set; } = null!;

        /// <summary>
        /// The number of cars using this category. 
        /// </summary>
        public int CarCount { get; set; }

        #endregion
    }
}
