using AutoMapper;
using FribergCarRentals.Shared.Models.Dto.CarCategory;
using FribergCarRentals.Shared.Models.ViewModels.CarCategory;
using FribergCarRentals.Shared.Models.ViewModels.Message;
using FribergCarRentalsBlazor.Components.Admin;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.AdminApi;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentalsBlazor.Pages.Admin.Car.Category
{
    /// <summary>
    /// The page component class for editing a car category.
    /// </summary>
    public partial class EditCategory : AdminPageComponentBase
    {
        #region Constants

        /// <summary>
        /// The url for the page. 
        /// </summary>
        public const string PageUrl = PageUrlBase + "/{CarCategoryId:int}";

        /// <summary>
        /// The base URL for the page without the category ID.
        /// </summary>
        public const string PageUrlBase = "/admin/car/category/edit";
        #endregion

        #region Fields

        /// <summary>
        /// A collection of validation errors returned from the API.
        /// </summary>
        private List<MessageViewModel> _apiValidationErrors = new();

        // TODO - Find a cleaner solution to avoid conflicts and sources for future bugs. 
        /// <summary>
        /// The complete viewmodel of the car category that is being edited. 
        /// This property is used to support the delete category button. 
        /// </summary>
        private CarCategoryViewModel _carCategory = default!;

        #endregion

        #region Properties

        /// <summary>
        /// The ID of the car category. 
        /// </summary>
        [Parameter]
        public int CarCategoryId { get; set; }

        /// <summary>
        /// The injected admin car category API service.
        /// </summary>
        [Inject]
        private IAdminCarCategoryApiService AdminCarCategoryApiService { get; set; } = default!;

        /// <summary>
        /// The injected Auto Mapper service. 
        /// </summary>
        [Inject]
        private IMapper AutoMapper { get; set; } = default!;

        /// <summary>
        /// The view model for editing the car category. 
        /// </summary>
        [FromForm]
        private EditCarCategoryViewModel EditCarCategoryViewModel { get; set; } = new EditCarCategoryViewModel();

        #endregion

        #region Methods

        /// <summary>
        /// Gets the page URL for a category.
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        /// <returns>A <see cref="string"/> that contains the URL of the page.</returns>
        public static string GetPageUrl(int categoryId)
        {
            return $"{PageUrlBase}/{categoryId}";
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            var result = await AdminCarCategoryApiService.GetCarCategoryStatisticsByIdAsync(CarCategoryId);

            if (result.Success)
            {
                _carCategory = AutoMapper.Map<CarCategoryViewModel>(result.Value!);
                EditCarCategoryViewModel = AutoMapper.Map<EditCarCategoryViewModel>(result.Value!);
            }
            else
            {
                _apiValidationErrors = result.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList();
            }
        }

        /// <summary>
        /// Event callback for a delete category operation.
        /// </summary>
        /// <param name="result">The result of the operation</param>
        /// <returns>A <see cref="Task"/> that represents an async operation.</returns>
        private async Task OnDeleteCategory(DeleteCategoryEventCallbackArgs result)
        {
            if (result.ApiResponse.Success)
            {
                await SessionStorageService.SetItemAsStringAsync(ListCategories.DeletedCarCategoryIdStorageDataKey, EditCarCategoryViewModel.CarCategoryId.ToString());
                NavigationManager.NavigateTo(ListCategories.PageUrl);
            }
            else
            {
                _apiValidationErrors = result.ApiResponse.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList();
            }
        }

        /// <summary>
        /// Event callback for the edit category form submission.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents an async operation.</returns>
        private async Task OnEditCategory()
        {
            var result = await AdminCarCategoryApiService.EditCarCategoryAsync(EditCarCategoryViewModel.CarCategoryId, AutoMapper.Map<EditCarCategoryDto>(EditCarCategoryViewModel));

            if (result.Success)
            {
                _carCategory = AutoMapper.Map<CarCategoryViewModel>(result.Value);
                EditCarCategoryViewModel = AutoMapper.Map<EditCarCategoryViewModel>(result.Value);
                EditCarCategoryViewModel.Messages.Add(MessageViewModelHelper.CreateCarCategoryUpdateSuccessMessage(EditCarCategoryViewModel.CarCategoryId));
            }
            else
            {
                _apiValidationErrors = result.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList();
            }
        }

        #endregion
    }
}
