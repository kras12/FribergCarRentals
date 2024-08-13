namespace MvcRazorPages.Shared.Services
{
    /// <summary>
    /// Interface for a service that provides action results to facilitate image downloads from a standard MVC controller. 
    /// </summary>
    public interface IImageDownloadService
    {
        #region Methods

        /// <summary>
        /// Returns the url for fetching an image.
        /// </summary>
        /// <param name="fileName">The filename of the image.</param>
        /// <returns>The url of the image as a <see cref="string"/>.</returns>
        public string GetImageUrl(string fileName);

        #endregion
    }
}