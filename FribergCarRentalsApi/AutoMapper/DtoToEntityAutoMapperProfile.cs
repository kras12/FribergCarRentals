using AutoMapper;
using FribergCarRentals.Data.Entities;
using FribergCarRentals.Shared.Dto.Customer;

namespace FribergCarRentalsApi.AutoMapper
{
    /// <summary>
    /// An auto mapper profile that contains mappings for converting DTO classes to entity classes.
    /// </summary>
    public class DtoToEntityAutoMapperProfile : Profile
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public DtoToEntityAutoMapperProfile()
        {
            CreateMap<CreateCustomerDto, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
        }
    }
}
