using AutoMapper;
using FribergCarRentals.Shared.Helpers;
using FribergCarRentals.Shared.Models.Dto.Car;
using FribergCarRentals.Shared.Models.ViewModels.Car;
using FribergCarRentals.Shared.Models.ViewModels.CarCategory;
using FribergCarRentals.Shared.Models.ViewModels.Image;
using FribergCarRentals.Shared.Models.ViewModels.Message;
using FribergCarRentalsBlazor.Components.Admin;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.AdminApi;
using FribergCarRentalsBlazor.ViewModels.Car;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentalsBlazor.Pages.Admin.Car
{
    /// <summary>
    /// The page component class for editing a car.
    /// </summary>
    public partial class EditCar : AdminPageComponentBase
    {
        #region Constants

        /// <summary>
        /// The url for the page. 
        /// </summary>
        public const string PageUrl = PageUrlBase + "/{CarId:int}";

        /// <summary>
        /// The base URL for the page without the car ID.
        /// </summary>
        public const string PageUrlBase = "/admin/car/edit";
        #endregion

        #region Fields

        /// <summary>
        /// A collection of validation errors returned from the API.
        /// </summary>
        private List<MessageViewModel> _apiValidationErrors = new();

        // TODO - Find a cleaner solution to avoid conflicts and sources for future bugs. 
        /// <summary>
        /// The complete viewmodel of the car that is being edited. 
        /// This property is used to support the delete car button. 
        /// </summary>
        private CarViewModel _car = default!;

        /// <summary>
        /// Dummy variable to satisfy Blazor's implementation of the 'ValueChanged' event. 
        /// </summary>
        private bool _dummy = false;

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
        /// The injected admin car categories API service.
        /// </summary>
        [Inject]
        private IAdminCarCategoryApiService AdminCarCategoriesApiService { get; set; } = default!;

        /// <summary>
        /// The injected Auto Mapper service. 
        /// </summary>
        [Inject]
        private IMapper AutoMapper { get; set; } = default!;

        /// <summary>
        /// The view model for editing the car. 
        /// </summary>
        [FromForm]
        private EditCarViewModel EditCarViewModel { get; set; } = new();

        #endregion

        #region Methods

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
            var carResult = await AdminCarApiService.GetCarByIdAsync(CarId);
            var categoriesResult = await AdminCarCategoriesApiService.GetCarCategoriesAsync();

            if (carResult.Success && categoriesResult.Success)
            {
                _car = AutoMapper.Map<CarViewModel>(carResult.Value!);
                EditCarViewModel = AutoMapper.Map<EditCarViewModel>(carResult.Value!);
                EditCarViewModel.Categories = AutoMapper.Map<List<CarCategoryViewModel>>(categoriesResult.Value);
            }
            else
            {
                _apiValidationErrors.Clear();
                _apiValidationErrors.AddRange(carResult.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList());
                _apiValidationErrors.AddRange(categoriesResult.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList());
            }
        }

        /// <summary>
        /// Event callback for a delete car operation.
        /// </summary>
        /// <param name="result">The result of the operation</param>
        /// <returns>A <see cref="Task"/> that represents an async operation.</returns>
        private async Task OnDeleteCar(DeleteCarEventCallbackArgs result)
        {
            if (result.ApiResponse.Success)
            {
                await SessionStorageService.SetItemAsStringAsync(ListCars.DeletedCarIdStorageDataKey, EditCarViewModel.CarId.ToString());
                NavigationManager.NavigateTo(ListCars.PageUrl);
            }
            else
            {
                _apiValidationErrors = result.ApiResponse.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList();
            }
        }

        /// <summary>
        /// Event handler to handle checkbox changes for existing car images being marked for deletion.
        /// </summary>
        private void OnDeleteCarImagesChanged(ImageViewModel image, bool isMarkedForDeletion)
        {
            // This method to keep track of images marked for deletion is being used in order to reuse the existing shared view models. 
            // In a view model designed to be used exclusively by Blazor we could have used data binding directly to the image view models instead. 
            if (!isMarkedForDeletion)
            {
                EditCarViewModel.DeleteImages.Remove(image.ImageId!.Value);
            }
            else if (!EditCarViewModel.DeleteImages.Contains(image.ImageId!.Value))
            {
                EditCarViewModel.DeleteImages.Add(image.ImageId.Value);
            }
        }

        /// <summary>
        /// Event callback for a edit car operation.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents an async operation.</returns>
        private async Task OnEditCar()
        {
            _apiValidationErrors.Clear();
            var editCarResult = await AdminCarApiService.EditCarAsync(EditCarViewModel.CarId, AutoMapper.Map<EditCarDto>(EditCarViewModel));

            if (!editCarResult.Success)
            {
                _apiValidationErrors.AddRange(editCarResult.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList());
                return;
            }

            if (EditCarViewModel.UploadImages.Count > 0)
            {
                var uploadImagesResult = await AdminCarApiService.UploadCarImages(EditCarViewModel.CarId, EditCarViewModel.UploadImages);

                if (!uploadImagesResult.Success)
                {
                    _apiValidationErrors.AddRange(uploadImagesResult.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList());
                    return;
                }

                EditCarViewModel.UploadImages.Clear();
            }

            if (EditCarViewModel.DeleteImages.Count > 0)
            {
                var deleteImagesResult = await AdminCarApiService.DeleteCarImagesAsync(EditCarViewModel.CarId, new DeleteCarImagesDto(EditCarViewModel.DeleteImages));

                if (!deleteImagesResult.Success)
                {
                    _apiValidationErrors.AddRange(deleteImagesResult.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList());
                    return;
                }

                EditCarViewModel.DeleteImages.Clear();
            }

            var fetchCarResult = await AdminCarApiService.GetCarByIdAsync(EditCarViewModel.CarId);

            if (!fetchCarResult.Success)
            {
                _apiValidationErrors.AddRange(fetchCarResult.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList());
                return;
            }

            // We must create new viewmodel objects so that Blazor refrehes the view and removes all checkboxes. This will delete all car categories.
            var categories = EditCarViewModel.Categories;            
            _car = AutoMapper.Map<CarViewModel>(fetchCarResult.Value);
            EditCarViewModel = AutoMapper.Map<EditCarViewModel>(fetchCarResult.Value);
            EditCarViewModel.Categories = categories;
            EditCarViewModel.Messages.Clear();
            EditCarViewModel.Messages.Add(MessageViewModelHelper.CreateCarUpdateSuccessMessage(EditCarViewModel.CarId));
        }

        /// <summary>
        /// Event handler to handle changes for the input file element in the form. 
        /// </summary>
        /// <param name="e">The event arguments.</param>
        private void OnFileUploadChanged(InputFileChangeEventArgs e)
        {
            EditCarViewModel.UploadImages = e.GetMultipleFiles(maximumFileCount: 100)
                .Where(x => ImageTypeHelper.IsSupportedImageMime(x.ContentType))
                .ToList();
        }

        /// <summary>
        /// Event handler for when the user changes the rental status in the edit form. 
        /// </summary>
        /// <param name="newId">The ID of the new rental status.</param>
        private void OnRentalStatusChanged(int newId)
        {
            EditCarViewModel.SelectedRentalStatusId = EditCarViewModel.RentalStatuses.Single(x => x.CarRentalStatusId == newId).CarRentalStatusId;
        }

        /// <summary>
        /// Event handler for when the user changes the propulsion system in the edit form. 
        /// </summary>
        /// <param name="newId">The ID of the new propulsion system.</param>
        private void OnVehiclePropulsionChanged(int newId)
        {
            EditCarViewModel.SelectedPropulsionSystemId = EditCarViewModel.VehiclePropulsions.Single(x => x.VehiclePropulsionId == newId).VehiclePropulsionId;
        }

        #endregion
    }
}
