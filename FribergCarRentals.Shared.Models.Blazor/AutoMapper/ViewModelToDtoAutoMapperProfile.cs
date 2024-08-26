using AutoMapper;
using FribergCarRentals.Shared.Models.Blazor.ViewModels.Car;
using FribergCarRentals.Shared.Models.Dto.Car;

namespace FribergCarRentals.Shared.Models.Blazor.AutoMapper
{
    /// <summary>
    /// An auto mapper profile that contains mappings for converting view model classes to DTO classes.
    /// </summary>
    public class ViewModelToDtoAutoMapperProfile : Profile
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public ViewModelToDtoAutoMapperProfile()
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
            CreateMap<CreateCarViewModel, CreateCarDto>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.SelectedCategoryId))
                .ForMember(dest => dest.RentalStatusId, opt => opt.MapFrom(src => src.SelectedRentalStatusId))
                .ForMember(dest => dest.PropulsionSystemId, opt => opt.MapFrom(src => src.SelectedPropulsionSystemId));

            CreateMap<EditCarViewModel, EditCarDto>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.SelectedCategoryId))
                .ForMember(dest => dest.RentalStatusId, opt => opt.MapFrom(src => src.SelectedRentalStatusId))
                .ForMember(dest => dest.PropulsionSystemId, opt => opt.MapFrom(src => src.SelectedPropulsionSystemId));
        }

        #endregion
    }
}
