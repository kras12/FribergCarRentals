using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.Helpers;
using MvcRazorPages.Shared.Data;
using MvcRazorPages.Shared.ViewModels.Car;
using FribergFastigheter.Server.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using MvcRazorPages.Shared.Services;

namespace FribergCarRentals.Pages.Admin.Car
{
    /// <summary>
    /// A page model for creating cars in the admin back office.
    /// </summary>
    public class CreateModel : PageModelBase
    {
        #region Constants

        /// <summary>
        /// The key for the ID of the car that was created.
        /// </summary>
        public const string CreatedCarIdTempDataKey = "AdminCreatedCarId";

        #endregion

        #region Fields

        /// <summary>
        /// The injected car category repository.
        /// </summary>
        private readonly ICarCategoryRepository _carCategoryRepository;

        /// <summary>
        /// The injected car repository.
        /// </summary>
        private readonly ICarRepository _carRepository;

        /// <summary>
        /// The injected image upload service.
        /// </summary>
        private readonly IImageUploadService _imageUploadService;

        // The injected Auto Mapper.
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="carRepository">The injected car repository.</param>
        /// <param name="carCategoryRepository">The injected car category repository.</param>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="signInManager">The injected signin manager.</param>
        /// <param name="mapper">The injected Auto Mapper.</param>
        /// <param name="imageUploadService">The injected image upload service.</param>
        public CreateModel(ICarRepository carRepository, ICarCategoryRepository carCategoryRepository, IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager, IMapper mapper, IImageUploadService imageUploadService) : base(authorizationService, signInManager)
        {
            _carRepository = carRepository;
            _carCategoryRepository = carCategoryRepository;
            _mapper = mapper;
            _imageUploadService = imageUploadService;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The view model used for car creation.
        /// </summary>
        [BindProperty]
        public CreateCarViewModel CreateCarViewModel { get; set; } = new CreateCarViewModel();

        #endregion

        #region HandlerMethods

        /// <summary>
        /// Handler for GET requests.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync()
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin();
            }

            CreateCarViewModel = new CreateCarViewModel(await _carCategoryRepository.GetAllAsync());

            return Page();
        }

        /// <summary>
        /// Handler for POST requests.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin();
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var car = _mapper.Map<CarEntity>(CreateCarViewModel);
                var selectedCategory = await _carCategoryRepository.GetByIdAsync(CreateCarViewModel.SelectedCategoryId!.Value);
                car.Category = selectedCategory;

                if (CreateCarViewModel.UploadImages is not null && CreateCarViewModel.UploadImages.Count > 0)
                {
                    var savedImageFileNames = await _imageUploadService.SaveImagesToDisk(CreateCarViewModel.UploadImages);

                    foreach (var imageFileName in savedImageFileNames)
                    {
                        car.Images.Add(new ImageEntity(imageFileName, car));
                    }
                }

                await _carRepository.AddAsync(car);
                TempDataHelper.Set(TempData, CreatedCarIdTempDataKey, car.CarId);
                return RedirectToPage("Details", new { id = car.CarId });

            }

            return Page();
        }

        #endregion

        #region OtherMethods

        /// <summary>
        /// Redirects to the login page and request a redirect back afterwards. 
        /// </summary>
        /// <returns></returns>
        private IActionResult RedirectToLogin()
        {
            TempDataHelper.Set(TempData, LoginModel.RedirectToPageTempDataKey, new RedirectToPageData(
                    "Car/Create/"));

            return RedirectToPage("../Login");
        }

        #endregion
    }
}
