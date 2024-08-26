using AutoMapper;
using FribergCarRentals.Shared.Helpers;
using FribergCarRentals.Shared.Models.Blazor.ViewModels.Car;
using FribergCarRentals.Shared.Models.Dto.Car;
using FribergCarRentals.Shared.Models.ViewModels.CarCategory;
using FribergCarRentals.Shared.Models.ViewModels.Message;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.AdminApi;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace FribergCarRentalsBlazor.Pages.Admin.Car
{
    /// <summary>
    /// The page component class for creating cars.
    /// </summary>
    public partial class CreateCar : AdminPageComponentBase
    {
        #region Constants

        /// <summary>
        /// The url for the page. 
        /// </summary>
        public const string PageUrl = "/admin/car/create";

        #endregion

        #region Fields

        /// <summary>
        /// A collection of validation errors returned from the API.
        /// </summary>
        private List<MessageViewModel> _apiValidationErrors = new();

        #endregion

        #region Properties

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
        /// The view model used for car creation.
        /// </summary>
        [SupplyParameterFromForm]
        private CreateCarViewModel? CreateCarViewModel { get; set; } = null;

        #endregion

        #region Methods        

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var categoriesResult = await AdminCarCategoriesApiService.GetCarCategoriesAsync();

            if (categoriesResult.Success)
            {
                CreateCarViewModel = new CreateCarViewModel();
                CreateCarViewModel.Categories = AutoMapper.Map<List<CarCategoryViewModel>>(categoriesResult.Value);
            }
            else
            {
                _apiValidationErrors.Clear();
                _apiValidationErrors.AddRange(categoriesResult.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList());
            }
        }

        /// <summary>
        /// Creates a car.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents an async operation.</returns>
        private async Task OnCreateCar()
        {
            var createCarResult = await AdminCarApiService.CreateCarAsync(AutoMapper.Map<CreateCarDto>(CreateCarViewModel));

            if (createCarResult.Success)
            {
                if (CreateCarViewModel!.UploadImages.Any())
                {
                    var uploadImagesResult = await AdminCarApiService.UploadCarImages(createCarResult.Value!.CarId, CreateCarViewModel.UploadImages);

                    if (!uploadImagesResult.Success)
                    {
                        _apiValidationErrors = uploadImagesResult.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList();
                        return;
                    }
                }                

                await SessionStorageService.SetItemAsStringAsync(ListCars.CreatedCarIdStorageDataKey, createCarResult.Value!.CarId.ToString());
                NavigationManager.NavigateTo(ListCars.PageUrl);
            }
            else
            {
                _apiValidationErrors = createCarResult.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList();
            }
        }

        /// <summary>
        /// Event handler to handle changes for the input file element in the form. 
        /// </summary>
        /// <param name="e">The event arguments.</param>
        private void OnFileUploadChanged(InputFileChangeEventArgs e)
        {
            CreateCarViewModel!.UploadImages = e.GetMultipleFiles(maximumFileCount: 100)
                .Where(x => ImageTypeHelper.IsSupportedImageMime(x.ContentType))
                .ToList();
        }
        /// <summary>
        /// Event handler for when the user changes the rental status in the edit form. 
        /// </summary>
        /// <param name="newId">The ID of the new rental status.</param>
        private void OnRentalStatusChanged(int newId)
        {
            CreateCarViewModel!.SelectedRentalStatusId = CreateCarViewModel.RentalStatuses.Single(x => x.CarRentalStatusId == newId).CarRentalStatusId;
        }

        /// <summary>
        /// Event handler for when the user changes the propulsion system in the edit form. 
        /// </summary>
        /// <param name="newId">The ID of the new propulsion system.</param>
        private void OnVehiclePropulsionChanged(int newId)
        {
            CreateCarViewModel!.SelectedPropulsionSystemId = CreateCarViewModel.VehiclePropulsions.Single(x => x.VehiclePropulsionId == newId).VehiclePropulsionId;
        }

        #endregion
    }
}
