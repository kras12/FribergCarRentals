using AutoMapper;
using FribergCarRentals.Shared.Models.ViewModels.CarCategory;
using FribergCarRentals.Shared.Models.ViewModels.Message;
using FribergCarRentals.Shared.Models.ViewModels.Other;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.AdminApi;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FribergCarRentalsBlazor.Pages.Admin.Car.Category
{
    /// <summary>
    /// The page component class for listing car categories.
    /// </summary>
    public partial class List : AdminPageComponentBase
    {
        #region Constants

        /// <summary>
        /// The url for the page. 
        /// </summary>
        public const string PageUrl = "/admin/car/categories/list";

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

        /// <summary>
        /// The injected JavaScript runtime. 
        /// </summary>
        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a car category
        /// </summary>
        /// <param name="category">The category to delete.</param>
        /// <returns>A <see cref="Task"/> that represents an async operation.</returns>
        private async Task DeleteCategory(CarCategoryViewModel category)
        {
            if (await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to delete this category?"))
            {
                var result = await AdminCarCategoryApiService.DeleteCarCategoryAsync(category.CarCategoryId);

                if (result.Success)
                {
                    _categories.Messages.Add(MessageViewModelHelper.CreateCarCategoryDeletionSuccessMessage(category.CarCategoryId));
                    _categories.Models.Remove(category);
                    StateHasChanged();
                }
                else
                {
                    _apiValidationErrors = result.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList();
                }
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
            }
            else
            {
                _apiValidationErrors = result.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList();
            }
        }

        #endregion
    }
}
