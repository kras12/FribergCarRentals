using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FribergCarRentals.Shared.Models.ViewModels.CarCategory;
using FribergCarRentals.Shared.Types.Attributes;
using FribergCarRentals.Shared.Types.Enums;
using FribergCarRentals.Shared.Types.Extensions;

namespace FribergCarRentals.Shared.Models.ViewModels.Car
{
	/// <summary>
	///  A view model base class that handles data for creating a new car.
	/// </summary>
	public abstract class CreateCarViewModelBase<TUploadedImage> : CarViewModelBase where TUploadedImage : class
	{
        #region Constructors

        /// <summary>
        ///  A constructor.
        /// </summary>
        protected CreateCarViewModelBase() : base()
        {
            RentalStatuses = Enum.GetValues<RentalCarStatus>()
                .Select(x => new CarRentalStatusViewModel((int)x, x.GetAttribute<EnumDatabaseValueAttribute>().Value, x.GetAttribute<EnumDatabaseValueAttribute>().DescriptionValue))
                .ToList();

            VehiclePropulsions = Enum.GetValues<VehiclePropulsionType>()
                .Select(x => new VehiclePropulsionViewModel((int)x, x.GetAttribute<EnumDatabaseValueAttribute>().Value, x.GetAttribute<EnumDatabaseValueAttribute>().DescriptionValue))
                .ToList();
        }

        /// <summary>
        ///  A constructor.
        /// </summary>
        /// <param name="categories">A collection of available car categories to choose from.</param>
        protected CreateCarViewModelBase(IEnumerable<CarCategoryViewModel> categories) : this()
        {
            Categories = categories.ToList();
            SelectedCategoryId = Categories.FirstOrDefault()?.CategoryId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// A collection of available car categories to choose from.
        /// </summary>
        public List<CarCategoryViewModel> Categories { get; set; } = new();

        /// <summary>
        /// Returns true if any car categories was found.
        /// </summary>
        public bool HaveCategories
        {
            get
            {
                return Categories.Count > 0;
            }
        }

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
        [DisplayName("Category")]
        public int? SelectedCategoryId { get; set; }

		/// <summary>
		/// The images to upload
		/// </summary>
		[DisplayName("Upload Images")]
		public List<TUploadedImage>? UploadImages { get; set; } = null;

		#endregion
	}
}