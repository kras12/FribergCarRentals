namespace FribergCarRentals.Shared.Dto.Car
{
    /// <summary>
    /// A DTO class that handles data for a car category. 
    /// </summary>
    public class CreateCarCategoryDto
    {
        #region Properties

        /// <summary>
        /// The name for the category.
        /// </summary>
        public string CategoryName { get; set; } = "";

        #endregion
    }
}
