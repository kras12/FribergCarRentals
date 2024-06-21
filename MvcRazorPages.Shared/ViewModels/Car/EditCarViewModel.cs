using FribergCarRentals.DataAccess.EntityClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MvcRazorPages.Shared.ViewModels.CarCategory;
using MvcRazorPages.Shared.ViewModels.Image;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcRazorPages.Shared.ViewModels.Car
{
    /// <summary>
    /// A view model class to handle data used for editing a car. 
    /// </summary>
    public class EditCarViewModel : CarViewModelBase
    {

        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public EditCarViewModel() : base()
        {

        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="car">The car to model.</param>
        /// <param name="categories">A collection of available car categories to choose from.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public EditCarViewModel(CarEntity car, IEnumerable<CarCategoryEntity> categories)
            : base(car.Brand, car.Color, car.Model, car.ModelYear, car.PropulsionSystem!, car.RegistrationNumber, car.RentalCostPerDay, car.RentalStatus!)
        {
            #region Checks

            if (car.CarId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(car.CarId), $"The value of property '{car.CarId}' can't be negative.");
            }

            #endregion

            CarId = car.CarId;
            Categories = categories.Select(x => new CarCategoryViewModel(x)).ToList();
            Images = car.Images.Select(x => new ImageViewModel(x)).ToList();
            SelectedCategoryId = car.Category!.CarCategoryId;
            PageSubTitle = $"#{CarId} - {CarInfo}";
        }

        #endregion

        #region Properties

        /// <summary>
        /// The ID for the car.
        /// </summary>
        [DisplayName("ID")]
        [Required]
        public int CarId { get; set; }

        /// <summary>
        /// A collection of available car categories to choose from.
        /// </summary>
        public List<CarCategoryViewModel> Categories { get; set; } = new();

        /// <summary>
        /// The images to delete.
        /// </summary>
        [DisplayName("Delete Images")]
        public List<int>? DeleteImages { get; set; } = new();

        /// <summary>
        /// A collection of images for the car.
        /// </summary>
        [DisplayName("Images")]
        [BindNever]
        public List<ImageViewModel> Images { get; set;  } = new();

        /// <summary>
        /// The ID of the selected category.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int SelectedCategoryId { get; set; }

        /// <summary>
        /// The images to upload
        /// </summary>
        [DisplayName("Upload Images")]
        public List<IFormFile>? UploadImages { get; set; } = null;

        #endregion

    }
}