using AutoMapper;
using FribergCarRentals.Data.Entities;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Data.Types;
using FribergCarRentals.Shared.Dto.Car;
using FribergCarRentals.Shared.Dto.Customer;

namespace FribergCarRentalsApi.AutoMapper
{
    /// <summary>
    /// An auto mapper profile that contains mappings for converting DTO classes to entity classes.
    /// </summary>
    public class DtoToEntityAutoMapperProfile : Profile
    {
        #region Constructors
        
        /// <summary>
        /// Constructor.
        /// </summary>
        public DtoToEntityAutoMapperProfile()
        {
            CreateCustomerMappings();
            CreateCarCategoryMappings();
            CreateCarMappings();
        }

        #endregion

        #region Methods

        private void CreateCarMappings()
        {
            CreateMap<CreateCarDto, CarEntity>()
                .ForMember(dest => dest.PropulsionSystem, opt =>
                    opt.MapFrom(src => VehiclePropulsionEntity.CreateFromType((VehiclePropulsionType)src.PropulsionSystemId)))
                .ForMember(dest => dest.RentalStatus, opt =>
                    opt.MapFrom(src => CarRentalStatusEntity.CreateFromType((RentalCarStatus)src.RentalStatusId)));

            CreateMap<EditCarDto, CarEntity>()
                .ForMember(dest => dest.PropulsionSystem, opt =>
                    opt.MapFrom(src => VehiclePropulsionEntity.CreateFromType((VehiclePropulsionType)src.PropulsionSystemId)))
                .ForMember(dest => dest.RentalStatus, opt =>
                    opt.MapFrom(src => CarRentalStatusEntity.CreateFromType((RentalCarStatus)src.RentalStatusId)));
        }

        /// <summary>
        /// Creates mappings for car categories.
        /// </summary>
        private void CreateCarCategoryMappings()
        {
            CreateMap<CreateCarCategoryDto, CarCategoryEntity>();
            CreateMap<EditCarCategoryDto, CarCategoryEntity>();
        }

        /// <summary>
        /// Creates mappings for customers.
        /// </summary>
        private void CreateCustomerMappings()
        {
            CreateMap<CreateCustomerDto, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            CreateMap<EditCustomerDto, ApplicationUser>();
        }

        #endregion
    }
}
