using AutoMapper;
using FribergCarRentals.Shared.Models.Dto.CarCategory;
using FribergCarRentals.Shared.Models.ViewModels.CarCategory;
using FribergCarRentals.Shared.Models.ViewModels.Message;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.AdminApi;
using Microsoft.AspNetCore.Components;

namespace FribergCarRentalsBlazor.Pages.Admin.Car.Category
{
    /// <summary>
    /// The page component class for creating car categories.
    /// </summary>
    public partial class CreateCategory : AdminPageComponentBase
    {
		#region Constants

		/// <summary>
		/// The url template for the page. 
		/// </summary>
		private const string PageUrlTemplate = "/admin/car/category/create";

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
        [SupplyParameterFromForm]
        private CreateCarCategoryViewModel CreateCarCategoryViewModel { get; set; } = new CreateCarCategoryViewModel();

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
		/// Creates a car category
		/// </summary>
		/// <returns>A <see cref="Task"/> that represents an async operation.</returns>
		private async Task OnCreateCategory()
        {
            var result = await AdminCarCategoryApiService.CreateCarCategoryAsync(AutoMapper.Map<CreateCarCategoryDto>(CreateCarCategoryViewModel));

            if (result.Success)
            {
                await SessionStorageService.SetItemAsStringAsync(ListCategories.CreatedCarCategoryIdStorageDataKey, result.Value!.CarCategoryId.ToString());
                NavigationManager.NavigateTo(ListCategories.GetPageUrl());
            }
            else
            {
                _apiValidationErrors = result.Errors.Select(x => new MessageViewModel(MessageType.Error, x.ErrorMessage, title: x.ErrorType)).ToList();
            }
        }

        #endregion
    }
}
