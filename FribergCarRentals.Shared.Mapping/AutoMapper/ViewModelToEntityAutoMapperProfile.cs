using AutoMapper;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Data.Entities;
using FribergCarRentals.Shared.Models.ViewModels.CarCategory;
using FribergCarRentals.Shared.Models.ViewModels.Customer;
using FribergCarRentals.Shared.Models.ViewModels.Car;
using FribergCarRentals.Shared.Models.ViewModels.Image;
using FribergCarRentals.Shared.Models.ViewModels.Admin;
using FribergCarRentals.Shared.Models.ViewModels.Order;
using FribergCarRentals.Data.DTO;
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
			CreateMappingsForAdmins();
			CreateMappingsForCars();
			CreateMappingsForCarCategories();
			CreateMappingsForCustomers();
			CreateMappingsForCarImages();
			CreateMappingsForOrders();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Creates mappings for admins.
		/// </summary>
		public void CreateMappingsForAdmins()
		{
			CreateMap<AdminViewModel, AdminEntity>();
		}

		/// <summary>
		/// Creates mappings for car categories.
		/// </summary>
		public void CreateMappingsForCarCategories()
		{
			CreateMap<CarCategoryViewModel, CarCategoryEntity>();
			CreateMap<CarCategoryViewModel, CarCategoryCountDto>();
			CreateMap<CreateCarCategoryViewModel, CarCategoryEntity>();
			CreateMap<EditCarCategoryViewModel, CarCategoryEntity>();
		}

		/// <summary>
		/// Creates mappings for car images.
		/// </summary>
		public void CreateMappingsForCarImages()
		{
			CreateMap<ImageViewModel, ImageEntity>();
		}

		/// <summary>
		/// Creates mappings for cars.
		/// </summary>
		public void CreateMappingsForCars()
		{
			CreateMap<CarViewModel, CarEntity>();
			CreateMap<CreateCarViewModel, CarEntity>();
			CreateMap<EditCarViewModel, CarEntity>();
		}

		/// <summary>
		/// Creates mappings for customers.
		/// </summary>
		public void CreateMappingsForCustomers()
		{
			CreateMap<CustomerViewModel, CustomerEntity>();
			CreateMap<EditCustomerViewModel, CustomerEntity>();
			CreateMap<RegisterCustomerViewModel, ApplicationUser>();
			CreateMap<EditCustomerViewModel, ApplicationUser>();
		}

		/// <summary>
		/// Creates mappings for orders.
		/// </summary>
		public void CreateMappingsForOrders()
		{
			CreateMap<OrderViewModel, CarOrderEntity>();
		}

		#endregion
	}
}
