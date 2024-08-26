using AutoMapper;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Data.Repositories;
using FribergCarRentals.Shared;
using FribergCarRentals.Shared.Constants;
using FribergCarRentals.Shared.Models.Dto.Api;
using FribergCarRentals.Shared.Models.Dto.Car;
using FribergCarRentals.Shared.Models.Dto.CarCategory;
using FribergCarRentals.Shared.Models.Dto.Image;
using FribergCarRentals.Shared.Mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentalsApi.Controllers.AdminApi
{
    /// <summary>
    /// Handles admin related activites for cars.
    /// </summary>
    [Route("admin-api/car")]
    [ApiController]
    public class AdminCarController : ApiControllerBase
    {
        #region Fields

        /// <summary>
        /// The injected car category repository.
        /// </summary>
        private readonly ICarCategoryRepository _carCategoryRepository;

        /// <summary>
        /// The injected car repository.
        /// </summary>
        private readonly ICarRepository _carRepository;

        /// <summary>
        /// The injected image download service.
        /// </summary>
        private readonly IImageApiDownloadService _imageDownloadService;

        /// <summary>
        /// The injected image upload service.
        /// </summary>
        private readonly IImageUploadService _imageUploadService;

        /// <summary>
        /// The injected Auto Mapper. 
        /// </summary>
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="mapper">The injected Auto Mapper.</param>
        /// <param name="carRepository">The injected car repository.</param>
        /// <param name="carCategoryRepository">The injected car category repository.</param>
        /// <param name="imageUploadService">The injected image upload service.</param>
        /// <param name="imageDownloadService">The injected image download service.</param>
        public AdminCarController(IAuthorizationService authorizationService, IMapper mapper, ICarRepository carRepository,
            ICarCategoryRepository carCategoryRepository, IImageUploadService imageUploadService, IImageApiDownloadService imageDownloadService)
            : base(authorizationService)
        {
            _mapper = mapper;
            _carRepository = carRepository;
            _carCategoryRepository = carCategoryRepository;
            _imageUploadService = imageUploadService;
            _imageDownloadService = imageDownloadService;
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Creates a new car.
        /// </summary>
        /// <param name="createCarDto">The input data for the car.</param>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        [ProducesResponseType<ApiValueResponseDto<CarDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCarDto createCarDto)
        {
            if (!await IsAuthorized(ApplicationUserPolicies.Admin))
            {
                return Unauthorized(CreateUnauthorizedResponse());
            }

            var car = _mapper.Map<CarEntity>(createCarDto);
            var selectedCategory = await _carCategoryRepository.GetByIdAsync(createCarDto.CategoryId);

            if (selectedCategory == null)
            {
                return BadRequest(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData,
                    $"Car category not found: {createCarDto.CategoryId}"));
            }

            car.Category = selectedCategory;
            await _carRepository.AddAsync(car);

            return Ok(ApiValueResponseDto<CarDto>.CreateSuccessfulResponse(_mapper.Map<CarDto>(car)));
        }

        /// <summary>
        /// Creates car images.
        /// </summary>
        /// <param name="id">The ID of the car the images belongs to.</param>
        /// <param name="files">A collection of uploaded image files.</param>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        [ProducesResponseType<ApiValueResponseDto<List<CarImageDto>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status404NotFound)]
        [HttpPost("{id}/image")]
        public async Task<IActionResult> CreateCarImages(int id, [FromForm] IFormFileCollection files)
        {
            if (!await IsAuthorized(ApplicationUserPolicies.Admin))
            {
                return Unauthorized(CreateUnauthorizedResponse());
            }

            if (files.Count == 0)
            {
                return BadRequest(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData, "No files were submitted."));
            }

            List<ImageEntity> imageEntities = new();

            foreach (var file in files)
            {
                imageEntities.Add(new ImageEntity(await _imageUploadService.SaveImageToDiskAsync(file)));
            }

            await _carRepository.AddImages(id, imageEntities);

            return Ok(ApiValueResponseDto<List<CarImageDto>>.CreateSuccessfulResponse(_mapper.Map<List<CarImageDto>>(imageEntities)));
        }

        /// <summary>
        /// Deletes a car.
        /// </summary>
        /// <param name="id">The ID for the car.</param>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            if (!await IsAuthorized(ApplicationUserPolicies.Admin))
            {
                return Unauthorized(CreateUnauthorizedResponse());
            }

            if (id <= 0)
            {
                return BadRequest(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData, $"Invalid car id: {id}"));
            }

            var car = await _carRepository.GetByIdAsync(id);

            if (car == null)
            {
                return NotFound(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.ResourceNotFound, $"Failed to find car with ID: {id}"));
            }

            if (car!.Images.Count > 0)
            {
                _imageUploadService.DeleteImagesFromDisk(car!.Images.Select(x => x.FileName));
            }

            await _carRepository.DeleteAsync(id);

            return Ok(ApiResponseDto.CreateSuccessfulResponse());
        }

        /// <summary>
        /// Deletes car images.
        /// </summary>
        /// <param name="carId">The ID for the car.</param>
        /// <param name="deleteCarImagesDto">Contains a collection of IDs for the images to delete.</param>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status404NotFound)]
        [HttpDelete("{carId:int}/image")]
        public async Task<IActionResult> DeleteCarImages(int carId, DeleteCarImagesDto deleteCarImagesDto)
        {
            if (!await IsAuthorized(ApplicationUserPolicies.Admin))
            {
                return Unauthorized(CreateUnauthorizedResponse());
            }

            if (deleteCarImagesDto.ImageIds.Count == 0)
            {
                return BadRequest(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData, $"The collection of images to delete is empty."));
            }

            if (carId <= 0)
            {
                return BadRequest(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData, $"Invalid car id: {carId}"));
            }

            if (!await _carRepository.CarExists(carId))
            {
                return NotFound(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.ResourceNotFound, $"Failed to find car with ID: {carId}"));
            }

            await _carRepository.DeleteCarImages(carId, deleteCarImagesDto.ImageIds);

            return Ok(ApiResponseDto.CreateSuccessfulResponse());
        }

        /// <summary>
        /// Edits a car.
        /// </summary>
        /// <param name="createCarDto">The input data for the car.</param>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        [ProducesResponseType<ApiValueResponseDto<CarDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, EditCarDto editCarDto)
        {
            if (!await IsAuthorized(ApplicationUserPolicies.Admin))
            {
                return Unauthorized(CreateUnauthorizedResponse());
            }

            if (id <= 0)
            {
                return BadRequest(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData, $"Invalid car id: {id}"));
            }

            var car = await _carRepository.GetByIdAsync(id);

            if (car == null)
            {
                return NotFound(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.ResourceNotFound, $"Failed to find car with ID: {id}"));
            }

            _mapper.Map(editCarDto, car);

            if (car.Category!.CarCategoryId != editCarDto.CategoryId)
            {
                var category = await _carCategoryRepository.GetByIdAsync(editCarDto.CategoryId);

                if (category == null)
                {
                    return NotFound(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.ResourceNotFound, $"Failed to find car category with ID: {editCarDto.CategoryId}"));
                }

                car.Category = category;
            }

            if (editCarDto.DeleteImages != null && editCarDto.DeleteImages.Count > 0)
            {
                car.Images.RemoveAll(x => editCarDto.DeleteImages.Contains(x.ImageId));
            }

            await _carRepository.UpdateAsync(car);            

            var finalCar = _mapper.Map<CarDto>(car);
            SetImageUrls(finalCar.Images);

            return Ok(ApiValueResponseDto<CarDto>.CreateSuccessfulResponse(finalCar));
        }

        /// <summary>
        /// Gets a car by ID.
        /// </summary>
        /// <param name="id">The ID for the car.</param>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        [ProducesResponseType<ApiValueResponseDto<CarDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarById(int id)
        {
            if (!await IsAuthorized(ApplicationUserPolicies.Admin))
            {
                return Unauthorized(CreateUnauthorizedResponse());
            }

            if (id <= 0)
            {
                return BadRequest(ApiValueResponseDto<CarCategoryDto>.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData, $"Invalid car id: {id}"));
            }

            var car = await _carRepository.GetByIdAsync(id);

            if (car == null)
            {
                return NotFound(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.ResourceNotFound, $"Failed to find car with ID: {id}"));
            }

            var finalCar = _mapper.Map<CarDto>(car);
            SetImageUrls(finalCar.Images);

            return Ok(ApiValueResponseDto<CarDto>.CreateSuccessfulResponse(finalCar));
        }

        /// <summary>
        /// Gets all cars.
        /// </summary>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        [ProducesResponseType<ApiValueResponseDto<List<CarDto>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        public async Task<IActionResult> List()
        {
            if (!await IsAuthorized(ApplicationUserPolicies.Admin))
            {
                return Unauthorized(CreateUnauthorizedResponse());
            }

            var cars = _mapper.Map<List<CarDto>>((await _carRepository.GetAllAsync()).ToList());

            var finalCars = _mapper.Map<List<CarDto>>(cars);
            SetImageUrls(finalCars.SelectMany(x => x.Images).ToList());

            return Ok(ApiValueResponseDto<List<CarDto>>.CreateSuccessfulResponse(finalCars));
        }

        #endregion

        #region Methods

        /// <summary>
		/// Sets the image urls for image DTOs.
		/// </summary>
		/// <param name="images">A collection of image DTOs to process.</param>
		private void SetImageUrls(List<CarImageDto> images)
        {
            images.ForEach(x => x.Url = AdminFileController.GetImageUrl(HttpContext, x.FileName));
        }

        #endregion
    }
}
