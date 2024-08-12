using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MvcRazorPages.Shared.Services
{
    /// <summary>
    /// A service that handles uploaded images on the storage disk. 
    /// </summary>
    public class ImageUploadService : IImageUploadService
    {
        #region Constants

        /// <summary>
        /// The max number of attempts to try and save a file to disk.
        /// </summary>
        private const int MaxDiskSaveAttemptsPerFile = 1_000;

        /// <summary>
        /// The largest number number suffix for image files.
        /// </summary>
        private const int MaxFileNumberSuffix = 10_000;

        /// <summary>
        /// The smallest number number suffix for image files.
        /// </summary>
        private const int MinFileNumberSuffix = 1_000;

        #endregion

        #region Fields

        /// <summary>
        /// The injected configuration.
        /// </summary>
        private readonly IConfiguration _configuration;

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
        public ImageUploadService(IConfiguration configuration)
        {
            _configuration = configuration;
            _ImageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), 
                _configuration["ImageUploadService:ImageUploadsFolderPath"]!);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes all image files from the disk. 
        /// </summary>
        public void ClearAllImagesFromDisk()
        {
            foreach (var image in Directory.EnumerateFiles(_ImageFolderPath))
            {
                File.Delete(image);
            }
        }

        /// <summary>
        /// Deletes the image file from the local disk. 
        /// </summary>
        /// <param name="imageFileName">The name of the image to delete.</param>
        public void DeleteImageFromDisk(string imageFileName)
        {
            DeleteImagesFromDisk(new List<string>() { imageFileName });
        }

        /// <summary>
        /// Deletes images files from the local disk. 
        /// </summary>
        /// <param name="imageFileNames">A collection of images to delete.</param>
        /// <exception cref="ArgumentException"></exception>
        public void DeleteImagesFromDisk(IEnumerable<string> imageFileNames)
        {
            #region Checks

            if (!imageFileNames.Any())
            {
                throw new ArgumentException($"The collection '{nameof(imageFileNames)}' can't be empty.");
            }

            #endregion

            foreach (var imageFileName in imageFileNames)
            {
                File.Delete(Path.Combine(_ImageFolderPath, imageFileName));
            }
        }

        /// <summary>
        /// Saves an uploaded image to the local disk. 
        /// </summary>
        /// <param name="imageFile">The uploaded image to save to the disk.</param>
        /// <returns>The file name of the saved file.</returns>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async Task<string> SaveImageToDiskAsync(IFormFile imageFile)
        {
            return (await SaveImagesToDisk(new List<IFormFile>() { imageFile })).Single();
        }

        /// <summary>
        /// Saves a collection of uploaded images to the local disk. 
        /// </summary>
        /// <param name="imageFiles">A collection of uploaded images to save to the disk.</param>
        /// <returns>A collection of strings containing the file names of the images that were saved.</returns>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async Task<List<string>> SaveImagesToDisk(IEnumerable<IFormFile> imageFiles)
        {
            #region Checks

            if (!imageFiles.Any())
            {
                throw new ArgumentException($"The collection '{nameof(imageFiles)}' can't be empty.");
            }

            #endregion

            List<string> result = new();

            try
            {
                var random = new Random();

                if (!Directory.Exists(_ImageFolderPath))
                {
                    Directory.CreateDirectory(_ImageFolderPath);
                }

                foreach (var imageFile in imageFiles)
                {
                    FileInfo fileInfo = new FileInfo(imageFile.FileName);
                    string fileName = "";
                    string filePath = "";

                    for (int i = 0; i < MaxDiskSaveAttemptsPerFile; i++)
                    {
                        fileName = $"{Path.GetFileNameWithoutExtension(fileInfo.Name)}-{random.Next(MinFileNumberSuffix, MaxFileNumberSuffix)}{fileInfo.Extension}";
                        filePath = Path.Combine(_ImageFolderPath, fileName);

                        if (!File.Exists(filePath))
                        {
                            break;
                        }
                    }

                    using (var stream = new FileStream(filePath, FileMode.CreateNew))
                    {
                        await imageFile.CopyToAsync(stream);
                        result.Add(fileName);
                    }
                }

                return result;
            }
            catch (IOException ex)
            {
                try
                {
                    DeleteImagesFromDisk(result);
                }
                catch (IOException)
                {
                    throw new IOException("Failed to cleanup all images from the disk after the image save process failed.", ex);
                }

                throw;
            }
        }

        #endregion
    }
}
