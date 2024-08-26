using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using FribergCarRentals.Data.Repositories;
using FribergCarRentals.Controllers.Customer;
using FribergCarRentals.Shared.Mvc.Helpers;
using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FribergCarRentals.Shared.Mvc.Services;
using FribergCarRentals.Shared.Models.ViewModels.Image;
using FribergCarRentals.Shared.Models.ViewModels.Other;

namespace FribergCarRentals.Controllers
{
    [Route("/[action]")]
    public class HomeController : ViewControllerBase
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

        /// <summary>
        /// The injected image upload service
        /// </summary>
        private readonly IImageUploadService _imageUploadService;

        #endregion

        #region Constructor

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="carRepository">Injected car repository.</param>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="signInManager">The injected signin manager.</param>
        /// <param name="imageUploadService">The injected image upload service</param>
        /// <param name="imageDownloadService">The injected image download service.</param>
        public HomeController(ICarRepository carRepository, IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager, IImageUploadService imageUploadService, IImageDownloadService imageDownloadService) 
            : base(authorizationService, signInManager)
        {
            _carRepository = carRepository;
            _imageUploadService = imageUploadService;
            _imageDownloadService = imageDownloadService;
        }

        #endregion

        #region Actions        

        [Route("/")]
        public async Task<IActionResult> Index()
        {
            List<SlideShowImageViewModel> images = new();

            var cars = (await _carRepository.GetFirstCarWithImagesByCategory()).ToList();

            foreach (var car in cars)
            {
                var image = car.Images.First();

                images.Add(new SlideShowImageViewModel(
                    _imageDownloadService.GetImageUrl(image.FileName), image.FileName, image.ImageId,
                    imageCaption: car.Category!.CategoryName,
                    linksToPage: Url.Action(
                        action: nameof(CustomerOrderController.Book),
                        controller: ControllerHelper.GetControllerName<CustomerOrderController>(),
                        values: new RouteValueDictionary(new { CarCategoryId = car.Category!.CarCategoryId }))
                    ));
            }

            return View(new ListViewModel<SlideShowImageViewModel>(images));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion
    }
}
