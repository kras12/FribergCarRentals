namespace FribergCarRentals.Shared.Models.Dto.CarCategory
{
    /// <summary>
    /// A DTO class that represents a car category.
    /// </summary>
    public class CarCategoryDto
    {
        #region Properties

        /// <summary>
        /// The ID of the category.
        /// </summary>
        public int CarCategoryId { get; set; }


        /// <summary>
        /// The name of the category.
        /// </summary>
        public string CategoryName { get; set; } = "";

        #endregion
    }
}
