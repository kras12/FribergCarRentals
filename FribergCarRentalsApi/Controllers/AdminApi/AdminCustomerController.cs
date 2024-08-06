using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Data.Repositories;
using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using FribergCarRentals.Data.Exceptions;
using FribergCarRentalsApi.Controllers;
using FribergCarRentals.Shared;
using FribergCarRentals.Shared.Constants;
using FribergCarRentals.Shared.Models.Dto.Api;
using FribergCarRentals.Shared.Models.Dto.Customer;

namespace FribergCarRentals.Areas.Admin.Controllers
{
    /// <summary>
    /// Handles admin related activites for customers.
    /// </summary>
    [Route("api/admin/customer/")]
    [ApiController]
    public class AdminCustomerController : ApiControllerBase
    {
        #region Fields

        /// <summary>
        /// The injected customer repository.
        /// </summary>
        private readonly ICustomerRepository _customerRepository;

        // The injected Auto Mapper.
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="customerRepository">The injected customer repository.</param>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="mapper">The injected Auto Mapper.</param>
        public AdminCustomerController(ICustomerRepository customerRepository, IAuthorizationService authorizationService,
            IMapper mapper) : base(authorizationService)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        #endregion

        #region Endpoints        

        /// <summary>
        /// Creates a new customer.
        /// </summary>
        /// <param name="createCustomerDto">The input data for the customer.</param>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        [ProducesResponseType<ApiValueResponseDto<CreatedCustomerDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCustomerDto createCustomerDto)
        {
            if (!await IsAuthorized(ApplicationUserPolicies.Admin))
            {
                return Unauthorized(CreateUnauthorizedResponse());
            }

            var customer = new CustomerEntity(_mapper.Map<ApplicationUser>(createCustomerDto));

            if (await _customerRepository.CustomerExists(customer.User.Email!))
            {
                return NotFound(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.UserExist, $"An user already exists with email: {customer.User.Email!}"));
            }
            else
            {
                try
                {
                    await _customerRepository.AddAsync(customer);                    
                    return Ok(ApiValueResponseDto<CreatedCustomerDto>.CreateSuccessfulResponse(_mapper.Map<CreatedCustomerDto>(customer)));                    
                }
                catch (CreateUserException ex)
                {
                    return BadRequest(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.UserCreationFailed.ToString(), ex.Message));
                }
            }
        }


        /// <summary>
        /// Deletes a customer
        /// </summary>
        /// <param name="id">The ID for the customer.</param>
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
                return BadRequest(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData, $"Invalid customer id: {id}"));
            }

            if (!await _customerRepository.CustomerExists(id))
            {
                return NotFound(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.ResourceNotFound, $"Failed to find a customer with ID: {id}"));
            }

            await _customerRepository.DeleteAsync(id);

            return Ok(ApiResponseDto.CreateSuccessfulResponse());
        }


        /// <summary>
        /// Gets a customer by ID.
        /// </summary>
        /// <param name="id">The ID for the customer.</param>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        [ProducesResponseType<ApiValueResponseDto<CustomerDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            if (!await IsAuthorized(ApplicationUserPolicies.Admin))
            {
                return Unauthorized(CreateUnauthorizedResponse());
            }

            if (id <= 0)
            {
                return BadRequest(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData, $"Invalid customer id: {id}"));
            }

            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
            {
                return NotFound(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.ResourceNotFound, $"Failed to find a customer with ID: {id}"));
            }

            return Ok(ApiValueResponseDto<CustomerDto>.CreateSuccessfulResponse(_mapper.Map<CustomerDto>(customer)));
        }

        /// <summary>
        /// Edits a customer.
        /// </summary>
        /// <param name="editCustomerDto">The input data for the customer.</param>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        [ProducesResponseType<ApiValueResponseDto<CustomerDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, EditCustomerDto editCustomerDto)
        {
            if (!await IsAuthorized(ApplicationUserPolicies.Admin))
            {
                return Unauthorized(CreateUnauthorizedResponse());
            }

            if (id <= 0)
            {
                return BadRequest(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData, $"Invalid customer id: {id}"));
            }

            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
            {
                return NotFound(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.ResourceNotFound, $"Failed to find a customer with ID: {id}"));
            }

            _mapper.Map(editCustomerDto, customer.User);
            await _customerRepository.UpdateAsync(customer);

            return Ok(ApiValueResponseDto<CustomerDto>.CreateSuccessfulResponse(_mapper.Map<CustomerDto>(customer)));    
        }


        /// <summary>
        /// Gets all customers.
        /// </summary>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        [ProducesResponseType<ApiValueResponseDto<List<CustomerDto>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        public async Task<IActionResult> List()
        {
            if (!await IsAuthorized(ApplicationUserPolicies.Admin))
            {
                return Unauthorized(CreateUnauthorizedResponse());
            }

            var customers = _mapper.Map<List<CustomerDto>>((await _customerRepository.GetAllAsync()).ToList());
            return Ok(ApiValueResponseDto<List<CustomerDto>>.CreateSuccessfulResponse(customers));
        }

        /// <summary>
        /// Gets the confirm email link for the customer.
        /// </summary>
        /// <param name="id">The ID of the customer.</param>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        [ProducesResponseType<ApiValueResponseDto<CustomerDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status404NotFound)]
        [HttpGet("{id}/email-confirm-link")]
        public async Task<IActionResult> ResendConfirmEmailLink(int id)
        {
            // Since we haven't implemented code for sending emails to the customer, 
            // we will manually confirm the emails ourselves for now. 

            if (!await IsAuthorized(ApplicationUserPolicies.Admin))
            {
                return Unauthorized(CreateUnauthorizedResponse());
            }

            if (!await _customerRepository.CustomerExists(id))
            {
                return NotFound(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.ResourceNotFound, $"Failed to find a customer with ID: {id}"));
            }
            
            string code = await _customerRepository.GenerateEmailConfirmationTokenAsync(id);
            var confirmResult = await _customerRepository.ConfirmEmailAsync(id, code);

            if (!confirmResult.Succeeded)
            {
                return BadRequest(ApiResponseDto.CreateErrorResponse(confirmResult.Errors.Select(x => new KeyValuePair<string, string>(x.Code, x.Description)).ToList()));
            }

            return Ok(ApiResponseDto.CreateSuccessfulResponse());
        }

        #endregion
    }
}
