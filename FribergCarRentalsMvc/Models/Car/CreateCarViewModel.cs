using FribergCarRentals.Data.SharedClasses;
using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.DataAccess.Types;
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
        /// A constructor.
        /// </summary>
        /// <param name="brand">The brand for the car.</param>
        /// <param name="color">The color for the car.</param>
        /// <param name="model">The model for the car.</param>
        /// <param name="modelYear">The model year for the car.</param>
        /// <param name="propulsionSystem">The propulsion system for the car.</param>
        /// <param name="registrationNumber">The registration number for the car.</param>
        /// <param name="rentalCostPerDay">The rental cost per day.</param>
        /// <param name="rentalStatus">The rental status for the car.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public CreateCarViewModel(string brand, string color, string model, int modelYear, VehiclePropulsionEntity propulsionSystem,
            string registrationNumber, decimal rentalCostPerDay, CarRentalStatusEntity rentalStatus) 
            : base(brand, color, model, modelYear, propulsionSystem, registrationNumber, rentalCostPerDay, rentalStatus)
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// The images to upload
        /// </summary>
        [DisplayName("Upload Images")]
        public List<IFormFile>? UploadImages { get; set; } = null;

        #endregion
    }
}