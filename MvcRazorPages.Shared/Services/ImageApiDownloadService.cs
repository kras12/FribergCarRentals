using FribergCarRentals.Shared.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO.Compression;

namespace MvcRazorPages.Shared.Services
{
    /// <summary>
    /// A service that provides action results to facilitate image downloads from an API.
    /// </summary>
    public class ImageApiDownloadService : IImageApiDownloadService
    {
        #region Fields

        /// <summary>
        /// The injected configuration.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// The path to the image folder on the local disk.
        /// </summary>
        private readonly string _imageFolderPath;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="configuration">The injected configuration.</param>
        public ImageApiDownloadService(IConfiguration configuration)
        {
            _configuration = configuration;

            string relativeImageDownloadFolderPath = _configuration["ImageApiDownloadService:ImageDownloadFolderPath"]!;
            _imageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), relativeImageDownloadFolderPath);
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
            var filePath = Path.Combine(_imageFolderPath, imageFileName);

            if (!File.Exists(filePath))
            {
                throw new IOException($"The file '{imageFileName}' doesn't exists");
            }

            return new FileContentResult(await File.ReadAllBytesAsync(filePath), ImageTypeHelper.GetMimeTypeFromImageFileName(imageFileName))
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
                    var filePath = Path.Combine(_imageFolderPath, imageFileName);

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

        #endregion  
    }
}
