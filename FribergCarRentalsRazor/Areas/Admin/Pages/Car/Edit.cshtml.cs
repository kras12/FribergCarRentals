using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.Data;
using MvcRazorPages.Shared.Helpers;
using MvcRazorPages.Shared.ViewModels.Car;
using MvcRazorPages.Shared.ViewModels.Image;
using FribergFastigheter.Server.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using MvcRazorPages.Shared.Services;

namespace FribergCarRentals.Areas.Admin.Pages.Car
{
    /// <summary>
    /// Page model class for editing a car in the admin back office. 
    /// </summary>
    public class EditModel : AdminPageModelBase
    {
        #region Constants

        /// <summary>
        /// The key to use when storing page sub titles in temporary storage.
        /// </summary>
        private const string PageSubTitleTempStorageKey = "AdminCarEditPageSubTitleTempStorageKey";

        /// <summary>
        /// The page URL relative to the login page
        /// </summary>
        public const string PageUrlRelativeToLoginPage = "Car/Edit";

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
        public EditModel(ICarRepository carRepository, ICarCategoryRepository carCategoryRepository, IAuthorizationService authorizationService,
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
        /// The view model used for editing a car. 
        /// </summary>
        [BindProperty]
        public EditCarViewModel EditCarViewModel { get; set; } = new();

        #endregion

        #region HandlerMethods        

        /// <summary>
        /// Handler for GET requests. 
        /// </summary>
        /// <param name="id">The ID of the car to edit.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing an <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(new RedirectToPageData(PageUrlRelativeToLoginPage, new RouteValueDictionary(new { id = id }), area: Area));
            }

            if (id < 0)
            {
                throw new Exception($"Invalid ID: {id}");
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var car = await _carRepository.GetByIdAsync(id);

                if (car is not null)
                {
                    var carCategories = await _carCategoryRepository.GetAllAsync();
                    EditCarViewModel = new EditCarViewModel(car, carCategories, _imageUploadService);
                    TempDataHelper.Set(TempData, PageSubTitleTempStorageKey, EditCarViewModel.PageSubTitle!);
                    return Page();
                }
            }

            throw new Exception($"Failed to show the car with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        /// <summary>
        /// Handler for POST requests. 
        /// </summary>
        /// <param name="id">The ID of the car to edit.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing an <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(new RedirectToPageData(PageUrlRelativeToLoginPage, new RouteValueDictionary(new { id = id }), area: Area));
            }

            if (id <= 0 || id != EditCarViewModel.CarId)
            {
                throw new Exception($"Invalid ID or ID mismatch - QueryParameter: {id} - ViewModel: {EditCarViewModel.CarId}");
            }

            // The images is also needed for invalid submissions, so we fetch them up here. 
            var carImages = await _carRepository.GetCarImagesAsync(id);

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var car = _mapper.Map<CarEntity>(EditCarViewModel);
                car.Category = await _carCategoryRepository.GetByIdAsync(EditCarViewModel.SelectedCategoryId);
                car.Images.AddRange(carImages);

                if (EditCarViewModel.UploadImages is not null && EditCarViewModel.UploadImages.Count > 0)
                {
                    var savedImageFileNames = await _imageUploadService.SaveImagesToDisk(EditCarViewModel.UploadImages!);
                    car.Images.AddRange(savedImageFileNames.Select(x => new ImageEntity(x, car)));
                }

                if (EditCarViewModel.DeleteImages is not null && EditCarViewModel.DeleteImages.Count > 0)
                {
                    var imagesToDelete = car.Images.IntersectBy(EditCarViewModel.DeleteImages, x => x.ImageId).ToList();

                    if (imagesToDelete.Count > 0)
                    {
                        _imageUploadService.DeleteImagesFromDisk(imagesToDelete.Select(x => x.FileName));
                        imagesToDelete.ForEach(x => car.Images.Remove(x));
                    }
                }

                await _carRepository.UpdateAsync(car);
                var carCategories = await _carCategoryRepository.GetAllAsync();
                EditCarViewModel = new EditCarViewModel(car, carCategories, _imageUploadService);
                EditCarViewModel.Messages.Add(UserMesssageHelper.CreateCarUpdateSuccessMessage(id));

                return Page();
            }

            EditCarViewModel.Images = carImages.Select(x => new ImageViewModel(_imageUploadService.GetImageUrl(x))).ToList();

            if (TempDataHelper.TryGet(TempData, PageSubTitleTempStorageKey, out string? pageSubTitle))
            {
                EditCarViewModel.PageSubTitle = pageSubTitle;
                TempDataHelper.Set(TempData, PageSubTitleTempStorageKey, EditCarViewModel.PageSubTitle!); // The user can fail again.
            }

            return Page();
        }

        #endregion
    }
}
