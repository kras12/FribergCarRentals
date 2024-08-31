using AutoMapper;
using FribergCarRentals.Data.DTO;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Shared.Models.Dto.CarCategory;

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
            CreateCarCategoryMappings();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates mappings for car categories.
        /// </summary>
        private void CreateCarCategoryMappings()
        {
            CreateMap<CarCategoryCountDto, CarCategoryStatisticsDto>()
                .IncludeMembers(src => src.CarCategoryEntity);
        }

        #endregion
    }
}
