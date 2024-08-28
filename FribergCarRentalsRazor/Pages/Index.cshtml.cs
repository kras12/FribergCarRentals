using FribergCarRentals.Shared.Mvc.Data;
using FribergCarRentals.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FribergCarRentals.Shared.Mvc.Services;
using FribergCarRentals.Shared.Models.ViewModels.Image;
using FribergCarRentals.Shared.Models.ViewModels.Other;

namespace FribergCarRentals.Pages
{
    /// <summary>
    /// The page model for the main page.
    /// </summary>
    public class IndexModel : PageModel
    {
        #region Fields

        /// <summary>
        /// Injected car repository.
        /// </summary>
        private readonly ICarRepository _carRepository;

        /// <summary>
        /// The injected image download service.
        /// </summary>
        private readonly IImageDownloadService _imageDownloadService;

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="carRepository">Injected car repository.</param>
        /// <param name="imageDownloadService">The injected image download service.</param>
        public IndexModel(ICarRepository carRepository, IImageDownloadService imageDownloadService)
        {
            _carRepository = carRepository;
            _imageDownloadService = imageDownloadService;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Slideshow images.
        /// </summary>
        public ListViewModel<SlideShowImageViewModel> SlideShowImagesViewModel { get; set; } = new();

        #endregion

        #region Methods

        /// <summary>
        /// Handler for Get requests.
        /// </summary>
        public async Task<IActionResult> OnGet()
        {
            List<SlideShowImageViewModel> images = new();

            var cars = (await _carRepository.GetFirstCarPerCategory()).ToList();

            foreach (var car in cars)
            {
                var image = car.Images.First();

                images.Add(new SlideShowImageViewModel(
                    _imageDownloadService.GetImageUrl(image.FileName), image.FileName, image.ImageId,
                    imageCaption: car.Category!.CategoryName,
                    linksToPage: Url.Page(
                        pageName: "Order/Book",
                        values: new RouteValueDictionary(new { CarCategoryId = car.Category!.CarCategoryId }))
                    ));
            }

            SlideShowImagesViewModel = new ListViewModel<SlideShowImageViewModel>(images);
            return Page();
        }

        #endregion
    }
}
