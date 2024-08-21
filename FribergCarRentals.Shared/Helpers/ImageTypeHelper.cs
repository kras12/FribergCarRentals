
namespace FribergCarRentals.Shared.Helpers
{
    // TODO - Find a better way to manage supported image file types for the projects. 

    /// <summary>
    /// Helper class for managing image types.
    /// </summary>
    public static class ImageTypeHelper
    {
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
            var fileExtenstion = Path.GetExtension(fileName).ToLower();

            switch (fileExtenstion)
            {
                case ".jpg":
                    return "application/jpg";

                case ".png":
                    return "application/png";

                case ".webp":
                    return "application/webp";

                default:
                    throw new NotSupportedException($"The file extension is not supported: {fileExtenstion}");
            }
        }

        /// <summary>
        /// Returns true if the file is a supported image file.
        /// </summary>
        /// <param name="fileName">The name of the file to test.</param>
        /// <returns>True if the file is a supported image file.</returns>
        public static bool IsSupportedImageFile(string fileName)
        {
            try
            {
                GetMimeTypeFromImageFileName(fileName);
                return true;
            }
            catch (NotSupportedException)
            {
                return false;
            }
        }

        #endregion
    }
}
