using AutoMapper;
using FribergCarRentals.Data.Entities;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Data.Exceptions;
using FribergCarRentals.Data.Repositories;
using FribergCarRentals.Shared;
using FribergCarRentals.Shared.Dto.Api;
using FribergCarRentals.Shared.Dto.Customer;
using FribergCarRentals.Shared.Dto.User;
using FribergCarRentalsApi.Services;
using FribergFastigheter.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;
using System.Text;

namespace FribergCarRentalsApi.Controllers.CustomerApi
{
    /// <summary>
    /// Handles customer related activites like registration and login. 
    /// </summary>
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : ApiControllerBase
    {
        #region Fields

        /// <summary>
        /// The injected signin manager.
        /// </summary>
        protected readonly SignInManager<ApplicationUser> _signInManager;

        /// <summary>
        /// The injected customer repository. 
        /// </summary>
        private readonly ICustomerRepository _customerRepository;

        /// <summary>
        /// The injected Auto Mapper. 
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The injected token service. 
        /// </summary>
        private readonly ITokenService _tokenService;

        /// <summary>
        /// The injected user manager. 
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
        /// <param name="tokenService">The injected token service. </param>
        /// <param name="authorizationService">The injected authorization service.</param>
        public CustomerController(ICustomerRepository customerRepository, SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IMapper mapper, ITokenService tokenService, IAuthorizationService authorizationService) 
            : base(authorizationService) 
        {
            _customerRepository = customerRepository;
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Confirms the email of a customer.
        /// </summary>
        /// <param name="confirmEmailDto"></param>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        [HttpPost("confirm-email")]
        [ProducesResponseType<ApiResponseDto<LoginUserResponseDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto<LoginUserResponseDto>>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ApiResponseDto<LoginUserResponseDto>>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailDto confirmEmailDto)
        {
            var customer = await _customerRepository.GetByEmailAsync(confirmEmailDto.Email);

            if (customer != null)
            {
                string decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(confirmEmailDto.Code));
                var confirmResult = await _userManager.ConfirmEmailAsync(customer.User, decodedCode);

                if (confirmResult.Succeeded)
                {
                    return Ok(await CreateLoginResponse(customer));
                }
                else
                {
                    return Unauthorized(ApiResponseDto<LoginUserResponseDto>.CreateErrorResponse(ApiErrorMessageTypes.EmailConfirmationFailed.ToString(), "The email confirmation failed."));
                }
            }
            else
            {
                return NotFound(ApiResponseDto<LoginUserResponseDto>.CreateErrorResponse(ApiErrorMessageTypes.UserLoginFailed.ToString(), "The customer was not found."));
            }
        }

        /// <summary>
        /// Creates a new customer.
        /// </summary>
        /// <param name="createCustomerDto"></param>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        [HttpPost("create")]
        [ProducesResponseType<ApiResponseDto<CreatedCustomerDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto<CreatedCustomerDto>>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerDto createCustomerDto)
        {
            var customer = new CustomerEntity(_mapper.Map<ApplicationUser>(createCustomerDto));

            if (await _customerRepository.CustomerExists(customer.User.Email!))
            {
                return BadRequest(ApiResponseDto<CreatedCustomerDto>.CreateErrorResponse(ApiErrorMessageTypes.UserExist.ToString(), $"A customer with email '{createCustomerDto.Email}' already exists."));
            }
            else
            {
                try
                {
                    await _customerRepository.AddAsync(customer);
                    CreatedCustomerDto createdCustomerDto = _mapper.Map<CreatedCustomerDto>(customer);

                    if (_userManager.Options.SignIn.RequireConfirmedEmail)
                    {
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(customer.User);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        createdCustomerDto.ConfirmEmailLink = Url.Action(nameof(ConfirmEmail), new { userId = customer.User.Id, code = code });
                    }
                    else
                    {
                        createdCustomerDto.Token = await _tokenService.CreateToken(customer);
                    }

                    return Ok(ApiResponseDto<CreatedCustomerDto>.CreateSuccessfulResponse(createdCustomerDto));
                }
                catch (CreateUserException ex)
                {
                    return BadRequest(ApiResponseDto<CreatedCustomerDto>.CreateErrorResponse(ApiErrorMessageTypes.UserCreationFailed.ToString(), ex.Message));
                }
            }
        }

        /// <summary>
        /// Gets a customer by ID.
        /// </summary>
        /// <param name="id">The ID of the customer.</param>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType<ApiResponseDto<CustomerDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto<CustomerDto>>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ApiResponseDto<CustomerDto>>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            if (!await IsAuthorized(ApplicationUserPolicies.Customer))
            {
                return Unauthorized(CreateUnauthorizedResponse<CustomerDto>());
            }

            int userId = int.Parse(User.FindFirstValue(ApplicationUserClaims.CustomerId)!);

            if (id != userId)
            {
                return Unauthorized(CreateUnauthorizedResponse<CustomerDto>("You are not authorized to fetch data for other customers."));
            }

            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer != null)
            {
                return Ok(_mapper.Map<CustomerDto>(customer));
            }
            else
            {
                return NotFound(ApiResponseDto<CustomerDto>.CreateErrorResponse(ApiErrorMessageTypes.UserNotFound.ToString(), "The customer was not found."));
            }
        }

        /// <summary>
        /// Attempts to login a customer.
        /// </summary>
        /// <param name="credentials">The credentials for the login.</param>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        [HttpPost("login")]
        [ProducesResponseType<ApiResponseDto<LoginUserResponseDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto<LoginUserResponseDto>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponseDto<LoginUserResponseDto>>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ApiResponseDto<LoginUserResponseDto>>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> LoginCustomer([FromBody] LoginCustomerDto credentials)
        {
            var customer = await _customerRepository.GetByEmailAsync(credentials.Email);

            if (customer != null)
            {
                if (_userManager.Options.SignIn.RequireConfirmedEmail && !await _customerRepository.IsEmailConfirmedAsync(customer))
                {
                    return BadRequest(ApiResponseDto<LoginUserResponseDto>.CreateErrorResponse(ApiErrorMessageTypes.UserLoginFailed.ToString(), "The email address must be confirmed before logging in."));
                }

                var result = await _signInManager.CheckPasswordSignInAsync(customer.User, credentials.Password, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return Ok(await CreateLoginResponse(customer));
                }
                else
                {
                    if (result.IsLockedOut)
                    {
                        return Unauthorized(ApiResponseDto<LoginUserResponseDto>.CreateErrorResponse(ApiErrorMessageTypes.UserLoginFailed.ToString(), "The user is locked out."));
                    }
                    else if (result.IsNotAllowed)
                    {
                        return Unauthorized(ApiResponseDto<LoginUserResponseDto>.CreateErrorResponse(ApiErrorMessageTypes.UserLoginFailed.ToString(), "The user is not allowed to login."));
                    }
                    else
                    {
                        return Unauthorized(ApiResponseDto<LoginUserResponseDto>.CreateErrorResponse(ApiErrorMessageTypes.UserLoginFailed.ToString(), "Invalid credentials."));
                    }
                }
            }
            else
            {
                return NotFound(ApiResponseDto<LoginUserResponseDto>.CreateErrorResponse(ApiErrorMessageTypes.UserLoginFailed.ToString(), "Invalid credentials."));
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a response for a successful login containing user information and a JWT token. 
        /// </summary>
        /// <param name="customer">The customer</param>
        /// <returns></returns>
        private async Task<ApiResponseDto<LoginUserResponseDto>> CreateLoginResponse(CustomerEntity customer)
        {
            return ApiResponseDto<LoginUserResponseDto>.CreateSuccessfulResponse(
                new LoginUserResponseDto()
                {
                    Email = customer.User!.Email!,
                    Token = await _tokenService.CreateToken(customer)
                });
        }

        #endregion
    }
}
