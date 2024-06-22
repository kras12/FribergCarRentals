using FribergCarRentals.DataAccess.DataTransferObjects;
using FribergCarRentals.DataAccess.EntityClasses;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MvcRazorPages.Shared.ViewModels.Other;
using System.ComponentModel;

namespace MvcRazorPages.Shared.ViewModels.CarCategory
{
    /// <summary>
    /// A view model class that handles data for creation of a car category.
    /// </summary>
    public class CarCategoryViewModel : ViewModelBase
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="category">The car category to model.</param>
        public CarCategoryViewModel(CarCategoryStatisticsDto category)
            : this(category.CarCategoryEntity, category.CarCount)
        {

        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="category">The car category to model.</param>
        /// <param name="carCount">The number of cars using this category.</param>
        public CarCategoryViewModel(CarCategoryEntity category, int? carCount = null)
            : this(category.CarCategoryId, category.CategoryName, carCount)
        {
            #region Checks

            if (category is null)
            {
                throw new ArgumentNullException(nameof(category), $"The value of parameter '{nameof(category)}' can't be null.");
            }

            if (category.CarCategoryId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(category.CarCategoryId), $"The value of property '{nameof(category.CarCategoryId)}' can't be negative.");
            }

            #endregion
        }

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
        [BindNever]
        public string CategoryName { get; } = "";

        /// <summary>
        /// The ID for the category.
        /// </summary>
        [DisplayName("Category ID")]
        [BindNever]
        public int CategoryId { get; }

        /// <summary>
        /// The number of cars using this category. 
        /// </summary>
        [DisplayName("Cars")]
        [BindNever]
        public int? CarCount { get; } = null;

        /// <summary>
        /// Returns true if there's a known number of cars using this category. 
        /// </summary>
        [BindNever]
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
