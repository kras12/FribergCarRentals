using AutoMapper;
using FribergCarRentals.Shared.Models.ViewModels.Customer;
using FribergCarRentals.Shared.Models.Dto.Customer;

namespace FribergCarRentals.Shared.Mapping.AutoMapper
{
	/// <summary>
	/// An auto mapper profile that contains mappings for converting view model classes to DTO classes.
	/// </summary>
	public class ViewModelToDtoAutoMapperProfile : Profile
	{
		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		public ViewModelToDtoAutoMapperProfile()
		{
			CreateMappingsForCustomers();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Creates mappings for customers.
		/// </summary>
		public void CreateMappingsForCustomers()
		{
			CreateMap<LoginCustomerViewModel, LoginCustomerDto>();
		}

		#endregion
	}
}
