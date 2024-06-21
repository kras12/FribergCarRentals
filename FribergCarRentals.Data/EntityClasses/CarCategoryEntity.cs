using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FribergCarRentals.DataAccess.EntityClasses
{
    /// <summary>
    /// A an entity class that represents a car category.
    /// </summary>
    [Table("CarCategories")]
    public class CarCategoryEntity
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public CarCategoryEntity()
        {
            
        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="categoryName">The name of the category.</param>
        public CarCategoryEntity(string categoryName)
        {
            #region Checks

            if (string.IsNullOrEmpty(categoryName))
            {
                throw new ArgumentException($"The value of parameter '{nameof(categoryName)}' can't be empty.", nameof(categoryName));
            }

            #endregion

            CategoryName = categoryName;
        }


        #endregion

        #region Properties

        /// <summary>
        /// The ID of the category.
        /// </summary>
        [Key]
        public int CarCategoryId { get; set; }


        /// <summary>
        /// The name of the category.
        /// </summary>
        public string CategoryName { get; set; } = "";

       
        #endregion
    }
}
