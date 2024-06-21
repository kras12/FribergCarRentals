using FribergCarRentals.Data.SharedClasses;
using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.DataAccess.Types;
using FribergCarRentals.Models.CarCategory;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Models.Car
{
    /// <summary>
    ///  A view model class that handles data for creating a new car.
    /// </summary>
    public class CreateCarViewModel : CarViewModelBase
    {

        #region Constructors

        /// <summary>
        ///  A constructor.
        /// </summary>
        public CreateCarViewModel() : base()
        {

        }

        /// <summary>
        ///  A constructor.
        /// </summary>
        /// <param name="categories">A collection of available car categories to choose from.</param>
        public CreateCarViewModel(IEnumerable<CarCategoryEntity> categories) : base()
        {
            Categories = categories.Select(x => new CarCategoryViewModel(x)).ToList();
            SelectedCategoryId = Categories.First().CategoryId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// A collection of available car categories to choose from.
        /// </summary>
        public List<CarCategoryViewModel> Categories { get; set; } = new();

        /// <summary>
        /// The ID of the selected category.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        [DisplayName("Category")]
        public int SelectedCategoryId { get; set; }

        /// <summary>
        /// The images to upload
        /// </summary>
        [DisplayName("Upload Images")]
        public List<IFormFile>? UploadImages { get; set; } = null;

        #endregion
    }
}