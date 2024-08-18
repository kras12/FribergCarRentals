using AutoMapper;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Data.Repositories;
using FribergCarRentals.Shared;
using FribergCarRentals.Shared.Constants;
using FribergCarRentals.Shared.Models.Dto.Api;
using FribergCarRentals.Shared.Models.Dto.CarCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentalsApi.Controllers.AdminApi
{
    /// <summary>
    /// Handles admin related activites for car categories.
    /// </summary>
    [Route("admin-api/car/category")]
    [ApiController]
    public class AdminCarCategoryController : ApiControllerBase
    {
        #region Fields

        /// <summary>
        /// The injected car category repository.
        /// </summary>
        private readonly ICarCategoryRepository _carCategoryRepository;

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
        /// <param name="carCategoryRepository">The injected car category repository.</param>
        public AdminCarCategoryController(IAuthorizationService authorizationService, IMapper mapper, ICarCategoryRepository carCategoryRepository)
            : base(authorizationService)
        {
            _mapper = mapper;
            _carCategoryRepository = carCategoryRepository;
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Creates a new car category.
        /// </summary>
        /// <param name="createCarCategoryDto">The input data for the category.</param>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        [HttpPost]
        [ProducesResponseType<ApiValueResponseDto<CarCategoryDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateCategory(CreateCarCategoryDto createCarCategoryDto)
        {
            if (!await IsAuthorized(ApplicationUserPolicies.Admin))
            {
                return Unauthorized(CreateUnauthorizedResponse());
            }

            var category = _mapper.Map<CarCategoryEntity>(createCarCategoryDto);
            await _carCategoryRepository.AddAsync(category);
            
            return Ok(ApiValueResponseDto<CarCategoryDto>.CreateSuccessfulResponse(_mapper.Map<CarCategoryDto>(category)));
        }

        /// <summary>
        /// Deletes a car category.
        /// </summary>
        /// <param name="id">The ID of the category.</param>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType<ApiValueResponseDto<CarCategoryDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await IsAuthorized(ApplicationUserPolicies.Admin))
            {
                return Unauthorized(CreateUnauthorizedResponse());
            }

            if (id <= 0)
            {
                return BadRequest(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData, $"Invalid category id: {id}"));
            }

            if (!await _carCategoryRepository.CategoryExists(id))
            {
                return NotFound(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.ResourceNotFound, "Car category not found."));
            }

            await _carCategoryRepository.DeleteAsync(id);
            return Ok(ApiValueResponseDto<CarCategoryDto>.CreateSuccessfulResponse(null!));
        }

        /// <summary>
        /// Edits a car category.
        /// </summary>
        /// <param name="id">The ID of the category.</param>
        /// <param name="editCarCategoryDto">The new data for the category.</param>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType<ApiValueResponseDto<CarCategoryDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Edit(int id, EditCarCategoryDto editCarCategoryDto)
        {
            if (!await IsAuthorized(ApplicationUserPolicies.Admin))
            {
                return Unauthorized(CreateUnauthorizedResponse());
            }

            if (id <= 0)
            {
                return BadRequest(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData, $"Invalid category id: {id}"));
            }

            if (!await _carCategoryRepository.CategoryExists(id))
            {
                return NotFound(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.ResourceNotFound, "Car category not found."));
            }

            var category = _mapper.Map<CarCategoryEntity>(editCarCategoryDto);
            category.CarCategoryId = id;

            await _carCategoryRepository.UpdateAsync(category);

            return Ok(ApiValueResponseDto<CarCategoryDto>.CreateSuccessfulResponse(_mapper.Map<CarCategoryDto>(category)));
        }

        /// <summary>
        /// Returns all car categories.
        /// </summary>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        [HttpGet]
        [ProducesResponseType<ApiValueResponseDto<List<CarCategoryDto>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCarCategories()
        {
            if (!await IsAuthorized(ApplicationUserPolicies.Admin))
            {
                return Unauthorized(CreateUnauthorizedResponse());
            }

            var categoryStatistics = await _carCategoryRepository.GetAllAsync();
            return Ok(ApiValueResponseDto<List<CarCategoryDto>>.CreateSuccessfulResponse(_mapper.Map<List<CarCategoryDto>>(categoryStatistics)));
        }

        /// <summary>
        /// Returns statistics for all car categories.
        /// </summary>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        [HttpGet("statistics")]
        [ProducesResponseType<ApiValueResponseDto<List<CarCategoryStatisticsDto>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCarCategoryStatistics()
        {
            if (!await IsAuthorized(ApplicationUserPolicies.Admin))
            {
                return Unauthorized(CreateUnauthorizedResponse());
            }

            var categoryStatistics = await _carCategoryRepository.GetCategoryStatistics();
            return Ok(ApiValueResponseDto<List<CarCategoryStatisticsDto>>.CreateSuccessfulResponse(_mapper.Map<List<CarCategoryStatisticsDto>>(categoryStatistics)));
        }

        /// <summary>
        /// Gets a car category by ID.
        /// </summary>
        /// <param name="id">The ID of the category.</param>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType<ApiValueResponseDto<CarCategoryDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            if (!await IsAuthorized(ApplicationUserPolicies.Admin))
            {
                return Unauthorized(CreateUnauthorizedResponse());
            }

            if (id <= 0)
            {
                return BadRequest(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData, $"Invalid category id: {id}"));
            }

            var category = await _carCategoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.ResourceNotFound, "Car category not found."));
            }

            return Ok(ApiValueResponseDto<CarCategoryDto>.CreateSuccessfulResponse(_mapper.Map<CarCategoryDto>(category)));
        }

        /// <summary>
        /// Gets statistics for a car category by ID.
        /// </summary>
        /// <param name="id">The ID of the category.</param>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        [HttpGet("{id}/statistics")]
        [ProducesResponseType<ApiValueResponseDto<CarCategoryStatisticsDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategoryStatisticsById(int id)
        {
            if (!await IsAuthorized(ApplicationUserPolicies.Admin))
            {
                return Unauthorized(CreateUnauthorizedResponse());
            }

            if (id <= 0)
            {
                return BadRequest(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData, $"Invalid category id: {id}"));
            }

            var category = await _carCategoryRepository.GetCategoryStatisticsById(id);

            if (category == null)
            {
                return NotFound(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.ResourceNotFound, "Car category not found."));
            }

            return Ok(ApiValueResponseDto<CarCategoryStatisticsDto>.CreateSuccessfulResponse(_mapper.Map<CarCategoryStatisticsDto>(category)));
        }

        #endregion
    }
}
