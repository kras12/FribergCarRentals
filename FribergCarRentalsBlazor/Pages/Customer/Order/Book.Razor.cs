using AutoMapper;
using Blazored.SessionStorage;
using FribergCarRentals.Shared.Constants;
using FribergCarRentals.Shared.Models.Dto.Order;
using FribergCarRentals.Shared.Models.ViewModels.Car;
using FribergCarRentals.Shared.Models.ViewModels.CarCategory;
using FribergCarRentals.Shared.Models.ViewModels.Order;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Text.Json;

namespace FribergCarRentalsBlazor.Pages.Customer.Order
{
    public partial class Book : ComponentBase
    {
        #region Constants

        /// <summary>
        /// The storage key for data used to create a car booking. 
        /// </summary>
        public const string PrepareCarBookingStorageKey = "PrepareCarBookingStorageKey";

        #endregion

        #region Fields

        /// <summary>
        /// A collection of validation errors returned from the API.
        /// </summary>
        private List<string> _apiValidationErrors = new List<string>();

        #endregion

        #region Properties

        /// <summary>
        /// The cascaded authentication state task.
        /// </summary>
        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; } = default!;

        /// <summary>
        /// The injected authorization service.
        /// </summary>
        [Inject]
        private IAuthorizationService AuthorizationService { get; set; } = default!;

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

        /// <summary>
        /// The injected navigation manager. 
        /// </summary>
        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        /// <summary>
        /// The injected session storage service.
        /// </summary>
        [Inject]
        private ISessionStorageService SessionStorageService { get; set; } = default!;

        #endregion

        #region Methods

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var result = await CustomerOrderApiService.GetCarCategories();

            if (result.Success)
            {
                 BookCarViewModel.SetAvailableCarCategoryFilters(AutoMapper.Map<List<CarCategoryViewModel>>(result.Value!));
            }
            else
            {
                _apiValidationErrors.AddRange(result.Errors.Select(x => $"{x.Key}: {x.Value}").ToList());
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
            await SessionStorageService.SetItemAsStringAsync(PrepareCarBookingStorageKey, JsonSerializer.Serialize(newOrder));
            var authResult = await AuthorizationService.AuthorizeAsync((await AuthenticationStateTask).User, ApplicationUserPolicies.Customer);

            if (authResult.Succeeded)
            {
                NavigationManager.NavigateTo("/customer/order/confirm");
            }
            else
            {
                await SessionStorageService.SetItemAsStringAsync(Authenticate.RedirectUrlStorageKey, "/customer/order/confirm");
                NavigationManager.NavigateTo("/customer/login");
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
                _apiValidationErrors.Add(ValidationMessages.PickupDateMustBeInFutureErrorMessage);
            }
            else if (!ValidateReturnDate(BookCarViewModel.PickupDateLocalTime, BookCarViewModel.ReturnDateLocalTime))
            {
                _apiValidationErrors.Add(ValidationMessages.ReturnDateOccursBeforePickupDateErrorMessage);
            }
            else
            {
                var searchData = AutoMapper.Map<CarRentalSearchDto>(BookCarViewModel);

                if (searchData.SelectedCarCategoryFilter == BookCarViewModel.AllCarCategoriesValue)
                {
                    searchData.SelectedCarCategoryFilter = null;
                }

                var result = await CustomerOrderApiService.SearchRentableCars(searchData);

                if (result.Success)
                {
                    BookCarViewModel.AvailableCars = AutoMapper.Map<List<CarViewModel>>(result.Value!.AvailableCars);
                    BookCarViewModel.HavePerformedCarSearch = true;
                }
                else
                {
                    _apiValidationErrors.AddRange(result.Errors.Select(x => $"{x.Key}: {x.Value}").ToList());
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
