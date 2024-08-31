using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.Shared.Mvc.Services
{
    /// <summary>
    /// A service that provides action results to facilitate image downloads from an API.
    /// </summary>
    public interface IImageApiDownloadService
    {
        #region Methods

        /// <summary>
        /// Gets an <see cref="ActionResult"/> derived object to support downloading of an image file.
        /// </summary>
        /// <param name="imageFileName">The file name of the image.</param>
        /// <returns>A <see cref="FileContentResult"/> if the file was found or null if not.</returns>
        public Task<FileContentResult> CreateImageFileDownloadResultAsync(string imageFileName);

        /// <summary>
		/// Gets an <see cref="ActionResult"/> derived object to support downloading of an image file.
		/// </summary>
		/// <param name="imageFileNames">A collection of image file names.</param>
		/// <returns>A <see cref="FileStreamResult"/> if the files was found or null if not.</returns>
        public Task<FileStreamResult> CreateImageZipArchiveDownloadResultAsync(List<string> imageFileNames);

        #endregion
    }
}