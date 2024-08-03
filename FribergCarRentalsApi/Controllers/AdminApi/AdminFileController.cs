using FribergCarRentals.Shared.Dto.Api;
using FribergCarRentals.Shared.Dto.Car;
using FribergCarRentals.Shared;
using FribergCarRentalsApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FribergFastigheter.Shared.Constants;

namespace FribergCarRentalsApi.Controllers.AdminApi
{
    /// <summary>
    /// Handles file transfers for the admin API.
    /// </summary>
    [Route("api/admin/file/")]
    [ApiController]
    public class AdminFileController : ApiControllerBase
    {
        #region Fields

        /// <summary>
        /// The injected image download service.
        /// </summary>
        private readonly IImageDownloadService _imageDownloadService;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="imageDownloadService">The injected image download service.</param>
        public AdminFileController(IAuthorizationService authorizationService, IImageDownloadService imageDownloadService)
            : base(authorizationService)
        {
            _imageDownloadService = imageDownloadService;
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// An API endpoint for retrieving an image file. 
        /// </summary>
        /// <param name="fileName">The file name of the image to fetch.</param>
        /// <returns>An embedded <see cref="FileResult"/> object.</returns>
        [HttpGet("image/{fileName}")]
        [ProducesResponseType<FileContentResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetImageFile(string fileName)
        {
            if (!await IsAuthorized(ApplicationUserPolicies.Admin))
            {
                return Unauthorized(CreateUnauthorizedResponse<CarCategoryDto>());
            }

            var fileResult = await _imageDownloadService.PrepareImageFileDownloadAsync(fileName);

            if (fileResult != null)
            {
                return fileResult;
            }
            else
            {
                return NotFound(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.ResourceNotFound, "The image doesn't exists"));
            }
        }

        #endregion
    }
}
