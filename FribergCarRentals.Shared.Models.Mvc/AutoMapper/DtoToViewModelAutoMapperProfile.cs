using AutoMapper;
using FribergCarRentals.Data.DTO;
using FribergCarRentals.Shared.Models.ViewModels.CarCategory;

namespace FribergCarRentals.Shared.Models.Mvc.AutoMapper
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
            CreateMappingsForCars();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates mappings for cars.
        /// </summary>
        public void CreateMappingsForCars()
        {
            CreateMap<CarCategoryCountDto, CarCategoryViewModel>();
        }

        #endregion
    }
}
