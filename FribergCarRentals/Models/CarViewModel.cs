using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Models
{
    public class CarViewModel : ViewModelBase
    {

        #region Constructors

        public CarViewModel()
        {

        }

        public CarViewModel(CarEntity carEntity)
        {
            Brand = carEntity.Brand;
            CarId = carEntity.CarId;
            Color = carEntity.Color;
            Model = carEntity.Model;
            ModelYear = carEntity.ModelYear;
            RegistrationNumber = carEntity.RegistrationNumber;
            Images = carEntity.Images;
            ImageFilePaths = string.Join(Environment.NewLine, carEntity.Images.Select(x => x.FilePath).ToList());
            RentalStatus = carEntity.RentalStatus;
            PropulsionSystem = carEntity.PropulsionSystem;
        }

        /// <summary>
        /// The brand for the car.
        /// </summary>
        public string Brand { get; set; } = "";

        /// <summary>
        /// The ID for the car.
        /// </summary>
        [DisplayName("Car ID")]
        public int CarId { get; set; }

        /// <summary>
        /// The color for the car.
        /// </summary>
        public string Color { get; set; } = "";

        /// <summary>
        /// The model for the car.
        /// </summary>
        public string Model { get; set; } = "";

        /// <summary>
        /// The model year for the car.
        /// </summary>
        [DisplayName("Model Year")]
        public int ModelYear { get; set; }

        [Required]
        /// <summary>
        /// The registration number for the car.
        /// </summary>
        [DisplayName("Registration Nr")]
        public string RegistrationNumber { get; set; } = "";

        /// <summary>
        /// A collection of images for the car.
        /// </summary>
        public List<ImageEntity> Images { get; set; } = new();

        [DisplayName("Images")]
        public string ImageFilePaths { get; set; } = "";

        /// <summary>
        /// The rental status for the car.
        /// </summary>
        [DisplayName("Rental Status")]
        public CarRentalStatusEntity RentalStatus { get; set; }

        /// <summary>
        /// The propulsion system for the car.
        /// </summary>
        [DisplayName("Propulsion")]
        public VehiclePropulsionEntity PropulsionSystem { get; set; } 

        #endregion
    }
}