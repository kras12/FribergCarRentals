using AutoMapper;
using FribergCarRentals.Shared.Models.Dto.Admin;
using FribergCarRentals.Shared.Models.Dto.Car;
using FribergCarRentals.Shared.Models.Dto.CarCategory;
using FribergCarRentals.Shared.Models.Dto.Customer;
using FribergCarRentals.Shared.Models.Dto.Image;
using FribergCarRentals.Shared.Models.Dto.Order;
using FribergCarRentals.Shared.Models.ViewModels.Admin;
using FribergCarRentals.Shared.Models.ViewModels.Car;
using FribergCarRentals.Shared.Models.ViewModels.CarCategory;
using FribergCarRentals.Shared.Models.ViewModels.Customer;
using FribergCarRentals.Shared.Models.ViewModels.Image;
using FribergCarRentals.Shared.Models.ViewModels.Order;

namespace FribergCarRentals.Shared.Models.AutoMapper
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
            CreateMappingsForAdmins();
            CreateMappingsForCarOrders();
            CreateMappingsForCustomers();
            CreateMappingsForCars();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates mappings for admins.
        /// </summary>
        public void CreateMappingsForAdmins()
        {
            CreateMap<AdminDto, AdminViewModel>();
        }

        /// <summary>
        /// Creates mappings for car orders.
        /// </summary>
        public void CreateMappingsForCarOrders()
        {
            CreateMap<OrderStatusDto, OrderStatusViewModel>();
            CreateMap<CarOrderDto, OrderViewModel>();

            CreateMap<CarBookingDto, CarBookingViewModel>()
                .ForMember(dest => dest.CarPickupDate, opt => opt.MapFrom(src => src.PickupDateUtc))
                .ForMember(dest => dest.CarReturnDate, opt => opt.MapFrom(src => src.ReturnDateUtc));
        }

        /// <summary>
        /// Creates mappings for cars.
        /// </summary>
        public void CreateMappingsForCars()
        {
            CreateMap<CarCategoryDto, CarCategoryViewModel>();
            CreateMap<CarCategoryDto, EditCarCategoryViewModel>();
            CreateMap<EditCarCategoryDto, CarCategoryViewModel>();
            CreateMap<CarCategoryStatisticsDto, CarCategoryViewModel>();
            CreateMap<CarCategoryStatisticsDto, EditCarCategoryViewModel>();
            CreateMap<CarDto, CarViewModel>();
            CreateMap<VehiclePropulsionDto, VehiclePropulsionViewModel>();
            CreateMap<CarRentalStatusDto, CarRentalStatusViewModel>();
            CreateMap<CarImageDto, ImageViewModel>();
            CreateMap<CreateCarCategoryDto, CreateCarCategoryViewModel>();
        }

        /// <summary>
        /// Creates mappings for customers.
        /// </summary>
        public void CreateMappingsForCustomers()
        {
            CreateMap<CustomerDto, CustomerViewModel>();
            CreateMap<CarOrderCustomerDto, CarOrderCustomerViewModel>();

            CreateMap<CustomerDto, EditCustomerViewModel>()
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.CustomerId));
        }

        #endregion
    }
}
