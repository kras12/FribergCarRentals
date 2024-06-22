using MvcRazorPages.Shared.Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.ViewModels.Other;
using MvcRazorPages.Shared.Helpers;
using FribergCarRentals.Controllers.Customer;
using FribergCarRentals.Helpers;
using MvcRazorPages.Shared.ViewModels.Image;
using FribergFastigheter.Server.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

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

        #endregion

        #region Constructor

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="carRepository">Injected car repository.</param>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="signInManager">The injected signin manager.</param>
        public HomeController(ICarRepository carRepository, IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager) : base(authorizationService, signInManager)
        {
            _carRepository = carRepository;
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
                    ImageHelper.GetImageFileUrl(image), image.FileName, image.ImageId,
                    imageCaption: car.Category!.CategoryName,
                    linksToPage: new RedirectToActionData(
                        action: nameof(CustomerOrderController.Book),
                        controller: ControllerHelper.GetControllerName<CustomerOrderController>(),
                        routeValues: new RouteValueDictionary(new { CarCategoryId = car.Category!.CarCategoryId }))));
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
