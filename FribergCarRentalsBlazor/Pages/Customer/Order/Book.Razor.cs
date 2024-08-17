using AutoMapper;
using FribergCarRentals.Shared.Constants;
using FribergCarRentals.Shared.Models.Dto.Order;
using FribergCarRentals.Shared.Models.ViewModels.Car;
using FribergCarRentals.Shared.Models.ViewModels.CarCategory;
using FribergCarRentals.Shared.Models.ViewModels.Message;
using FribergCarRentals.Shared.Models.ViewModels.Order;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.CustomerApi;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
namespace FribergCarRentalsBlazor.Pages.Customer.Order
{
    /// <summary>
    /// The page component class for searching rentable cars to book. 
    /// </summary>
    public partial class Book : CustomerPageComponentBase
    {
        #region Constants

        /// <summary>
        /// The key for storing the pending order to be confirmed by the customer.
        /// </summary>
        public const string PendingOrderTempDataKey = "CustomerOrderPendingOrder";

        #endregion

        #region Fields

        /// <summary>
        /// A collection of validation errors returned from the API.
        /// </summary>
        private List<MessageViewModel> _apiValidationErrors = new();

        #endregion

        #region Properties

        /// <summary>
        /// The injected Auto Mapper service. 
        /// </summary>
        [Inject]
        private IMapper AutoMapper { get; set; } = default!;

        /// <summary>
        /// The view model used for booking a car.
        /// </summary>
        private BookCarViewModel BookCarViewModel { get; set; } = new();

        /// <summary>
        /// The injected customer order API service.
        /// </summary>
        [Inject]
        private ICustomerOrderApiService CustomerOrderApiService { get; set; } = default!;

        #endregion

        #region Methods

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var result = await CustomerOrderApiService.GetCarCategoriesAsync();

            if (result.Success)
            {
                 BookCarViewModel.SetAvailableCarCategoryFilters(AutoMapper.Map<List<CarCategoryViewModel>>(result.Value!));
            }
            else
            {
                _apiValidationErrors = result.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList();
            }            
        }

        /// <summary>
        /// Prepares the booking by making sure that the customer is logged in before redirecting to the confirm page.
        /// </summary>
        /// <param name="car">The car the customer wants to </param>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        private async Task PrepareBooking(CarViewModel car)
        {
            CreateOrderViewModel newOrder = new CreateOrderViewModel(car, BookCarViewModel.PickupDateLocalTime, BookCarViewModel.ReturnDateLocalTime);
            await SessionStorageService.SetItemAsStringAsync(PendingOrderTempDataKey, JsonSerializer.Serialize(newOrder));

            if (await IsCustomerLoggedIn())
            {
                NavigationManager.NavigateTo("/customer/order/confirm");
            }
            else
            {
                await RedirectToLogin("/customer/order/confirm");
            }
        }

        /// <summary>
        /// Searches for rentable cars. 
        /// </summary>
        /// <returns>A <see cref="Task"/> representing an async operation.</returns>
        private async Task SearchCars()
        {
            if (!ValidatePickupDate(BookCarViewModel.PickupDateLocalTime))
            {
                _apiValidationErrors.Add(new MessageViewModel(MessageType.Error, ValidationMessages.PickupDateMustBeInFutureErrorMessage));
            }
            else if (!ValidateReturnDate(BookCarViewModel.PickupDateLocalTime, BookCarViewModel.ReturnDateLocalTime))
            {
                _apiValidationErrors.Add(new MessageViewModel(MessageType.Error, ValidationMessages.ReturnDateOccursBeforePickupDateErrorMessage));
            }
            else
            {
                var searchData = AutoMapper.Map<CarRentalSearchDto>(BookCarViewModel);

                if (searchData.SelectedCarCategoryFilter == BookCarViewModel.AllCarCategoriesValue)
                {
                    searchData.SelectedCarCategoryFilter = null;
                }

                var result = await CustomerOrderApiService.SearchRentableCarsAsync(searchData);

                if (result.Success)
                {
                    BookCarViewModel.AvailableCars = AutoMapper.Map<List<CarViewModel>>(result.Value!.AvailableCars);
                    BookCarViewModel.HavePerformedCarSearch = true;
                }
                else
                {
                    _apiValidationErrors.AddRange(result.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList());
                    BookCarViewModel.AvailableCars.Clear();
                }
            }
        }

        /// <summary>
		/// Validates the pickup date for car rentals.
		/// </summary>
		/// <param name="pickupDate">The pickup date.</param>
		/// <returns>True if the date is valid.</returns>
		private bool ValidatePickupDate(DateTime pickupDate)
        {
            return pickupDate.Date > DateTime.Now.Date;
        }

        /// <summary>
        /// Validates the return date for car rentals.
        /// </summary>
        /// <param name="pickupDate">The pickup date.</param>
        /// <param name="returnDate">The return date.</param>
        /// <returns>True if the date is valid.</returns>
        private bool ValidateReturnDate(DateTime pickupDate, DateTime returnDate)
        {
            return returnDate >= pickupDate;
        }

        #endregion
    }
}
