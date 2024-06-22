using AutoMapper;
using FribergCarRentals.Data.EntityClasses;
using FribergFastigheter.Server.Data.Entities;
using MvcRazorPages.Shared.ViewModels.Car;
using MvcRazorPages.Shared.ViewModels.CarCategory;
using MvcRazorPages.Shared.ViewModels.Customer;
namespace FribergFastigheter.Server.AutoMapper
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
			CreateMap<RegisterCustomerViewModel, ApplicationUser>();
            CreateMap<CreateCarCategoryViewModel, CarCategoryEntity>();
            CreateMap<EditCarCategoryViewModel, CarCategoryEntity>();
			CreateMap<CreateCarViewModel, CarEntity>();
			CreateMap<EditCustomerViewModel, CustomerEntity>();
            CreateMap<EditCustomerViewModel, ApplicationUser>();
        }
	}
}
