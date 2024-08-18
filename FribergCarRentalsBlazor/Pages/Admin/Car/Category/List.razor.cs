using AutoMapper;
using FribergCarRentals.Shared.Models.ViewModels.CarCategory;
using FribergCarRentals.Shared.Models.ViewModels.Message;
using FribergCarRentals.Shared.Models.ViewModels.Other;
using FribergCarRentalsBlazor.Components.Admin;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.AdminApi;
using Microsoft.AspNetCore.Components;

namespace FribergCarRentalsBlazor.Pages.Admin.Car.Category
{
    /// <summary>
    /// The page component class for listing car categories.
    /// </summary>
    public partial class List : AdminPageComponentBase
    {
        #region Constants

        // TODO - Find a better solution for storing this ID, as this page does not have the primary responsibility for deleting a category. 
        /// <summary>
        /// The key for the ID of the car category that was created. 
        /// </summary>
        public const string CreatedCarCategoryIdStorageDataKey = "AdminCreatedCarCategoryId";

        // TODO - Find a better solution for storing this ID, as this page does not have the primary responsibility for deleting a category. 
        /// <summary>
        /// The key for the ID of the car category that was deleted. 
        /// </summary>
        public const string DeletedCarCategoryIdStorageDataKey = "AdminDeletedCarCategoryId";

        /// <summary>
        /// The url for the page. 
        /// </summary>
        public const string PageUrl = "/admin/car/category/list";        

        #endregion

        #region Fields

        /// <summary>
        /// A collection of validation errors returned from the API.
        /// </summary>
        private List<MessageViewModel> _apiValidationErrors = new();

        /// <summary>
        /// The car categories to list on the page. 
        /// </summary>
        private ListViewModel<CarCategoryViewModel> _categories = default!;

        #endregion

        #region Properties

        /// <summary>
        /// The injected Auto Mapper service. 
        /// </summary>
        [Inject]
        private IMapper AutoMapper { get; set; } = default!;

        /// <summary>
        /// The injected admin car category API service.
        /// </summary>
        [Inject]
        private IAdminCarCategoryApiService AdminCarCategoryApiService { get; set; } = default!;

        #endregion

        #region Methods

        /// <summary>
        /// Event callback for a delete category operation.
        /// </summary>
        /// <param name="result">The result of the operation</param>
        /// <returns>A <see cref="Task"/> that represents an async operation.</returns>
        private void OnDeleteCategory(DeleteCategoryEventCallbackArgs result)
        {
            if (result.ApiResponse.Success)
            {
                _categories.Messages.Add(MessageViewModelHelper.CreateCarCategoryDeletionSuccessMessage(result.CarCategory.CarCategoryId));
                _categories.Models.Remove(result.CarCategory);
                StateHasChanged();
            }
            else
            {
                _apiValidationErrors = result.ApiResponse.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList();
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var result = await AdminCarCategoryApiService.GetCarCategoryStatisticsAsync();

            if (result.Success)
            {
                _categories = new ListViewModel<CarCategoryViewModel>(AutoMapper.Map<List<CarCategoryViewModel>>(result.Value!));

                if (await SessionStorageService.ContainKeyAsync(CreatedCarCategoryIdStorageDataKey))
                {
                    var categoryId = await SessionStorageService.GetItemAsync<int>(CreatedCarCategoryIdStorageDataKey);
                    await SessionStorageService.RemoveItemAsync(CreatedCarCategoryIdStorageDataKey);
                    _categories.Messages.Add(MessageViewModelHelper.CreateCarCategoryCreationSuccessMessage(categoryId));
                }                

                if (await SessionStorageService.ContainKeyAsync(DeletedCarCategoryIdStorageDataKey))
                {
                    var categoryId = await SessionStorageService.GetItemAsync<int>(DeletedCarCategoryIdStorageDataKey);
                    await SessionStorageService.RemoveItemAsync(DeletedCarCategoryIdStorageDataKey);
                    _categories.Messages.Add(MessageViewModelHelper.CreateCarCategoryDeletionSuccessMessage(categoryId));
                }
            }
            else
            {
                _apiValidationErrors = result.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList();
            }
        }

        #endregion
    }
}
