using FribergCarRentals.Shared.Models.Dto.Api;
using FribergCarRentals.Shared.Models.ViewModels.Car;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.AdminApi;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FribergCarRentalsBlazor.Components.Admin
{
    /// <summary>
    /// A component that shows a delete car button and handles the deletion process.
    /// </summary>
    public partial class DeleteCarButton : ComponentBase
    {
        #region Properties

        /// <summary>
        /// The injected admin car API service.
        /// </summary>
        [Inject]
        private IAdminCarApiService AdminCarApiService { get; set; } = default!;

        /// <summary>
        /// The car to delete. 
        /// </summary>
        [Parameter]
        public CarViewModel Car { get; set; } = new();

        /// <summary>
        /// The injected JavaScript runtime. 
        /// </summary>
        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

        /// <summary>
        /// Event callback for when a car has been deleted. 
        /// </summary>
        [Parameter]
        public EventCallback<DeleteCarEventCallbackArgs> OnDeletedCar { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a car.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents an async operation.</returns>
        private async Task DeleteCar()
        {
            if (await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to delete this car?"))
            {
                var result = await AdminCarApiService.DeleteCarAsync(Car.CarId);
                await OnDeletedCar.InvokeAsync(new DeleteCarEventCallbackArgs(Car, result));
            }
        }

        #endregion
    }

    /// <summary>
    /// Event callback storage class that contains the result of a delete car operation.
    /// </summary>
    public class DeleteCarEventCallbackArgs
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="car">The car that was deleted.</param>
        /// <param name="result">The API response for the operation.</param>
        public DeleteCarEventCallbackArgs(CarViewModel car, ApiResponseDto result)
        {
            Car = car;
            ApiResponse = result;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The car that was deleted.
        /// </summary>
        public CarViewModel Car { get; }
            
        /// <summary>
        /// The API response for the operation.
        /// </summary>
        public ApiResponseDto ApiResponse { get; }

        #endregion
    }
}
