using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Models.Car
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
        [DisplayName("ID")]
        public int CarId { get; set; }

        /// <summary>
        /// Returns a short description of the car (brand, model, and year).
        /// </summary>
        [DisplayName("Car")]
        public string CarInfoWithRegistrationNumber
        {
            get
            {
                return $"{RegistrationNumber} - {Brand} {Model} {ModelYear} ";
            }
        }

        /// <summary>
        /// Returns a short description of the car (brand, model, and year).
        /// </summary>
        [DisplayName("Car")]
        public string CarInfo
        {
            get
            {
                return $"{Brand} {Model} {ModelYear} ";
            }
        }

        /// <summary>
        /// The color for the car.
        /// </summary>
        public string Color { get; set; } = "";

        [DisplayName("Images")]
        public string ImageFilePaths { get; set; } = "";

        /// <summary>
        /// A collection of images for the car.
        /// </summary>
        [DisplayName("Images")]
        public List<ImageEntity> Images { get; set; } = new();

        /// <summary>
        /// The number of images for this car.
        /// </summary>
        [DisplayName("Images")]
        public int ImageCount
        {
            get
            {
                return Images.Count;
            }
        }

        /// <summary>
        /// The model for the car.
        /// </summary>
        public string Model { get; set; } = "";

        /// <summary>
        /// The model year for the car.
        /// </summary>
        [DisplayName("Year")]
        public int ModelYear { get; set; }

        /// <summary>
        /// The propulsion system for the car.
        /// </summary>
        [DisplayName("Propulsion")]
        public VehiclePropulsionEntity PropulsionSystem { get; set; }

        [Required]
        /// <summary>
        /// The registration number for the car.
        /// </summary>
        [DisplayName("Reg Nr")]
        public string RegistrationNumber { get; set; } = "";
        /// <summary>
        /// The rental status for the car.
        /// </summary>
        [DisplayName("Status")]
        public CarRentalStatusEntity RentalStatus { get; set; }

        #endregion
    }
}