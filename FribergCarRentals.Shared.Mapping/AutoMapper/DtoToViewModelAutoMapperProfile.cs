using AutoMapper;
using FribergCarRentals.Shared.Models.Dto.Car;
using FribergCarRentals.Shared.Models.Dto.CarCategory;
using FribergCarRentals.Shared.Models.Dto.Image;
using FribergCarRentals.Shared.Models.ViewModels.Car;
using FribergCarRentals.Shared.Models.ViewModels.CarCategory;
using FribergCarRentals.Shared.Models.ViewModels.Image;

namespace FribergCarRentals.Shared.Mapping.AutoMapper
{
	/// <summary>
	/// An auto mapper profile that contains mappings for converting DTO classes to view model classes.
	/// </summary>
	public class DtoToViewModelAutoMapperProfile : Profile
	{
		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		public DtoToViewModelAutoMapperProfile()
		{
			CreateMappingsForCars();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Creates mappings for cars.
		/// </summary>
		public void CreateMappingsForCars()
		{
			CreateMap<CarCategoryDto, CarCategoryViewModel>();
			CreateMap<CarDto, CarViewModel>();
			CreateMap<VehiclePropulsionDto, VehiclePropulsionViewModel>();
			CreateMap<CarRentalStatusDto, CarRentalStatusViewModel>();
			CreateMap<CarImageDto, ImageViewModel>();
		}

		#endregion
	}
}
