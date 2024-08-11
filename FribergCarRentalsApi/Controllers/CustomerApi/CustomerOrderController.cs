using AutoMapper;
using FribergCarRentals.Data.Entities;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Data.Repositories;
using FribergCarRentals.Shared;
using FribergCarRentals.Shared.Constants;
using FribergCarRentals.Shared.Models.Dto.Api;
using FribergCarRentals.Shared.Models.Dto.Car;
using FribergCarRentals.Shared.Models.Dto.CarCategory;
using FribergCarRentals.Shared.Models.Dto.Order;
using FribergCarRentals.Shared.Types.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FribergCarRentalsApi.Controllers.CustomerApi
{
    /// <summary>
    /// Handles customer related activites like booking a car and place an order for the booking.
    /// </summary>
    [Route("customer-api/customer/order")]
    [ApiController]
    public class CustomerOrderController : ApiControllerBase
    {
        #region Fields

        /// <summary>
        /// // The injected signin manager.
        /// </summary>
        protected readonly SignInManager<ApplicationUser> _signInManager;

        /// <summary>
        /// The injected car category repository.
        /// </summary>
        private readonly ICarCategoryRepository _carCategoryRepository;

        /// <summary>
        /// The injected car repository.
        /// </summary>
        private readonly ICarRepository _carRepository;
        /// <summary>
        ///The injected customer repository. 
        /// </summary>
        private readonly ICustomerRepository _customerRepository;

        /// <summary>
        ///The injected Auto Mapper. 
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The injected order repository.
        /// </summary>
        private readonly ICarOrderRepository _orderRepository;

        /// <summary>
        ///The injected user manager. 
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="customerRepository">The injected customer repository.</param>
        /// <param name="signInManager">The injected signin manager.</param>
        /// <param name="userManager">The injected user manager.</param>
        /// <param name="mapper">The injected Auto Mapper.</param>
        /// <param name="carCategoryRepository">The injected car category repository.</param>
        /// <param name="carRepository">The injected car repository.</param>
        /// <param name="orderRepository">The injected order repository.</param>
        public CustomerOrderController(ICustomerRepository customerRepository, SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IMapper mapper, ICarCategoryRepository carCategoryRepository, ICarRepository carRepository, ICarOrderRepository orderRepository, 
            IAuthorizationService authorizationService)
            : base(authorizationService)
        {
            _customerRepository = customerRepository;
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _carCategoryRepository = carCategoryRepository;
            _carRepository = carRepository;
            _orderRepository = orderRepository;
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Attempts to cancel an order.
        /// </summary>
        /// <param name="id">The ID of the order.</param>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        [HttpPost("{id}/cancel")]
        [ProducesResponseType<ApiValueResponseDto<CarOrderDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CancelOrder(int id)
        {
            if (!await IsAuthorized(ApplicationUserPolicies.Customer))
            {
                return Unauthorized(CreateUnauthorizedResponse());
            }

            if (id <= 0)
            {
                return BadRequest(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData, $"Invalid order ID: {id}"));
            }

            if (!await _orderRepository.Exists(id))
            {
                return BadRequest(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.ResourceNotFound, "Order was not found."));
            }

            if (await _orderRepository.TryCancelOrderAsync(id))
            {
                var order = _mapper.Map<CarOrderDto>(await _orderRepository.GetByIdAsync(id));
                return Ok(ApiValueResponseDto<CarOrderDto>.CreateSuccessfulResponse(order));
            }

            return BadRequest(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData, $"Failed to cancel order with id {id}. Please check the status of the order."));
        }

        /// <summary>
        /// Creates an order.
        /// </summary>
        /// <param name="createCarOrderDto">The input data for the new order.</param>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        [HttpPost]
        [ProducesResponseType<ApiValueResponseDto<CarOrderDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create(CreateCarOrderDto createCarOrderDto)
        {
            if (!await IsAuthorized(ApplicationUserPolicies.Customer))
            {
                return Unauthorized(CreateUnauthorizedResponse());
            }

            if (!ValidatePickupDate(createCarOrderDto.PickupDateLocalTime))
            {
                return BadRequest(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData.ToString(),
                    ValidationMessages.PickupDateMustBeInFutureErrorMessage));
            }

            if (!ValidateReturnDate(createCarOrderDto.PickupDateLocalTime, createCarOrderDto.ReturnDateLocalTime))
            {
                return BadRequest(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData.ToString(),
                    ValidationMessages.ReturnDateOccursBeforePickupDateErrorMessage));
            }

            if (createCarOrderDto.CarId <= 0)
            {
                return BadRequest(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData.ToString(),
                    "Invalid car ID."));
            }

            int customerId = int.Parse(User.FindFirstValue(ApplicationUserClaims.CustomerId)!);
            var customer = await _customerRepository.GetByIdAsync(customerId);
            var car = await _carRepository.GetByIdAsync(createCarOrderDto.CarId);

            if (customer == null)
            {
                return NotFound(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.ResourceNotFound, "Customer was not found."));
            }

            if (car == null)
            {
                return NotFound(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.ResourceNotFound, "Car was not found."));
            }

            car.RentalStatus = CarRentalStatusEntity.CreateFromType(RentalCarStatus.PendingPickup);
            var order = new CarOrderEntity(customer);
            order.CarBookings.Add(
                new CarBookingEntity(order, car, pickupDateUTC: createCarOrderDto.PickupDateLocalTime.Date,
                    returnDateUTC: createCarOrderDto.ReturnDateLocalTime.Date));
            await _orderRepository.AddAsync(order);

            return Ok(ApiValueResponseDto<CarOrderDto>.CreateSuccessfulResponse(_mapper.Map<CarOrderDto>(order)));
        }

        /// <summary>
        /// Gets all orders.
        /// </summary>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        [HttpGet]
        [ProducesResponseType<ApiValueResponseDto<List<CarOrderDto>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllOrders()
        {
            if (!await IsAuthorized(ApplicationUserPolicies.Customer))
            {
                return Unauthorized(CreateUnauthorizedResponse());
            }

            int customerId = int.Parse(User.FindFirstValue(ApplicationUserClaims.CustomerId)!);
            var customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer == null)
            {
                return NotFound(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.ResourceNotFound, "Customer was not found."));
            }

            return Ok(ApiValueResponseDto<List<CarOrderDto>>.CreateSuccessfulResponse(_mapper.Map<List<CarOrderDto>>(customer.Orders)));
        }

        /// <summary>
        /// Returns all car categories.
        /// </summary>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        [HttpGet("car-categories")]
        [ProducesResponseType<ApiValueResponseDto<CarCategoryDto>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCarCategories()
        {
            var carCategories = _mapper.Map<List<CarCategoryDto>>(await _carCategoryRepository.GetAllAsync());
            return Ok(ApiValueResponseDto<List<CarCategoryDto>>.CreateSuccessfulResponse(carCategories));
        }

        /// <summary>
        /// Gets an order by ID.
        /// </summary>
        /// <param name="id">The ID of the order.</param>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("{id}")]
        [ProducesResponseType<ApiValueResponseDto<CarOrderDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderById(int id)
        {
            if (!await IsAuthorized(ApplicationUserPolicies.Customer))
            {
                return Unauthorized(CreateUnauthorizedResponse());
            }

            if (id < 0)
            {
                throw new Exception($"Invalid ID: {id}");
            }

            var order = await _orderRepository.GetByIdAsync(id);

            if (order is not null)
            {
                return Ok(ApiValueResponseDto<CarOrderDto>.CreateSuccessfulResponse(_mapper.Map<CarOrderDto>(order)));
            }

            return NotFound(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.ResourceNotFound, "Order was not found."));
        }

        /// <summary>
        /// Searches for rentable cars.
        /// </summary>
        /// <param name="carRentalSearchDto">The search parameters.</param>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        [HttpPost("search-cars")]
        [ProducesResponseType<ApiValueResponseDto<CarRentalSearchResultDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchRentalCars(CarRentalSearchDto carRentalSearchDto)
        {
            if (!ValidatePickupDate(carRentalSearchDto.PickupDateLocalTime))
            {
                return BadRequest(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData.ToString(),
                    ValidationMessages.PickupDateMustBeInFutureErrorMessage));
            }
            else if (!ValidateReturnDate(carRentalSearchDto.PickupDateLocalTime, carRentalSearchDto.ReturnDateLocalTime))
            {
                return BadRequest(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData.ToString(),
                    ValidationMessages.ReturnDateOccursBeforePickupDateErrorMessage));
            }
            else if (carRentalSearchDto.SelectedCarCategoryFilter != null && !await _carCategoryRepository.CategoryExists(carRentalSearchDto.SelectedCarCategoryFilter.Value))
            {
                return BadRequest(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.ResourceNotFound.ToString(),
                    "Failed to find the car category"));
            }

            var cars = (await _carRepository.GetRentableCarsAsync(carRentalSearchDto.PickupDateLocalTime, carRentalSearchDto.ReturnDateLocalTime,
                carRentalSearchDto.SelectedCarCategoryFilter)).ToList();

            return Ok(ApiValueResponseDto<CarRentalSearchResultDto>.CreateSuccessfulResponse(new CarRentalSearchResultDto(_mapper.Map<List<CarDto>>(cars))));
        }

        #endregion

        #region OtherMethods

        /// <summary>
        /// Validates the pickup date for car rentals.
        /// </summary>
        /// <param name="pickupDate">The pickup date.</param>
        /// <returns>True if the date is valid.</returns>
        private bool ValidatePickupDate(DateTime pickupDate)
        {
            return pickupDate.Date > DateTime.Now.Date;
        }

        /// <summary>
        /// Validates the return date for car rentals.
        /// </summary>
        /// <param name="pickupDate">The pickup date.</param>
        /// <param name="returnDate">The return date.</param>
        /// <returns>True if the date is valid.</returns>
        private bool ValidateReturnDate(DateTime pickupDate, DateTime returnDate)
        {
            return returnDate >= pickupDate;
        }

        #endregion
    }
}
