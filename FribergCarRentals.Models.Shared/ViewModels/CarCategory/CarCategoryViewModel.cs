using System.ComponentModel;
using FribergCarRentals.Shared.ViewModels.Other;

namespace FribergCarRentals.Shared.ViewModels.CarCategory
{
    /// <summary>
    /// A view model class that handles data for creation of a car category.
    /// </summary>
    public class CarCategoryViewModel : ViewModelBase
    {
        #region Constructors

        /// <summary>
        /// A constructor
        /// </summary>
        /// <param name="categoryId">The ID for the category.</param>
        /// <param name="categoryName">The name for the category.</param>
        /// <param name="carCount">The number of cars using this category.</param>
        public CarCategoryViewModel(int categoryId, string categoryName, int? carCount = null)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
            CarCount = carCount;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The name for the category.
        /// </summary>
        [DisplayName("Category")]
        public string CategoryName { get; set; } = "";

        /// <summary>
        /// The ID for the category.
        /// </summary>
        [DisplayName("Category ID")]
        public int CategoryId { get; set; }

        /// <summary>
        /// The number of cars using this category. 
        /// </summary>
        [DisplayName("Cars")]
        public int? CarCount { get; set; } = null;

        /// <summary>
        /// Returns true if there's a known number of cars using this category. 
        /// </summary>
        public bool HaveCarCount
        {
            get
            {
                return CarCount != null;
            }
        }

        #endregion
    }
}
