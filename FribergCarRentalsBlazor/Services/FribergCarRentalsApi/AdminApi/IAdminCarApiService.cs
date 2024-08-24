using FribergCarRentals.Shared.Models.Dto.Api;
using FribergCarRentals.Shared.Models.Dto.Car;
using FribergCarRentals.Shared.Models.Dto.Image;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentalsBlazor.Services.FribergCarRentalsApi.AdminApi
{
    /// <summary>
    /// An interface for a service for managing car categories for Friberg Car Rentals Admin API endpoints.
    /// </summary>
    public interface IAdminCarApiService
    {
        /// <summary>
        /// Creates a car.
        /// </summary>
        /// <param name="createCarDto">The car input. </param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="CarDto"/> object if successful.</returns>
        public Task<ApiValueResponseDto<CarDto>> CreateCarAsync(CreateCarDto createCarDto);

        /// <summary>
        /// Deletes a car.
        /// </summary>
        /// <param name="carId">The ID of the car to delete.</param>
        /// <returns>An <see cref="ApiResponseDto"/>.</returns>
        public Task<ApiResponseDto> DeleteCarAsync(int carId);

        /// <summary>
        /// Deletes car images.
        /// </summary>
        /// <param name="carId">The ID of the car the image belongs to.</param>
        /// <param name="images">Contains the IDs of the images to delete.</param>
        /// <returns>An <see cref="ApiResponseDto"/> that contains the result of the operation.</returns>
        public Task<ApiResponseDto> DeleteCarImagesAsync(int carId, DeleteCarImagesDto images);

        /// <summary>
        /// Edits a car.
        /// </summary>
        /// <param name="carId">The ID of the car.</param>
        /// <param name="car">The new data for the car.</param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="CarDto"/> object if successful.</returns>
        public Task<ApiValueResponseDto<CarDto>> EditCarAsync(int carId, EditCarDto car);

        /// <summary>
        /// Gets a car by ID.
        /// </summary>
        /// <param name="carId">The ID of the car.</param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="CarDto"/> object if successful.</returns>
        public Task<ApiValueResponseDto<CarDto>> GetCarByIdAsync(int carId);

        /// <summary>
        /// Gets all cars.
        /// </summary>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a collection of <see cref="CarDto"/> object if successful.</returns>
        public Task<ApiValueResponseDto<List<CarDto>>> GetCarsAsync();

        /// <summary>
        /// Uploads images for a car.
        /// </summary>
        /// <param name="carId">The ID of the car the image belongs to.</param>
        /// <param name="newFiles">A collection of image files to upload.</param>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="CarImageDto"/> objects for the uploaded images.</returns>
        public Task<ApiValueResponseDto<List<CarImageDto>>> UploadCarImages([Required] int carId, List<IBrowserFile> newFiles);
    }
}