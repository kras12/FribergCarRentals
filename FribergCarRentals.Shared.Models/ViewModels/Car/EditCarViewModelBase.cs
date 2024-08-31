using FribergCarRentals.Shared.Models.ViewModels.CarCategory;
using FribergCarRentals.Shared.Models.ViewModels.Image;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FribergCarRentals.Shared.Attributes;
using FribergCarRentals.Shared.Enums;
using FribergCarRentals.Shared.Extensions;
using FribergCarRentals.Shared.Constants;

namespace FribergCarRentals.Shared.Models.ViewModels.Car
{
    /// <summary>
    /// A view model base class to handle data used for editing a car. 
    /// </summary>
    public abstract class EditCarViewModelBase<TUploadedImage> : CarViewModelBase where TUploadedImage : class
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        protected EditCarViewModelBase() : base()
        {
            InitRentalStatuses();
            InitVehiclePropulsions();
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
        [DisplayName("Categories")]
        public virtual List<CarCategoryViewModel> Categories { get; set; } = new();

        /// <summary>
        /// The images to delete.
        /// </summary>
        [DisplayName("Delete Images")]
        public List<int> DeleteImages { get; set; } = new();

        /// <summary>
        /// A collection of images for the car.
        /// </summary>
        [DisplayName("Images")]
        public virtual List<ImageViewModel> Images { get; set; } = new();

        /// <summary>
        /// A collection of rental statuses to choose from.
        /// </summary>
        public List<CarRentalStatusViewModel> RentalStatuses { get; private set; } = new();

        /// <summary>
        /// A collection of vehicle propulsion systmes to choose from. 
        /// </summary>
        public List<VehiclePropulsionViewModel> VehiclePropulsions { get; private set; } = new();

        /// <summary>
        /// The ID of the selected category.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int SelectedCategoryId { get; set; }

        /// <summary>
        /// The ID of the selected propulsion system.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = ValidationMessages.MandatoryFieldValidationMessage)]
        [Range(1, int.MaxValue)]
        [DisplayName("Propulsion")]
        public int SelectedPropulsionSystemId { get; set; } = new();

        /// <summary>
        /// The ID of the selected rental status. 
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = ValidationMessages.MandatoryFieldValidationMessage)]
        [Range(1, int.MaxValue)]
        [DisplayName("Status")]
        public int SelectedRentalStatusId { get; set; } = new();

        /// <summary>
        /// The images to upload
        /// </summary>
        [DisplayName("Upload Images")]
        public List<TUploadedImage>? UploadImages { get; set; } = new();

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the collection of rental statuses.
        /// </summary>
        private void InitRentalStatuses()
        {
            RentalStatuses = Enum.GetValues<RentalCarStatus>()
                .Select(x => new CarRentalStatusViewModel((int)x, x.GetAttribute<EnumDatabaseValueAttribute>().Value, x.GetAttribute<EnumDatabaseValueAttribute>().DescriptionValue))
                .ToList();
        }

        /// <summary>
        /// Initializes the collection of vehicle propulsions.
        /// </summary>
        private void InitVehiclePropulsions()
        {
            VehiclePropulsions = Enum.GetValues<VehiclePropulsionType>()
                .Select(x => new VehiclePropulsionViewModel((int)x, x.GetAttribute<EnumDatabaseValueAttribute>().Value, x.GetAttribute<EnumDatabaseValueAttribute>().DescriptionValue))
                .ToList();
        }

        #endregion
    }
}