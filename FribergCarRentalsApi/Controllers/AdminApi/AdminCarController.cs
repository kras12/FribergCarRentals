using AutoMapper;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Data.Repositories;
using FribergCarRentals.Helpers;
using FribergCarRentals.Shared;
using FribergCarRentals.Shared.Constants;
using FribergCarRentals.Shared.Dto.Api;
using FribergCarRentals.Shared.Dto.Car;
using FribergCarRentals.Shared.Dto.CarCategory;
using FribergCarRentals.Shared.Dto.Image;
using FribergCarRentalsApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcRazorPages.Shared.Services;

namespace FribergCarRentalsApi.Controllers.AdminApi
{
    /// <summary>
    /// Handles admin related activites for cars.
    /// </summary>
    [Route("api/admin/car/")]
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
        private readonly IImageDownloadService _imageDownloadService;

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
            ICarCategoryRepository carCategoryRepository, IImageUploadService imageUploadService, IImageDownloadService imageDownloadService)
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
        [HttpPost("{id}/images")]
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
        public async Task<IActionResult> Delete(int id)
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
        /// <param name="id">The ID for the car.</param>
        /// <param name="deleteCarImagesDto">Contains a collection of IDs for the images to delete.</param>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}/images")]
        public async Task<IActionResult> DeleteImages(int id, DeleteCarImagesDto deleteCarImagesDto)
        {
            if (!await IsAuthorized(ApplicationUserPolicies.Admin))
            {
                return Unauthorized(CreateUnauthorizedResponse());
            }

            if (deleteCarImagesDto.ImageIds.Count == 0)
            {
                return BadRequest(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData, $"The collection of images to delete is empty."));
            }

            if (id <= 0)
            {
                return BadRequest(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData, $"Invalid car id: {id}"));
            }

            if (!await _carRepository.CarExists(id))
            {
                return NotFound(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.ResourceNotFound, $"Failed to find car with ID: {id}"));
            }

            await _carRepository.DeleteCarImages(id, deleteCarImagesDto.ImageIds);

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

            if (editCarDto.DeleteImages != null && editCarDto.DeleteImages.Count > 0)
            {
                car.Images.RemoveAll(x => editCarDto.DeleteImages.Contains(x.ImageId));
            }

            await _carRepository.UpdateAsync(car);            

            var finalCar = _mapper.Map<CarDto>(car);
            finalCar.Images.ForEach(x => x.Url = _imageDownloadService.GetImageUrl(
                Url.Action(nameof(AdminFileController.GetImageFile), ControllerHelper.GetControllerName<AdminFileController>())!,
                x.FileName));

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
            finalCar.Images.ForEach(x => x.Url = _imageDownloadService.GetImageUrl(
                Url.Action(nameof(AdminFileController.GetImageFile), ControllerHelper.GetControllerName<AdminFileController>())!,
                x.FileName));

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
            finalCars.ForEach(car => 
                car.Images.ForEach(image => image.Url = _imageDownloadService.GetImageUrl(
                    Url.Action(nameof(AdminFileController.GetImageFile), ControllerHelper.GetControllerName<AdminFileController>())!,
                    image.FileName)));

            return Ok(ApiValueResponseDto<List<CarDto>>.CreateSuccessfulResponse(finalCars));
        }

        #endregion
    }
}
