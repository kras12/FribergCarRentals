using AutoMapper;
using FribergCarRentals.Shared.Models.Dto.CarCategory;
using FribergCarRentals.Shared.Models.ViewModels.CarCategory;
using FribergCarRentals.Shared.Models.ViewModels.Message;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.AdminApi;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentalsBlazor.Pages.Admin.Car.Category
{
    /// <summary>
    /// The page component class for creating car categories.
    /// </summary>
    public partial class Create : AdminPageComponentBase
    {
        #region Constants

        /// <summary>
        /// The url for the page. 
        /// </summary>
        public const string PageUrl = "/admin/car/category/create";

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
        /// The injected admin car category API service.
        /// </summary>
        [Inject]
        private IAdminCarCategoryApiService AdminCarCategoryApiService { get; set; } = default!;

        /// <summary>
        /// The view model used for car category creation.
        /// </summary>
        [FromForm]
        private CreateCarCategoryViewModel CreateCarCategoryViewModel { get; set; } = new CreateCarCategoryViewModel();

        #endregion

        #region Methods

        /// <summary>
        /// Creates a car category
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents an async operation.</returns>
        private async Task CreateCategory()
        {
            var result = await AdminCarCategoryApiService.CreateCarCategoryAsync(AutoMapper.Map<CreateCarCategoryDto>(CreateCarCategoryViewModel));

            if (result.Success)
            {
                await SessionStorageService.SetItemAsStringAsync(List.CreatedCarCategoryIdStorageDataKey, result.Value!.CarCategoryId.ToString());
                NavigationManager.NavigateTo(List.PageUrl);
            }
            else
            {
                _apiValidationErrors = result.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList();
            }
        }

        #endregion
    }
}
