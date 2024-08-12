using Microsoft.AspNetCore.Mvc;

namespace MvcRazorPages.Shared.Services
{
    /// <summary>
    /// Interface for an image url service that provides links and action results to facilitate image downloads.
    /// </summary>
    public interface IImageDownloadService
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

        /// <summary>
        /// Returns the url for fetching an image.
        /// </summary>
        /// <param name="fileName">The filename of the image.</param>
        /// <param name="overrideImageFolderUrl">An optional override of the configured image folder url. 
        ///     Can be useful when fetching images through an API.</param>
        /// <returns>The url of the image as a <see cref="string"/>.</returns>
        public string GetImageUrl(string fileName, string? overrideImageFolderUrl = null);

        #endregion
    }
}