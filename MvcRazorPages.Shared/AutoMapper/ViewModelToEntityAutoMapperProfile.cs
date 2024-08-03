using AutoMapper;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Data.Entities;
using MvcRazorPages.Shared.ViewModels.Car;
using MvcRazorPages.Shared.ViewModels.CarCategory;
using MvcRazorPages.Shared.ViewModels.Customer;

namespace MvcRazorPages.Shared.AutoMapper
{
    /// <summary>
    /// An auto mapper profile that contains mappings for converting view model classes to entity classes.
    /// </summary>
    public class ViewModelToEntityAutoMapperProfile : Profile
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public ViewModelToEntityAutoMapperProfile()
		{
            CreateMap<CreateCarCategoryViewModel, CarCategoryEntity>();
            CreateMap<EditCarCategoryViewModel, CarCategoryEntity>();
			CreateMap<CreateCarViewModel, CarEntity>();
			CreateMap<EditCustomerViewModel, CustomerEntity>();
            CreateMap<RegisterCustomerViewModel, ApplicationUser>();
            CreateMap<EditCustomerViewModel, ApplicationUser>();
            CreateMap<EditCarViewModel, CarEntity>();
        }
	}
}
