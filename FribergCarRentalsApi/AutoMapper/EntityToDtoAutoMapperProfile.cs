using AutoMapper;
using FribergCarRentals.Data.Entities;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Shared.Models.Dto.Admin;
using FribergCarRentals.Shared.Models.Dto.Car;
using FribergCarRentals.Shared.Models.Dto.CarCategory;
using FribergCarRentals.Shared.Models.Dto.Customer;
using FribergCarRentals.Shared.Models.Dto.Image;
using FribergCarRentals.Shared.Models.Dto.Order;
using FribergCarRentals.Shared.Models.Dto.User;

namespace FribergCarRentalsApi.AutoMapper
{
    /// <summary>
    /// An auto mapper profile that contains mappings for converting entity classes to DTO classes.
    /// </summary>
    public class EntityToDtoAutoMapperProfile : Profile
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public EntityToDtoAutoMapperProfile()
        {
            CreateCarMappings();
            CreateCustomerMappings();
            CreateOrderMappings();
            CreateUserMappings();
            CreateAdminMappings();
        }

        #endregion

        #region Methods

        private void CreateAdminMappings()
        {
            CreateMap<AdminEntity, AdminDto>();
        }

        /// <summary>
        /// Creates mappings for cars.
        /// </summary>
        private void CreateCarMappings()
        {
            CreateMap<CarCategoryEntity, CarCategoryDto>();
            CreateMap<CarCategoryEntity, CarCategoryStatisticsDto>();
            CreateMap<VehiclePropulsionEntity, VehiclePropulsionDto>();
            CreateMap<CarRentalStatusEntity, CarRentalStatusDto>();
            CreateMap<CarEntity, CarDto>();

            CreateMap<ImageEntity, CarImageDto>()
                .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.Car.CarId));
        }

        /// <summary>
        /// Creates mappings for customers.
        /// </summary>
        private void CreateCustomerMappings()
        {
            CreateMap<ApplicationUser, UserDto>();
            CreateMap<ApplicationUser, CustomerDto>();
            CreateMap<ApplicationUser, CreatedCustomerDto>();
            CreateMap<ApplicationUser, CarOrderCustomerDto>();

            CreateMap<CustomerEntity, UserDto>()
                .IncludeMembers(src => src.User);

            CreateMap<CustomerEntity, CustomerDto>()
                .IncludeMembers(src => src.User);

            CreateMap<CustomerEntity, CreatedCustomerDto>()
                .IncludeMembers(src => src.User);

            CreateMap<CustomerEntity, CarOrderCustomerDto>()
                .IncludeMembers(src => src.User);
        }

        /// <summary>
        /// Creates mappings for orders.
        /// </summary>
        private void CreateOrderMappings()
        {
            CreateMap<OrderStatusEntity, OrderStatusDto>();

            CreateMap<CarOrderEntity, CarOrderDto>()
                .ForMember(dest => dest.CarBooking, opt => opt.MapFrom(src => src.CarBookings.First()));

            CreateMap<CarBookingEntity, CarBookingDto>()
                .ForMember(dest => dest.CarOrderId, opt => opt.MapFrom(src => src.CarOrder!.CarOrderId));
        }

        /// <summary>
        /// Creates mappings for users.
        /// </summary>
        private void CreateUserMappings()
        {
            CreateMap<ApplicationUser, UserDto>();
        }

        #endregion
    }
}
