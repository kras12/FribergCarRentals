using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using FribergCarRentals.Shared.Constants;
using FribergCarRentals.Shared.Models.Dto.Api;
using FribergCarRentals.Shared.Models.Dto.Order;
using FribergCarRentals.Shared.Models.Dto.Image;
using FribergCarRentals.Shared.Enums;

namespace FribergCarRentalsApi.Controllers.AdminApi
{
    /// <summary>
    /// Handles admin related activites for orders.
    /// </summary>
    [Route("admin-api/order/")]
    [ApiController]
    public class AdminOrderController : ApiControllerBase
    {
        #region Fields

        // The injected Auto Mapper.
        private readonly IMapper _mapper;

        /// <summary>
        /// The injected order repository.
        /// </summary>
        private readonly ICarOrderRepository _orderRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="orderRepository">The injected order repository.</param>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="mapper">The injected Auto Mapper.</param>
        public AdminOrderController(ICarOrderRepository orderRepository, IAuthorizationService authorizationService, IMapper mapper) : base(authorizationService)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Completes an order. 
        /// </summary>
        /// <param name="id">The ID of the order. </param>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        [ProducesResponseType<ApiValueResponseDto<CarOrderDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status404NotFound)]
        [HttpPut("{id}/complete")]
        public async Task<IActionResult> Complete(int id)
        {
            if (!await IsAuthorized(ApplicationUserPolicies.Admin))
            {
                return Unauthorized(CreateUnauthorizedResponse());
            }

            if (id <= 0)
            {
                return BadRequest(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData, $"Invalid order ID: {id}"));
            }

            if (!await _orderRepository.Exists(id))
            {
                return NotFound(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData, $"Failed to find an order with ID: {id}"));
            }

            if (!await _orderRepository.TryCompleteOrderAsync(id))
            {
                return BadRequest(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.ResourceNotModified, "Failed to complete order. Please check the status of the order."));
            }

            var order = await _orderRepository.GetByIdAsync(id);

            return Ok(ApiValueResponseDto<CarOrderDto>.CreateSuccessfulResponse(_mapper.Map<CarOrderDto>(order)));            
        }

        /// <summary>
        /// Deletes an order.
        /// </summary>
        /// <param name="id">The ID of the order. </param>
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
                return BadRequest(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData, $"Invalid order ID: {id}"));
            }

            if (!await _orderRepository.Exists(id))
            {
                return NotFound(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData, $"Failed to find an order with ID: {id}"));
            }

            await _orderRepository.DeleteOrderAsync(id);
            
            return Ok(ApiResponseDto.CreateSuccessfulResponse());
        }


        /// <summary>
        /// Gets an order by ID.
        /// </summary>
        /// <param name="id">The ID for the order.</param>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        [ProducesResponseType<ApiValueResponseDto<CarOrderDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            if (!await IsAuthorized(ApplicationUserPolicies.Admin))
            {
                return Unauthorized(CreateUnauthorizedResponse());
            }

            if (id <= 0)
            {
                return BadRequest(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData, $"Invalid order ID: {id}"));
            }

            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
            {
                return NotFound(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData, $"Failed to find an order with ID: {id}"));
            }

            var orderDto = _mapper.Map<CarOrderDto>(order);
            SetImageUrls(orderDto.CarBooking.Car.Images.ToList());

            return Ok(ApiValueResponseDto<CarOrderDto>.CreateSuccessfulResponse(orderDto));
        }

        /// <summary>
        /// Gets all orders.
        /// </summary>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        [ProducesResponseType<ApiValueResponseDto<List<CarOrderDto>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        public async Task<IActionResult> List()
        {
            if (!await IsAuthorized(ApplicationUserPolicies.Admin))
            {
                return Unauthorized(CreateUnauthorizedResponse());
            }

            var orders = _mapper.Map<List<CarOrderDto>>((await _orderRepository.GetAllAsync()).ToList());
            SetImageUrls(orders.SelectMany(x => x.CarBooking.Car.Images).ToList());

            return Ok(ApiValueResponseDto<List<CarOrderDto>>.CreateSuccessfulResponse(orders));
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
