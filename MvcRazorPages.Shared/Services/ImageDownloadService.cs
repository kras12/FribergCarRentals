using Microsoft.Extensions.Configuration;

namespace MvcRazorPages.Shared.Services
{
    /// <summary>
    /// A service that provides action results to facilitate image downloads from a standard MVC controller. 
    /// </summary>
    public class ImageDownloadService : IImageDownloadService
    {
        #region Fields

        /// <summary>
        /// The injected configuration.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// The image folder URL.
        /// </summary>
        private readonly string imageFolderUrl;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="configuration">The injected configuration.</param>
        public ImageDownloadService(IConfiguration configuration)
        {
            _configuration = configuration;

            string relativeImageDownloadFolderPath = _configuration["ImageDownloadService:ImageDownloadFolderPath"]!;
            string imageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), relativeImageDownloadFolderPath);
            imageFolderUrl = $"/{Path.GetRelativePath(Directory.GetParent(imageFolderPath)!.FullName, imageFolderPath)}";
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the url for fetching an image.
        /// </summary>
        /// <param name="fileName">The filename of the image.</param>
        /// <returns>The url of the image as a <see cref="string"/>.</returns>
        public string GetImageUrl(string fileName)
        {
            return $"{imageFolderUrl}/{fileName}";
        }

        #endregion  
    }
}
