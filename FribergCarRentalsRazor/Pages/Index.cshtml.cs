using MvcRazorPages.Shared.Data;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.ViewModels.Image;
using MvcRazorPages.Shared.ViewModels.Other;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MvcRazorPages.Shared.Services;

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
        /// The injected image upload service
        /// </summary>
        private readonly IImageUploadService _imageUploadService;

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="carRepository">Injected car repository.</param>
        /// <param name="imageUploadService"> The injected image upload service</param>
        public IndexModel(ICarRepository carRepository, IImageUploadService imageUploadService)
        {
            _carRepository = carRepository;
            _imageUploadService = imageUploadService;
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

            var cars = (await _carRepository.GetFirstCarWithImagesByCategory()).ToList();

            foreach (var car in cars)
            {
                var image = car.Images.First();

                images.Add(new SlideShowImageViewModel(
                    _imageUploadService.GetImageUrl(image), image.FileName, image.ImageId,
                    imageCaption: car.Category!.CategoryName,
                    linksToPage: new RedirectToPageData(
                        pageName: "Order/Book",
                        routeValues: new RouteValueDictionary(new { CarCategoryId = car.Category!.CarCategoryId }))));
            }

            SlideShowImagesViewModel = new ListViewModel<SlideShowImageViewModel>(images);
            return Page();
        }

        #endregion
    }
}
