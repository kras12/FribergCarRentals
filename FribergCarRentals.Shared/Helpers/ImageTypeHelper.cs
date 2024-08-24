
namespace FribergCarRentals.Shared.Helpers
{
    // TODO - Find a better way to manage supported image file types for the projects. 
    // The types should be configured in appsettings.

    /// <summary>
    /// Helper class for managing image types.
    /// </summary>
    public static class ImageTypeHelper
    {
        #region Fields

        /// <summary>
        ///  A collection of MIMEs for supported image types.
        /// </summary>
        private static List<string> _supportedImageTypeMimes = new List<string>()
        {
            "image/jpg",
            "image/png",
            "image/webp"
        };

        #endregion

        #region Methods

        /// <summary>
        /// Gets the mime data for image files.
        /// </summary>
        /// <param name="fileName">The name of the image file including the extension.</param>
        /// <returns>A <see cref="string"/> that contains the mime type.</returns>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static string GetMimeTypeFromImageFileName(string fileName)
        {
            var fileExtenstion = Path.GetExtension(fileName).Replace(".", "").ToLower();
            string? result = _supportedImageTypeMimes.Find(x => x == $"image/{fileExtenstion}");

            if (result == null)
            {
                throw new NotSupportedException($"The file extension is not supported: {fileExtenstion}");
            }

            return result;  
        }

        /// <summary>
        /// Returns true if the file is a supported image file.
        /// </summary>
        /// <param name="fileName">The name of the file to test.</param>
        /// <returns>True if the file is a supported image file.</returns>
        public static bool IsSupportedImageFile(string fileName)
        {
            var fileExtenstion = Path.GetExtension(fileName).Replace(".", "").ToLower();
            return _supportedImageTypeMimes.Any(x => x == $"application/{fileExtenstion}");
        }

        /// <summary>
        /// Returns true if the provided MIME data represnts a supported image file.
        /// </summary>
        /// <param name="mimeData">The MIME data to compare.</param>
        /// <returns>True if the MIME data is supported.</returns>
        public static bool IsSupportedImageMime(string mimeData)
        {
            return _supportedImageTypeMimes.Any(x => x == mimeData);
        }

        #endregion
    }
}
