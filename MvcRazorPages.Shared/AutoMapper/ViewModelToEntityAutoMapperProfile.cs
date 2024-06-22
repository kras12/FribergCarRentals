using AutoMapper;
using FribergFastigheter.Server.Data.Entities;
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
        }
	}
}
