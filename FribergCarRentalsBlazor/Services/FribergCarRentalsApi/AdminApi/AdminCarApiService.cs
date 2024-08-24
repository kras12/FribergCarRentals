using FribergCarRentals.Shared.Models.Dto.Api;
using FribergCarRentals.Shared.Models.Dto.Car;
using FribergCarRentals.Shared.Models.Dto.Image;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace FribergCarRentalsBlazor.Services.FribergCarRentalsApi.AdminApi
{
    /// <summary>
    /// A service for managing cars for Friberg Car Rentals Admin API endpoints.
    /// </summary>
    public class AdminCarApiService : ApiServiceBase, IAdminCarApiService
    {
        #region Constants

        /// <summary>
        /// The relative API base address.
        /// </summary>
        private const string ApiBaseAddress = "admin-api/car";

        /// <summary>
        /// The car API endpoint address.
        /// </summary>
        private const string CarApiEndpoint = $"{ApiBaseAddress}";

        /// <summary>
        /// The get car by ID API endpoint address.
        /// </summary>
        private const string CarByIdApiEndpoint = $"{ApiBaseAddress}/{IdPlaceHolder}";

        /// <summary>
        /// The car image API endpoint address.
        /// </summary>
        private const string CarImageApiEndPoint = $"{ApiBaseAddress}/{IdPlaceHolder}/image";

        /// <summary>
        /// The create car API endpoint address.
        /// </summary>
        private const string CreateCarApiEndpoint = $"{ApiBaseAddress}";

        /// <summary>
        /// The delete car API endpoint address.
        /// </summary>
        private const string DeleteCarApiEndpoint = $"{ApiBaseAddress}/{IdPlaceHolder}";

        /// <summary>
        /// The edit car API endpoint address.
        /// </summary>
        private const string EditCarApiEndpoint = $"{ApiBaseAddress}/{IdPlaceHolder}";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="httpClient">The injected HTTP client.</param>
        /// <param name="authenticationStateProvider">The injected autenthication state provider.</param>
        public AdminCarApiService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
            : base(httpClient, authenticationStateProvider)
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a car.
        /// </summary>
        /// <param name="createCarDto">The car input. </param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="CarDto"/> object if successful.</returns>
        public async Task<ApiValueResponseDto<CarDto>> CreateCarAsync(CreateCarDto createCarDto)
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.PostAsJsonAsync(CreateCarApiEndpoint, createCarDto);
            var result = await response.Content.ReadFromJsonAsync<ApiValueResponseDto<CarDto>>();
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        /// <summary>
        /// Deletes a car.
        /// </summary>
        /// <param name="carId">The ID of the car to delete.</param>
        /// <returns>An <see cref="ApiResponseDto"/> that contains the result of the operation.</returns>
        public async Task<ApiResponseDto> DeleteCarAsync(int carId)
        {
            await SetAuthorizationHeaderAsync();
            var result = await _httpClient.DeleteFromJsonAsync<ApiResponseDto>(DeleteCarApiEndpoint.Replace(IdPlaceHolder, carId.ToString()));
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        /// <summary>
        /// Deletes car images.
        /// </summary>
        /// <param name="carId">The ID of the car the image belongs to.</param>
        /// <param name="images">Contains the IDs of the images to delete.</param>
        /// <returns>An <see cref="ApiResponseDto"/> that contains the result of the operation.</returns>
        public async Task<ApiResponseDto> DeleteCarImagesAsync(int carId, DeleteCarImagesDto images)
        {
            #region Checks

            if (images.ImageIds.Count == 0)
            {
                throw new ArgumentException($"Parameter collection '{nameof(images)}.{nameof(images.ImageIds)}' can't be empty.");
            }

            #endregion

            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete,
                $"{CarImageApiEndPoint.Replace(IdPlaceHolder, carId.ToString())}")
            {
                Content = JsonContent.Create(images)
            };

            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.SendAsync(requestMessage);
            var result = await response.Content.ReadFromJsonAsync<ApiResponseDto>();
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        /// <summary>
        /// Edits a car.
        /// </summary>
        /// <param name="carId">The ID of the car.</param>
        /// <param name="car">The new data for the car.</param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="CarDto"/> object if successful.</returns>
        public async Task<ApiValueResponseDto<CarDto>> EditCarAsync(int carId, EditCarDto car)
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.PutAsJsonAsync(EditCarApiEndpoint.Replace(IdPlaceHolder, carId.ToString()), car);
            var result = await response.Content.ReadFromJsonAsync<ApiValueResponseDto<CarDto>>();
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        /// <summary>
        /// Gets all cars.
        /// </summary>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a collection of <see cref="CarDto"/> object if successful.</returns>
        public async Task<ApiValueResponseDto<List<CarDto>>> GetCarsAsync()
        {
            await SetAuthorizationHeaderAsync();
            var result = await _httpClient.GetFromJsonAsync<ApiValueResponseDto<List<CarDto>>>(CarApiEndpoint);
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        /// <summary>
        /// Gets a car by ID.
        /// </summary>
        /// <param name="carId">The ID of the car.</param>
        /// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="CarDto"/> object if successful.</returns>
        public async Task<ApiValueResponseDto<CarDto>> GetCarByIdAsync(int carId)
        {
            await SetAuthorizationHeaderAsync();
            var result = await _httpClient.GetFromJsonAsync<ApiValueResponseDto<CarDto>>(CarByIdApiEndpoint.Replace(IdPlaceHolder, carId.ToString()));
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        /// <summary>
        /// Uploads images for a car.
        /// </summary>
        /// <param name="carId">The ID of the car the image belongs to.</param>
        /// <param name="newFiles">A collection of image files to upload.</param>
        /// <returns>A <see cref="Task"/> containing a collection of <see cref="CarImageDto"/> objects for the uploaded images.</returns>
        public async Task<ApiValueResponseDto<List<CarImageDto>>> UploadCarImages([Required] int carId, List<IBrowserFile> newFiles)
        {
            if (newFiles.Count == 0)
            {
                throw new ArgumentException($"The collection '{newFiles}' can't be empty.", nameof(newFiles));
            }

            var content = new MultipartFormDataContent();

            foreach (var file in newFiles)
            {
                content.Add(new StreamContent(file.OpenReadStream()), "files", file.Name);
            }

            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.PostAsync($"{CarImageApiEndPoint.Replace(IdPlaceHolder, carId.ToString())}", content);
            var result = await response.Content.ReadFromJsonAsync<ApiValueResponseDto<List<CarImageDto>>>();
            return EnsureNotNull(result, "Failed to serialize the API response.");
        }

        #endregion
    }
}
