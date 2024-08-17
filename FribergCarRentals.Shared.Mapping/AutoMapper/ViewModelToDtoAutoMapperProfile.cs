using AutoMapper;
using FribergCarRentals.Shared.Models.ViewModels.Customer;
using FribergCarRentals.Shared.Models.Dto.Customer;
using FribergCarRentals.Shared.Models.ViewModels.Order;
using FribergCarRentals.Shared.Models.Dto.Order;

namespace FribergCarRentals.Shared.Mapping.AutoMapper
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
			CreateMappingsForCustomers();
			CreateMappingsForCarOrders();
        }

        #endregion

        #region Methods

		/// <summary>
		/// Creates mappings for cars.
		/// </summary>
        public void CreateMappingsForCars()
		{
			CreateMap<BookCarViewModel, CarRentalSearchDto>();
        }

        /// <summary>
        /// Creates mappings for customers.
        /// </summary>
        public void CreateMappingsForCustomers()
		{
			CreateMap<LoginCustomerViewModel, LoginCustomerDto>();
			CreateMap<RegisterCustomerViewModel, CreateCustomerDto>();
		}

        /// <summary>
        /// Creates mappings for car orders.
        /// </summary>
        public void CreateMappingsForCarOrders()
        {
			CreateMap<CreateOrderViewModel, CreateCarOrderDto>();
        }

        #endregion
    }
}
