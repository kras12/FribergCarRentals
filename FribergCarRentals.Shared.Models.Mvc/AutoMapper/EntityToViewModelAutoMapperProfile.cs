using AutoMapper;
using FribergCarRentals.Data.Entities;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Shared.Models.Mvc.ViewModels.Car;
using FribergCarRentals.Shared.Models.ViewModels.Admin;
using FribergCarRentals.Shared.Models.ViewModels.Car;
using FribergCarRentals.Shared.Models.ViewModels.CarCategory;
using FribergCarRentals.Shared.Models.ViewModels.Customer;
using FribergCarRentals.Shared.Models.ViewModels.Image;
using FribergCarRentals.Shared.Models.ViewModels.Order;

namespace FribergCarRentals.Shared.Models.Mvc.AutoMapper
{
    /// <summary>
    /// An auto mapper profile that contains mappings for converting entity classes to view model classes.
    /// </summary>
    public class EntityToViewModelAutoMapperProfile : Profile
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public EntityToViewModelAutoMapperProfile()
        {
            CreateCarMappings();
            CreateCustomerMappings();
            CreateOrderMappings();
            CreateAdminMappings();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates mappings for admins. 
        /// </summary>
        private void CreateAdminMappings()
        {
            CreateMap<ApplicationUser, AdminViewModel>();
            CreateMap<AdminEntity, AdminViewModel>()
                .IncludeMembers(src => src.User);
        }

        /// <summary>
        /// Creates mappings for cars.
        /// </summary>
        private void CreateCarMappings()
        {
            CreateMap<CarCategoryEntity, CarCategoryViewModel>();
			CreateMap<CarCategoryEntity, EditCarCategoryViewModel>();
			CreateMap<VehiclePropulsionEntity, VehiclePropulsionViewModel>();
            CreateMap<CarRentalStatusEntity, CarRentalStatusViewModel>();
            CreateMap<ImageEntity, ImageViewModel>();
            CreateMap<CarEntity, CarViewModel>();

            CreateMap<CarEntity, EditCarViewModel>()
                .ForMember(dest => dest.SelectedPropulsionSystemId, opt => opt.MapFrom(src => src.PropulsionSystem!.VehiclePropulsionId))
                .ForMember(dest => dest.SelectedRentalStatusId, opt => opt.MapFrom(src => src.RentalStatus!.CarRentalStatusId));
        }

        /// <summary>
        /// Creates mappings for customers.
        /// </summary>
        private void CreateCustomerMappings()
        {
            CreateMap<ApplicationUser, CustomerViewModel>();
            CreateMap<ApplicationUser, EditCustomerViewModel>();
            CreateMap<ApplicationUser, CarOrderCustomerViewModel>();

            CreateMap<CustomerEntity, EditCustomerViewModel>()
                .IncludeMembers(src => src.User)
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.CustomerId));

            CreateMap<CustomerEntity, CustomerViewModel>()
                .IncludeMembers(src => src.User)
                .ForMember(dest => dest.OrderCount, opt => opt.MapFrom(src => src.Orders.Count));

            CreateMap<CustomerEntity, CarOrderCustomerViewModel>()
                .IncludeMembers(src => src.User);
        }

        /// <summary>
        /// Creates mappings for orders.
        /// </summary>
        private void CreateOrderMappings()
        {
            CreateMap<OrderStatusEntity, OrderStatusViewModel>();

            CreateMap<CarBookingEntity, CarBookingViewModel>()
                .ForMember(dest => dest.CarPickupDate, opt => opt.MapFrom(src => src.PickupDateUtc.ToLocalTime()))
                .ForMember(dest => dest.CarReturnDate, opt => opt.MapFrom(src => src.ReturnDateUtc.ToLocalTime()));

            CreateMap<CarOrderEntity, OrderViewModel>()
                .ForMember(dest => dest.CarBooking, opt => opt.MapFrom(src => src.CarBookings.First()));
        }

        #endregion
    }
}
