using AutoMapper;
using FribergCarRentals.Shared.Models.ViewModels.Car;
using FribergCarRentals.Shared.Models.ViewModels.Message;
using FribergCarRentals.Shared.Models.ViewModels.Other;
using FribergCarRentalsBlazor.Components.Admin;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.AdminApi;
using Microsoft.AspNetCore.Components;

namespace FribergCarRentalsBlazor.Pages.Admin.Car
{
    /// <summary>
    /// The page component class for listing cars.
    /// </summary>
    public partial class ListCars : AdminPageComponentBase
    {
        #region Constants

        // TODO - Find a better solution for storing this ID, as this page does not have the primary responsibility for deleting a car. 
        /// <summary>
        /// The key for the ID of the car that was created. 
        /// </summary>
        public const string CreatedCarIdStorageDataKey = "AdminCreatedCarId";

        // TODO - Find a better solution for storing this ID, as this page does not have the primary responsibility for deleting a car. 
        /// <summary>
        /// The key for the ID of the car that was deleted. 
        /// </summary>
        public const string DeletedCarIdStorageDataKey = "AdminDeletedCarId";

		/// <summary>
		/// The url template for the page. 
		/// </summary>
		private const string PageUrlTemplate = "/admin/car/list";

        #endregion

        #region Fields

        /// <summary>
        /// A collection of validation errors returned from the API.
        /// </summary>
        private List<MessageViewModel> _apiValidationErrors = new();

        /// <summary>
        /// The cars to list on the page. 
        /// </summary>
        private ListViewModel<CarViewModel> _cars = default!;

        #endregion

        #region Properties

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

		#endregion

		#region Methods

		/// <summary>
		/// Gets the page URL.
		/// </summary>
		/// <returns>A <see cref="string"/> that contains the URL of the page.</returns>
		public static string GetPageUrl()
		{
			return PageUrlTemplate;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		protected override async Task OnInitializedAsync()
        {
            var result = await AdminCarApiService.GetCarsAsync();

            if (result.Success)
            {
                _cars = new ListViewModel<CarViewModel>(AutoMapper.Map<List<CarViewModel>>(result.Value!));

                if (await SessionStorageService.ContainKeyAsync(CreatedCarIdStorageDataKey))
                {
                    var carId = await SessionStorageService.GetItemAsync<int>(CreatedCarIdStorageDataKey);
                    await SessionStorageService.RemoveItemAsync(CreatedCarIdStorageDataKey);
                    _cars.Messages.Add(MessageViewModelHelper.CreateCarCreationSuccessMessage(carId));
                }

                if (await SessionStorageService.ContainKeyAsync(DeletedCarIdStorageDataKey))
                {
                    var carId = await SessionStorageService.GetItemAsync<int>(DeletedCarIdStorageDataKey);
                    await SessionStorageService.RemoveItemAsync(DeletedCarIdStorageDataKey);
                    _cars.Messages.Add(MessageViewModelHelper.CreateCarDeletionSuccessMessage(carId));
                }
            }
            else
            {
                _apiValidationErrors = result.Errors.Select(x => new MessageViewModel(MessageType.Error, x.ErrorMessage, title: x.ErrorType)).ToList();
            }
        }

        /// <summary>
        /// Event callback for a delete car operation.
        /// </summary>
        /// <param name="result">The result of the operation</param>
        /// <returns>A <see cref="Task"/> that represents an async operation.</returns>
        private void OnDeletedCar(DeleteCarEventCallbackArgs result)
        {
            if (result.ApiResponse.Success)
            {
                _cars.Messages.Add(MessageViewModelHelper.CreateCarDeletionSuccessMessage(result.Car.CarId));
                _cars.Models.Remove(result.Car);
            }
            else
            {
                _apiValidationErrors = result.ApiResponse.Errors.Select(x => new MessageViewModel(MessageType.Error, x.ErrorMessage, title: x.ErrorType)).ToList();
            }
        }

        #endregion
    }
}
