using FribergCarRentals.Shared.Dto.Api;
using FribergCarRentals.Shared;
using FribergFastigheter.Shared.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FribergCarRentalsApi.Controllers
{
    /// <summary>
    /// Base class for API controllers.
    /// </summary>
    public abstract class ApiControllerBase : ControllerBase
    {
        #region Fields

        /// <summary>
        /// The injected authorization service.
        /// </summary>
        protected readonly IAuthorizationService _authorizationService;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="authorizationService">The injected authorization service.</param>
        protected ApiControllerBase(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates an <see cref="ApiResponseDto{T}"/> response object for unathorized users.
        /// </summary>
        /// <typeparam name="T">The for the <see cref="ApiResponseDto{T}"/> object.</typeparam>
        /// <returns>An <see cref="ApiResponseDto{T}"/> object.</returns>
        protected ApiResponseDto CreateUnauthorizedResponse<T>(string? overrideMessage = null) where T : class
        {
            string message = overrideMessage ?? "Authorization failed.";
            return ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.AuthorizationError.ToString(),
                    message);
        }

        /// <summary>
        /// Checks whether the user is authorizated against a policy.
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <returns>True if the user is authorized.</returns>
        protected async Task<bool> IsAuthorized(string policy)
        {
            var result = await _authorizationService.AuthorizeAsync(User, policy);
            return result.Succeeded;
        }

        #endregion
    }
}
