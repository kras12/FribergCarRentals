using System.ComponentModel;
using FribergCarRentals.Shared.Models.ViewModels.Car;
using FribergCarRentals.Shared.Models.ViewModels.CarCategory;
using FribergCarRentals.Shared.Models.ViewModels.Image;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FribergCarRentalsApi.Models.Car
{
    /// <summary>
    /// A view model class to handle data used for editing a car. 
    /// </summary>
    public class EditCarViewModel : EditCarViewModelBase<IFormFile>
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
        /// <param name="carId">The ID of the car.</param>
        /// <param name="carCategory">The ID of the car category.</param>
        /// <param name="brand">The brand for the car.</param>
        /// <param name="color">The color for the car.</param>
        /// <param name="model">The model for the car.</param>
        /// <param name="modelYear">The model year for the car.</param>
        /// <param name="propulsionSystem">The propulsion system for the car.</param>
        /// <param name="registrationNumber">The registration number for the car.</param>
        /// <param name="rentalCostPerDay">The rental cost per day.</param>
        /// <param name="rentalStatus">The rental status for the car.</param>
        /// <param name="categories">A collection of available car categories to choose from.</param>
        /// <param name="images">The images for the car.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public EditCarViewModel(int carId, int carCategory, string brand, string color, string model, int modelYear, int propulsionSystem,
            string registrationNumber, decimal rentalCostPerDay, int rentalStatus, IEnumerable<CarCategoryViewModel> categories, IEnumerable<ImageViewModel> images)
            : base(carId, carCategory, brand, color, model, modelYear, propulsionSystem, registrationNumber, rentalCostPerDay, rentalStatus)
        {
            Categories = categories.ToList();
            Images = images.ToList();
        }

        #endregion

        #region Properties

        /// <summary>
        /// A collection of available car categories to choose from.
        /// </summary>
        [DisplayName("Categories")]
        [BindNever]
        public override List<CarCategoryViewModel> Categories { get; set; } = new();

        /// <summary>
        /// A collection of images for the car.
        /// </summary>
        [DisplayName("Images")]
        [BindNever]
        public override List<ImageViewModel> Images { get; set; } = new();

        #endregion
    }
}