using FribergCarRentals.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Shared.Models.Dto.Api;
using FribergCarRentals.Shared.Mvc.Services;

namespace FribergCarRentalsApi.Controllers.AdminApi
{
    /// <summary>
    /// Handles file transfers for the customer API.
    /// </summary>
    [Route(ApiControllerRoute)]
    [ApiController]
    public class CustomerFileController : ApiControllerBase
    {
        #region Constants

        /// <summary>
        /// The route for the API controller.
        /// </summary>
        private const string ApiControllerRoute = "customer-api/file";

        /// <summary>
        /// The relative route for the image file endpoint
        /// </summary>
        private const string ImageFileEndpointRelativeRoute = "image";

        #endregion

        #region Fields

        /// <summary>
        /// The injected image download service.
        /// </summary>
        private readonly IImageApiDownloadService _imageDownloadService;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="imageDownloadService">The injected image download service.</param>
        public CustomerFileController(IAuthorizationService authorizationService, IImageApiDownloadService imageDownloadService)
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
        [HttpGet(ImageFileEndpointRelativeRoute + "/{fileName}")]
        [ProducesResponseType<FileContentResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetImageFile(string fileName)
        {
            var fileResult = await _imageDownloadService.CreateImageFileDownloadResultAsync(fileName);

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

        #region Methods

        /// <summary>
        /// Returns the url for fetching an image from this controller.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="fileName">The filename of the image.</param>
        /// <returns>The url of the image as a <see cref="string"/>.</returns>
        public static string GetImageUrl(HttpContext httpContext, string fileName)
        {
            return $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/{ApiControllerRoute}/{ImageFileEndpointRelativeRoute}/{fileName}";
        }

        #endregion
    }
}
