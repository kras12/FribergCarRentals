using AutoMapper;
using FribergCarRentals.Shared.Models.ViewModels.CarCategory;
using FribergCarRentals.Shared.Models.ViewModels.Message;
using FribergCarRentalsBlazor.Components.Admin;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.AdminApi;
using Microsoft.AspNetCore.Components;

namespace FribergCarRentalsBlazor.Pages.Admin.Car.Category
{
    /// <summary>
    /// The page component class for showing car category details.
    /// </summary>
    public partial class CategoryDetails : AdminPageComponentBase
    {
		#region Constants

		/// <summary>
		/// The base URL for the page without the category ID.
		/// </summary>
		private const string PageUrlBase = "/admin/car/category/details";

		/// <summary>
		/// The url template for the page. 
		/// </summary>
		private const string PageUrlTemplate = PageUrlBase + "/{CarCategoryId:int}";

        #endregion

        #region Fields

        /// <summary>
        /// A collection of validation errors returned from the API.
        /// </summary>
        private List<MessageViewModel> _apiValidationErrors = new();

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
        /// The view model for car categories.
        /// </summary>
        private CarCategoryViewModel? CarCategory { get; set; } = null;

        #endregion

        #region Methods

        /// <summary>
        /// Event callback for a delete category operation.
        /// </summary>
        /// <param name="result">The result of the operation</param>
        /// <returns>A <see cref="Task"/> that represents an async operation.</returns>
        private async Task OnDeleteCategory(DeleteCategoryEventCallbackArgs result)
        {
            if (result.ApiResponse.Success)
            {
                await SessionStorageService.SetItemAsStringAsync(ListCategories.DeletedCarCategoryIdStorageDataKey, CarCategory.CarCategoryId.ToString());
                NavigationManager.NavigateTo(ListCategories.GetPageUrl()); 
            }
            else
            {
                _apiValidationErrors = result.ApiResponse.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList();
            }
        }

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
                CarCategory = AutoMapper.Map<CarCategoryViewModel>(result.Value!);
            }
            else
            {
                _apiValidationErrors = result.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList();
            }
        }

        #endregion
    }
}
