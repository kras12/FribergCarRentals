namespace FribergCarRentals.Shared.Dto.Car
{
    /// <summary>
    /// A DTO class that handles data for editing a car category.
    /// </summary>
    public class EditCarCategoryDto
    {
        #region Properties

        /// <summary>
        /// The filename for the category.
        /// </summary>
        public string CategoryName { get; set; } = "";

        #endregion
    }
}
