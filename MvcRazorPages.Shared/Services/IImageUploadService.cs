using Microsoft.AspNetCore.Http;

namespace FribergCarRentals.Shared.Mvc.Services
{
    /// <summary>
    /// Interface for a service that handles uploaded images on the storage disk. 
    /// </summary>
    public interface IImageUploadService
    {
        /// <summary>
        /// Deletes all image files from the disk. 
        /// </summary>
        public void ClearAllImagesFromDisk();

        /// <summary>
        /// Deletes the image file from the local disk. 
        /// </summary>
        /// <param name="imageFileName">The name of the image to delete.</param>
        public void DeleteImageFromDisk(string imageFileName);

        /// <summary>
        /// Deletes images files from the local disk. 
        /// </summary>
        /// <param name="imageFileNames">A collection of images to delete.</param>
        /// <exception cref="ArgumentException"></exception>
        public void DeleteImagesFromDisk(IEnumerable<string> imageFileNames);

        /// <summary>
        /// Saves an uploaded image to the local disk. 
        /// </summary>
        /// <param name="imageFile">The uploaded image to save to the disk.</param>
        /// <returns>The file name of the saved file.</returns>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public Task<string> SaveImageToDiskAsync(IFormFile imageFile);

        /// <summary>
        /// Saves a collection of uploaded images to the local disk. 
        /// </summary>
        /// <param name="imageFiles">A collection of uploaded images to save to the disk.</param>
        /// <returns>A collection of strings containing the file names of the images that were saved.</returns>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public Task<List<string>> SaveImagesToDisk(IEnumerable<IFormFile> imageFiles);
    }
}
