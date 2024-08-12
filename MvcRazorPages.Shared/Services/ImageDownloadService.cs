using FribergCarRentals.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO.Compression;

namespace MvcRazorPages.Shared.Services
{
    /// <summary>
    /// A service that provides links and action results to facilitate image downloads.
    /// </summary>
    public class ImageDownloadService : IImageDownloadService
    {
        #region Fields

        /// <summary>
        /// The injected configuration.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// The default image folder URL suited for regular MVC/Razor pages projects.
        /// </summary>
        private readonly string _defaultImageFolderUrl;

        /// <summary>
        /// The path to the image folder on the local disk.
        /// </summary>
        private readonly string _ImageFolderPath;

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

            _ImageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), relativeImageDownloadFolderPath);
            _defaultImageFolderUrl = $"/{Path.GetRelativePath(Directory.GetParent(_ImageFolderPath)!.FullName, _ImageFolderPath)}";
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates an <see cref="ActionResult"/> derived result object that contains an image file.
        /// </summary>
        /// <param name="imageFileName">The file name of the image.</param>
        /// <returns>A <see cref="FileContentResult"/> if the file was found or null if not.</returns>
        public async Task<FileContentResult> CreateImageFileDownloadResultAsync(string imageFileName)
        {
            var filePath = Path.Combine(_ImageFolderPath, imageFileName);

            if (!File.Exists(filePath))
            {
                throw new IOException($"The file '{imageFileName}' doesn't exists");
            }

            return new FileContentResult(await File.ReadAllBytesAsync(filePath), GetImageContentType(GetImageType(imageFileName)))
            {
                FileDownloadName = imageFileName
            };
        }

        /// <summary>
		/// Creates an <see cref="ActionResult"/> derived result object that contains a zip archive of images.
		/// </summary>
		/// <param name="imageFileNames">A collection of image file names.</param>
		/// <returns>A <see cref="FileStreamResult"/> if the files was found or null if not.</returns>
        public async Task<FileStreamResult> CreateImageZipArchiveDownloadResultAsync(List<string> imageFileNames)
        {
            if (imageFileNames.Count == 0)
            {
                throw new ArgumentException($"The collection '{imageFileNames}' can't be empty.", nameof(imageFileNames));
            }

            MemoryStream memoryStream = new();

            using (ZipArchive archive = new(memoryStream, ZipArchiveMode.Create, leaveOpen: true))
            {
                foreach (var imageFileName in imageFileNames)
                {
                    var filePath = Path.Combine(_ImageFolderPath, imageFileName);

                    if (!File.Exists(filePath))
                    {
                        throw new IOException($"The file '{imageFileName}' doesn't exists");
                    }
                    var entry = archive.CreateEntry(imageFileName);

                    using (var entryStream = entry.Open())
                    {
                        using (var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                        {
                            await fileStream.CopyToAsync(entryStream);
                        }
                    }
                }
            }

            memoryStream.Position = 0;
            return new FileStreamResult(memoryStream, "application/zip")
            {
                FileDownloadName = $"images-{DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss_fff")}-{new Random().Next(100_000, 1_000_000)}.zip"
            };
        }

        /// <summary>
        /// Returns the url for fetching an image.
        /// </summary>
        /// <param name="fileName">The filename of the image.</param>
        /// <param name="overrideImageFolderUrl">An optional override of the configured image folder url. 
        ///     Can be useful when fetching images through an API.</param>
        /// <returns>The url of the image as a <see cref="string"/>.</returns>
        public string GetImageUrl(string fileName, string? overrideImageFolderUrl = null)
        {
            string imageFolderUrl = string.IsNullOrEmpty(overrideImageFolderUrl) ?
                _defaultImageFolderUrl
                : overrideImageFolderUrl;

            return $"{imageFolderUrl}/{fileName}";
        }

        /// <summary>
		/// Gets the extension for the image type.
		/// </summary>
		/// <param name="imageType">The image type.</param>
		/// <returns>The image type as a <see cref="string"/>.</returns>
		private string GetImageContentType(SupportedImageTypes imageType)
        {
            switch (imageType)
            {
                case SupportedImageTypes.Jpeg:
                    return "application/jpg";

                case SupportedImageTypes.Png:
                    return "application/png";

                case SupportedImageTypes.Webp:
                    return "application/webp";

                default:
                    throw new NotSupportedException($"The image type is not supported: {imageType}");
            }
        }

        /// <summary>
        /// Gets the image type from a file name.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns><see cref="ImageTypes"/>.</returns>
        private SupportedImageTypes GetImageType(string fileName)
        {
            switch (Path.GetExtension(fileName.ToLower()))
            {
                case ".jpg":
                    return SupportedImageTypes.Jpeg;

                case ".png":
                    return SupportedImageTypes.Png;

                case ".webp":
                    return SupportedImageTypes.Webp;

                default:
                    throw new NotSupportedException($"The file extension is not supported: {Path.GetExtension(fileName)}");
            }
        }

        #endregion  
    }
}
