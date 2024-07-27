using AutoMapper;
using FribergCarRentals.Data.Entities;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Shared.Dto.Customer;
using FribergCarRentals.Shared.Dto.User;

namespace FribergCarRentalsApi.AutoMapper
{
    /// <summary>
    /// An auto mapper profile that contains mappings for converting entity classes to DTO classes.
    /// </summary>
    public class EntityToDtoAutoMapperProfile : Profile
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public EntityToDtoAutoMapperProfile()
        {
            CreateMap<CustomerEntity, CustomerDto>();
            CreateMap<CustomerEntity, CreatedCustomerDto>();
            CreateMap<ApplicationUser, ApplicationUserDto>();
        }
    }
}
