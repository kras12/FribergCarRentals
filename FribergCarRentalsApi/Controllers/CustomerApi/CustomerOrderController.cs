using AutoMapper;
using FribergCarRentals.Data.Entities;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Data.Repositories;
using FribergCarRentals.Data.Types;
using FribergCarRentals.Shared;
using FribergCarRentals.Shared.Dto.Api;
using FribergCarRentals.Shared.Dto.Car;
using FribergCarRentals.Shared.Dto.Order;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcRazorPages.Shared.ViewModels.Order;

namespace FribergCarRentalsApi.Controllers.CustomerApi
{
    /// <summary>
    /// Handles customer related activites like booking a car and place an order for the booking.
    /// </summary>
    [Route("api/customer/order")]
    [ApiController]
    public class CustomerOrderController : ControllerBase
    {
        #region ErrorMessages

        /// <summary>
        /// Error message for when the pickup date is not in the future. 
        /// </summary>
        private const string PickupDateMustBeInFutureErrorMessage = "The pickup date must be at least one day into the future.";

        /// <summary>
        /// Error message for when the return date occurrs before the pickup date.
        /// </summary>
        private const string ReturnDateOccursBeforePickupDateErrorMessage = "The return date can't occur before the pickup date.";

        #endregion

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
        /// // The injected signin manager.
        /// </summary>
        protected readonly SignInManager<ApplicationUser> _signInManager;

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
            IMapper mapper, ICarCategoryRepository carCategoryRepository, ICarRepository carRepository, ICarOrderRepository orderRepository)
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

        [HttpGet("car-categories")]
        public async Task<IActionResult> GetCarCategories()
        {
            var carCategories = _mapper.Map<List<CarCategoryDto>>(await _carCategoryRepository.GetAllAsync());
            return Ok(ApiResponseDto<List<CarCategoryDto>>.CreateSuccessfulResponse(carCategories));
        }

        [HttpPost("search-cars")]
        public async Task<IActionResult> SearchRentalCars(CarRentalSearchDto carRentalSearchDto)
        {
            if (!ValidatePickupDate(carRentalSearchDto.PickupDateLocalTime))
            {
                return BadRequest(ApiResponseDto<CarBookingDto>.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData.ToString(),
                    PickupDateMustBeInFutureErrorMessage));
            }
            else if (!ValidateReturnDate(carRentalSearchDto.PickupDateLocalTime, carRentalSearchDto.ReturnDateLocalTime))
            {
                return BadRequest(ApiResponseDto<CarBookingDto>.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData.ToString(),
                    ReturnDateOccursBeforePickupDateErrorMessage));
            }
            else if (carRentalSearchDto.SelectedCarCategoryFilter != null && !await _carCategoryRepository.CategoryExists(carRentalSearchDto.SelectedCarCategoryFilter.Value))
            {
                return BadRequest(ApiResponseDto<CarBookingDto>.CreateErrorResponse(ApiErrorMessageTypes.ResourceNotFound.ToString(),
                    "Failed to find the car category"));
            }

            var cars = (await _carRepository.GetRentableCarsAsync(carRentalSearchDto.PickupDateLocalTime, carRentalSearchDto.ReturnDateLocalTime,
                carRentalSearchDto.SelectedCarCategoryFilter)).ToList();

            return Ok(ApiResponseDto<CarRentalSearchResultDto>.CreateSuccessfulResponse(new CarRentalSearchResultDto(_mapper.Map<List<CarDto>>(cars))));
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            if (id <= 0)
            {
                return BadRequest(ApiResponseDto<CarOrderDto>.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData, $"Invalid order ID: {id}"));
            }

            if (!await _orderRepository.Exists(id))
            {
                return BadRequest(ApiResponseDto<CarOrderDto>.CreateErrorResponse(ApiErrorMessageTypes.ResourceNotFound, "Order was not found."));
            }

            if (await _orderRepository.TryCancelOrderAsync(id))
            {
                var order = _mapper.Map<CarOrderDto>(await _orderRepository.GetByIdAsync(id));
                return Ok(ApiResponseDto<CarOrderDto>.CreateSuccessfulResponse(order));
            }

            return BadRequest(ApiResponseDto<CarOrderDto>.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData, $"Failed to cancel order with id {id}. Please check the status of the order."));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCarOrderDto createCarOrderDto)
        {
            if (!ValidatePickupDate(createCarOrderDto.PickupDateLocalTime))
            {
                return BadRequest(ApiResponseDto<CarBookingDto>.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData.ToString(),
                    PickupDateMustBeInFutureErrorMessage));
            }
            else if (!ValidateReturnDate(createCarOrderDto.PickupDateLocalTime, createCarOrderDto.ReturnDateLocalTime))
            {
                return BadRequest(ApiResponseDto<CarBookingDto>.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData.ToString(),
                    ReturnDateOccursBeforePickupDateErrorMessage));
            }
            else if (createCarOrderDto.CarId <= 0)
            {
                return BadRequest(ApiResponseDto<CarBookingDto>.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData.ToString(),
                    "Invalid car ID."));
            }

            // TODO - Uncomment
            //var userId = User.FindFirst(x => x.Type == ApplicationUserClaims.UserId)!.Value;
            //var customer = await _customerRepository.GetByUserIdAsync(userId);
            //var car = await _carRepository.GetByIdAsync(createCarOrderDto.CarId);

            // TODO - remove
            var customer = await _customerRepository.GetByEmailAsync("kalle@ankeborg.com");
            var car = await _carRepository.GetByIdAsync(createCarOrderDto.CarId);

            if (customer == null)
            {
                return NotFound(ApiResponseDto<CarOrderDto>.CreateErrorResponse(ApiErrorMessageTypes.ResourceNotFound, "Customer was not found."));
            }

            if (car == null)
            {
                return NotFound(ApiResponseDto<CarOrderDto>.CreateErrorResponse(ApiErrorMessageTypes.ResourceNotFound, "Car was not found."));
            }


            

            car.RentalStatus = CarRentalStatusEntity.CreateFromType(RentalCarStatus.PendingPickup);
            var order = new CarOrderEntity(customer);
            order.CarBookings.Add(
                new CarBookingEntity(order, car, pickupDateUTC: createCarOrderDto.PickupDateLocalTime.Date,
                    returnDateUTC: createCarOrderDto.ReturnDateLocalTime.Date));
            await _orderRepository.AddAsync(order);

            return Ok(ApiResponseDto<CarOrderDto>.CreateSuccessfulResponse(_mapper.Map<CarOrderDto>(order)));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            if (id < 0)
            {
                throw new Exception($"Invalid ID: {id}");
            }

            var order = await _orderRepository.GetByIdAsync(id);

            if (order is not null)
            {
                return Ok(ApiResponseDto<CarOrderDto>.CreateSuccessfulResponse(_mapper.Map<CarOrderDto>(order)));
            }

            return NotFound(ApiResponseDto<CarOrderDto>.CreateErrorResponse(ApiErrorMessageTypes.ResourceNotFound, "Order was not found."));
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            // TODO - Uncomment
            //var userId = User.FindFirst(x => x.Type == ApplicationUserClaims.UserId)!.Value;
            //var customer = await _customerRepository.GetByUserIdAsync(userId);


            // TODO - remove
            var customer = await _customerRepository.GetByEmailAsync("kalle@ankeborg.com");


            if (customer == null)
            {
                return NotFound(ApiResponseDto<CarOrderDto>.CreateErrorResponse(ApiErrorMessageTypes.ResourceNotFound, "Customer was not found."));
            }

            return Ok(ApiResponseDto<List<CarOrderDto>>.CreateSuccessfulResponse(_mapper.Map<List<CarOrderDto>>(customer.Orders)));
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
