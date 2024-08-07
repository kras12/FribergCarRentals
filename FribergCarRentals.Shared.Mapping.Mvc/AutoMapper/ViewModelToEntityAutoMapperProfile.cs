using AutoMapper;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Shared.Models.Mvc.ViewModels.Car;

namespace FribergCarRentals.Shared.Mapping.AutoMapper
{
	/// <summary>
	/// An auto mapper profile that contains mappings for converting view model classes to entity classes.
	/// </summary>
	public class ViewModelToEntityAutoMapperProfile : Profile
	{
		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		public ViewModelToEntityAutoMapperProfile()
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
			CreateMap<CreateCarViewModel, CarEntity>();
			CreateMap<EditCarViewModel, CarEntity>();
		}

		#endregion
	}
}
