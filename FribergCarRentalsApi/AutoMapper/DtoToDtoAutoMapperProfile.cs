using AutoMapper;
using FribergCarRentals.Data.DTO;
using FribergCarRentals.Shared.Dto.CarCategory;

namespace FribergCarRentalsApi.AutoMapper
{
    /// <summary>
    /// An auto mapper profile that contains mappings for converting DTO classes to DTO classes.
    /// </summary>
    public class DtoToDtoAutoMapperProfile : Profile
    {
        #region Constructors
        
        /// <summary>
        /// Constructor.
        /// </summary>
        public DtoToDtoAutoMapperProfile()
        {
            CreateCarCategoryMappins();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates mappings for car categories.
        /// </summary>
        private void CreateCarCategoryMappins()
        {
            CreateMap<CarCategoryCountDto, CarCategoryStatisticsDto>()
                .ForMember(dest => dest.CarCategory, opt => opt.MapFrom(src => src.CarCategoryEntity));
        }

        #endregion
    }
}
