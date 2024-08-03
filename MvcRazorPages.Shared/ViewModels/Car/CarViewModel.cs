using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Shared.Constants;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MvcRazorPages.Shared.Services;
using MvcRazorPages.Shared.ViewModels.CarCategory;
using MvcRazorPages.Shared.ViewModels.Image;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcRazorPages.Shared.ViewModels.Car
{
    /// <summary>
    /// A view model class that handles car data.
    /// </summary>
    public class CarViewModel : CarViewModelBase
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="car">The car to model.</param>
        /// <param name="imageUploadService">The image upload service used for creating the image urls.</param>
        public CarViewModel(CarEntity car, IImageUploadService imageUploadService) 
            : base(car.Brand, car.Color, car.Model, car.ModelYear, car.PropulsionSystem!, car.RegistrationNumber, car.RentalCostPerDay, car.RentalStatus!)
        {
            #region Checks

            if (car.Category is null)
            {
                throw new ArgumentNullException(nameof(car.Category), $"The value of property '{nameof(car.Category)}' can't be null");
            }

            if (car.CarId < 0)
            {
                throw new ArgumentNullException(nameof(car.CarId), $"The value of property '{nameof(car.CarId)}' can't be null");
            }

            if (car.Images is null)
            {
                throw new ArgumentNullException(nameof(car.Images), $"The value of property '{nameof(car.Images)}' can't be null");
            }

            #endregion

            CarId = car.CarId;
            Category = new CarCategoryViewModel(car.Category);
            Images = car.Images.Select(x => new ImageViewModel(imageUploadService.GetImageUrl(x))).ToList();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The ID for the car.
        /// </summary>
        [DisplayName("Car ID")]
        [BindNever]
        public int CarId { get; }

        /// <summary>
        /// The category for the car.
        /// </summary>
        [DisplayName("Category")]
        public virtual CarCategoryViewModel Category { get; set; }

        /// <summary>
        /// A collection of images for the car.
        /// </summary>
        [DisplayName("Images")]
        [BindNever]
        public List<ImageViewModel> Images { get; } = new();

        /// <summary>
        /// The rental cost per day.
        /// </summary>
        [DisplayName("Cost per day")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = ValidationRules.DefaultPriceOutputFormatString)]
        [BindNever]
        public override decimal RentalCostPerDay { get; set; }

        #endregion
    }
}