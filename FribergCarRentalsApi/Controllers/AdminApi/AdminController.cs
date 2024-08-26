using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FribergCarRentals.Data.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using FribergCarRentals.Data.Entities;
using FribergCarRentalsApi.Services;
using FribergCarRentals.Shared.Constants;
using FribergCarRentals.Shared.Models.Dto.Admin;
using FribergCarRentals.Shared.Models.Dto.Api;
using FribergCarRentals.Shared.Models.Dto.User;
using FribergCarRentals.Shared.Enums;

namespace FribergCarRentalsApi.Controllers.AdminApi
{
    /// <summary>
    /// Handles admin related activites like login and fetching user data.
    /// </summary>
    [Route("admin-api/admin")]
    [ApiController]
    public class AdminController : ApiControllerBase
    {
        #region Fields

        /// <summary>
        /// The injected admin repository.
        /// </summary>
        private readonly IAdminRepository _adminRepository;

        /// <summary>
        /// The injected Auto Mapper. 
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The injected signin manager.
        /// </summary>
        protected readonly SignInManager<ApplicationUser> _signInManager;

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
        /// Constructor.
        /// </summary>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="adminRepository">The injected admin repository.</param>
        /// <param name="mapper">The injected Auto Mapper.</param>
        /// <param name="signInManager">The injected signin manager.</param>
        /// <param name="tokenService">The injected token service.</param>
        /// <param name="userManager">The injected user manager. </param>
        public AdminController(IAuthorizationService authorizationService, IAdminRepository adminRepository, IMapper mapper, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ITokenService tokenService) : base(authorizationService)
        {
            _adminRepository = adminRepository;
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Gets a admin by ID.
        /// </summary>
        /// <param name="id">The ID of the admin.</param>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType<ApiValueResponseDto<AdminDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAdminById(int id)
        {
            if (!await IsAuthorized(ApplicationUserPolicies.Admin))
            {
                return Unauthorized(CreateUnauthorizedResponse());
            }

            if (id <= 0)
            {
                return BadRequest(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.InvalidInputData, $"Invalid id: {id}"));
            }

            int adminId = int.Parse(User.FindFirstValue(ApplicationUserClaims.AdminId)!);            

            if (id != adminId)
            {
                return Unauthorized(CreateUnauthorizedResponse("You are not authorized to fetch data for other admins."));
            }

            var admin = await _adminRepository.GetByIdAsync(id);

            if (admin != null)
            {
                return Ok(ApiValueResponseDto<AdminDto>.CreateSuccessfulResponse(_mapper.Map<AdminDto>(admin)));
            }
            else
            {
                return NotFound(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.UserNotFound.ToString(), "The admin was not found."));
            }
        }

        /// <summary>
        /// Attempts to login an admin.
        /// </summary>
        /// <param name="credentials">The credentials for the login.</param>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        [HttpPost("login")]
        [ProducesResponseType<ApiValueResponseDto<LoginUserResponseDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> LoginAdmin([FromBody] LoginAdminDto credentials)
        {
            var admin = await _adminRepository.GetByEmailAsync(credentials.Email);

            if (admin != null)
            {
                if (_userManager.Options.SignIn.RequireConfirmedEmail && !await _adminRepository.IsEmailConfirmedAsync(admin))
                {
                    return BadRequest(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.UserLoginFailed.ToString(), "The email address must be confirmed before logging in."));
                }

                var result = await _signInManager.CheckPasswordSignInAsync(admin.User, credentials.Password, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return Ok(ApiValueResponseDto<LoginUserResponseDto>.CreateSuccessfulResponse(
                        new LoginUserResponseDto()
                        {
                            Email = admin.User!.Email!,
                            Token = await _tokenService.CreateToken(admin)
                        }));
                }
                else
                {
                    if (result.IsLockedOut)
                    {
                        return Unauthorized(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.UserLoginFailed.ToString(), "The user is locked out."));
                    }
                    else if (result.IsNotAllowed)
                    {
                        return Unauthorized(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.UserLoginFailed.ToString(), "The user is not allowed to login."));
                    }
                    else
                    {
                        return Unauthorized(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.UserLoginFailed.ToString(), "Invalid credentials."));
                    }
                }
            }
            else
            {
                return NotFound(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.UserLoginFailed.ToString(), "Invalid credentials."));
            }
        }

        #endregion

    }
}
