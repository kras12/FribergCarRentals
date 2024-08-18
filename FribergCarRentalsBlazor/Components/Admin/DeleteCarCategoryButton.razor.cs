using FribergCarRentals.Shared.Models.Dto.Api;
using FribergCarRentals.Shared.Models.ViewModels.CarCategory;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.AdminApi;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FribergCarRentalsBlazor.Components.Admin
{
    /// <summary>
    /// A component that conditionally shows a delete car category button and handles the deletion process.
    /// </summary>
    public partial class DeleteCarCategoryButton : ComponentBase
    {
        #region Properties

        /// <summary>
        /// The injected admin car category API service.
        /// </summary>
        [Inject]
        private IAdminCarCategoryApiService AdminCarCategoryApiService { get; set; } = default!;

        /// <summary>
        /// The view model used for car creation.
        /// </summary>
        [Parameter]
        public CarCategoryViewModel CarCategory { get; set; } = new CarCategoryViewModel();

        /// <summary>
        /// The injected JavaScript runtime. 
        /// </summary>
        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

        /// <summary>
        /// Event callback for when a category has been deleted. 
        /// </summary>
        [Parameter]
        public EventCallback<DeleteCategoryEventCallbackArgs> OnDeletedCategory { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a car category
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents an async operation.</returns>
        private async Task DeleteCategory()
        {
            if (await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to delete this category?"))
            {
                var result = await AdminCarCategoryApiService.DeleteCarCategoryAsync(CarCategory.CarCategoryId);
                await OnDeletedCategory.InvokeAsync(new DeleteCategoryEventCallbackArgs(CarCategory, result));
            }
        }

        #endregion
    }

    /// <summary>
    /// Event callback storage class that contains the result of a delete car category operation.
    /// </summary>
    public class DeleteCategoryEventCallbackArgs
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="carCategory">The car category that was deleted.</param>
        /// <param name="result">The API response for the operation.</param>
        public DeleteCategoryEventCallbackArgs(CarCategoryViewModel carCategory, ApiResponseDto result)
        {
            CarCategory = carCategory;
            ApiResponse = result;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The car category that was deleted.
        /// </summary>
        public CarCategoryViewModel CarCategory {  get; }
            
        /// <summary>
        /// The API response for the operation.
        /// </summary>
        public ApiResponseDto ApiResponse { get; }

        #endregion
    }
}
