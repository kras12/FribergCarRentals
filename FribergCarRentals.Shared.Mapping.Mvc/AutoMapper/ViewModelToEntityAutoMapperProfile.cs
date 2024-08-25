using AutoMapper;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Shared.Models.Mvc.ViewModels.Car;

namespace FribergCarRentals.Shared.Mapping.Mvc.AutoMapper
{
    /// <summary>
    /// An auto mapper profile that contains mappings for converting view model classes to entity classes.
    /// </summary>
    public class ViewModelToEntityAutoMapperProfile : Profile
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public ViewModelToEntityAutoMapperProfile()
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
            CreateMap<CreateCarViewModel, CarEntity>()
                .ForMember(dest => dest.PropulsionSystem, opt => opt.MapFrom(src => VehiclePropulsionEntity.CreateFromId(src.SelectedPropulsionSystemId)))
                .ForMember(dest => dest.RentalStatus, opt => opt.MapFrom(src => CarRentalStatusEntity.CreateFromId(src.SelectedRentalStatusId)));

            CreateMap<EditCarViewModel, CarEntity>()
                .ForMember(dest => dest.PropulsionSystem, opt => opt.MapFrom(src => VehiclePropulsionEntity.CreateFromId(src.SelectedPropulsionSystemId)))
                .ForMember(dest => dest.RentalStatus, opt => opt.MapFrom(src => CarRentalStatusEntity.CreateFromId(src.SelectedRentalStatusId)));
        }

        #endregion
    }
}
