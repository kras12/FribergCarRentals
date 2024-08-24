using AutoMapper;
using FribergCarRentals.Shared.Models.Dto.Car;
using FribergCarRentals.Shared.Models.ViewModels.Car;
using FribergCarRentalsBlazor.ViewModels.Car;
using Microsoft.AspNetCore.Components.Forms;

namespace FribergCarRentalsBlazor.AutoMapper
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
            CreateMap<CarDto, EditCarViewModelBase<IBrowserFile>>();
            CreateMap<CarDto, EditCarViewModel>()
                .ForMember(dest => dest.SelectedCategoryId, opt => opt.MapFrom(src => src.Category.CarCategoryId));
        }

        #endregion
    }
}
