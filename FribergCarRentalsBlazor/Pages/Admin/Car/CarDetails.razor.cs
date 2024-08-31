using AutoMapper;
using FribergCarRentals.Shared.Models.ViewModels.Car;
using FribergCarRentals.Shared.Models.ViewModels.Message;
using FribergCarRentalsBlazor.Components.Admin;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.AdminApi;
using Microsoft.AspNetCore.Components;

namespace FribergCarRentalsBlazor.Pages.Admin.Car
{
    /// <summary>
    /// The page component class for showing car details.
    /// </summary>
    public partial class CarDetails : AdminPageComponentBase
    {
        #region Constants

        /// <summary>
        /// The base URL for the page without the car ID.
        /// </summary>
        private const string PageUrlBase = "/admin/car/details";

		/// <summary>
		/// The url template for the page. 
		/// </summary>
		private const string PageUrlTemplate = PageUrlBase + "/{CarId:int}";

        #endregion

        #region Fields

        /// <summary>
        /// A collection of validation errors returned from the API.
        /// </summary>
        private List<MessageViewModel> _apiValidationErrors = new();

        #endregion

        #region Properties

        /// <summary>
        /// The ID of the car. 
        /// </summary>
        [Parameter]
        public int CarId { get; set; }

        /// <summary>
        /// The injected admin car API service.
        /// </summary>
        [Inject]
        private IAdminCarApiService AdminCarApiService { get; set; } = default!;

        /// <summary>
        /// The injected Auto Mapper service. 
        /// </summary>
        [Inject]
        private IMapper AutoMapper { get; set; } = default!;

        /// <summary>
        /// The viewmodel for the car to show.
        /// </summary>
        private CarViewModel? Car { get; set; } = null;

        #endregion

        #region Methods

        /// <summary>
        /// Event callback for a delete car operation.
        /// </summary>
        /// <param name="result">The result of the operation</param>
        /// <returns>A <see cref="Task"/> that represents an async operation.</returns>
        private async Task OnDeleteCar(DeleteCarEventCallbackArgs result)
        {
            if (result.ApiResponse.Success)
            {
                await SessionStorageService.SetItemAsStringAsync(ListCars.DeletedCarIdStorageDataKey, Car!.CarId.ToString());
                NavigationManager.NavigateTo(ListCars.GetPageUrl());
            }
            else
            {
                _apiValidationErrors = result.ApiResponse.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList();
            }
        }

        /// <summary>
        /// Gets the page URL for a car.
        /// </summary>
        /// <param name="carId">The ID of the car.</param>
        /// <returns>A <see cref="string"/> that contains the URL of the page.</returns>
        public static string GetPageUrl(int carId)
        {
            return $"{PageUrlBase}/{carId}";
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var result = await AdminCarApiService.GetCarByIdAsync(CarId);

            if (result.Success)
            {
                Car = AutoMapper.Map<CarViewModel>(result.Value!);
            }
            else
            {
                _apiValidationErrors = result.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList();
            }
        }

        #endregion
    }
}
