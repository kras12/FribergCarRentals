using AutoMapper;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Shared.Models.Mvc.ViewModels.Car;

namespace FribergCarRentals.Shared.Models.Mvc.AutoMapper
{
    /// <summary>
    /// An auto mapper profile that contains mappings for converting entity classes to view model classes.
    /// </summary>
    public class EntityToViewModelAutoMapperProfile : Profile
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public EntityToViewModelAutoMapperProfile()
        {
            CreateCarMappings();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates mappings for cars.
        /// </summary>
        private void CreateCarMappings()
        {
            CreateMap<CarEntity, EditCarViewModel>()
                .ForMember(dest => dest.SelectedPropulsionSystemId, opt => opt.MapFrom(src => src.PropulsionSystem!.VehiclePropulsionId))
                .ForMember(dest => dest.SelectedRentalStatusId, opt => opt.MapFrom(src => src.RentalStatus!.CarRentalStatusId));
        }

        #endregion
    }
}
