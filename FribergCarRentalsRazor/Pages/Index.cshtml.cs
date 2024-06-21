using MvcRazorPages.Shared.Data;
using FribergCarRentals.DataAccess.Repositories;
using MvcRazorPages.Shared.Helpers;
using FribergCarRentals.Models.Components;
using FribergCarRentals.Models.Other;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="carRepository">Injected car repository.</param>
        public IndexModel(ICarRepository carRepository)
        {
            _carRepository = carRepository;
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
                    ImageHelper.GetImageFileUrl(image), image.FileName, image.ImageId,
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
