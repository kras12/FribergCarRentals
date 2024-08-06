using MvcRazorPages.Shared.Helpers;
using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.Data;
using FribergCarRentals.Helpers;
using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using MvcRazorPages.Shared.Services;
using FribergCarRentals.Shared.Models.ViewModels.Car;
using FribergCarRentals.Shared.Models.ViewModels.Image;
using FribergCarRentals.Shared.Models.ViewModels.Other;
using FribergCarRentals.Shared.Models.ViewModels.CarCategory;
using FribergCarRentals.Shared.Models.Mvc.ViewModels.Car;

namespace FribergCarRentals.Areas.Admin.Controllers
{
	[Route($"{Area}/{CurrentControllerRoutePart}/[action]")]
    [Area(Area)]
    public class AdminCarController : AdminControllerBase
    {
        #region Constants

        /// <summary>
        /// The key for the ID of the car that was created.
        /// </summary>
        public const string CreatedCarIdTempDataKey = "AdminCreatedCarId";

        /// <summary>
        /// The route part for the controller.
        /// </summary>
        private const string CurrentControllerRoutePart = "Cars";

        /// <summary>
        /// The key for the ID of the car that was deleted. 
        /// </summary>
        public const string DeletedCarIdTempDataKey = "AdminDeletedCarId";

        /// <summary>
        /// The key to use when storing page sub titles in temporary storage.
        /// </summary>
        private const string PageSubTitleTempDataKey = "AdminCarEditPageSubTitle";

        /// <summary>
        /// The key for the deleted car redirect data stored in temp storage.
        /// </summary>
        public const string RedirectToPageAfterDeleteTempDataKey = "AdminDeletedCarRedirectToPage";

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
        /// <param name="imageUploadService">The injected image upload service</param>
        public AdminCarController(ICarRepository carRepository, ICarCategoryRepository carCategoryRepository,
            IAuthorizationService authorizationService, SignInManager<ApplicationUser> signInManager, IMapper mapper, IImageUploadService imageUploadService)
            : base(authorizationService, signInManager)
        {
            _carRepository = carRepository;
            _carCategoryRepository = carCategoryRepository;
            _mapper = mapper;
            _imageUploadService = imageUploadService;
        }

        #endregion

        #region Actions

        // GET: AdminCarController/Create
        public async Task<IActionResult> Create()
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(new RedirectToActionData(nameof(Create), ControllerHelper.GetControllerName<AdminCarController>(), area: Area));
            }

            CreateCarViewModel viewmodel = new CreateCarViewModel(_mapper.Map<List<CarCategoryViewModel>>(await _carCategoryRepository.GetAllAsync()));

            return View(viewmodel);
        }

        // POST: AdminCarController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCarViewModel createCarViewModel)
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(new RedirectToActionData(nameof(Create), ControllerHelper.GetControllerName<AdminCarController>(), area: Area));
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var car = _mapper.Map<CarEntity>(createCarViewModel);
                var selectedCategory = await _carCategoryRepository.GetByIdAsync(createCarViewModel.SelectedCategoryId!.Value);
                car.Category = selectedCategory;

                if (createCarViewModel.UploadImages is not null && createCarViewModel.UploadImages.Count > 0)
                {
                    var savedImageFileNames = await _imageUploadService.SaveImagesToDisk(createCarViewModel.UploadImages);

                    foreach (var imageFileName in savedImageFileNames)
                    {
                        car.Images.Add(new ImageEntity(imageFileName, car));
                    }
                }

                await _carRepository.AddAsync(car);
                TempDataHelper.Set(TempData, CreatedCarIdTempDataKey, car.CarId);
                return RedirectToActionInArea(nameof(Details), new RouteValueDictionary(new { id = car.CarId }));
            }

            return View();
        }

        // POST: AdminCarController/Delete/5
        [ActionName(nameof(Delete))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(new RedirectToActionData(nameof(Delete), ControllerHelper.GetControllerName<AdminCarController>(), new RouteValueDictionary(new { id }), area: Area));
            }

            if (id < 0)
            {
                throw new Exception($"Invalid ID: {id}");
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var car = await _carRepository.GetByIdAsync(id);

                if (car!.Images.Count > 0)
                {
                    _imageUploadService.DeleteImagesFromDisk(car!.Images.Select(x => x.FileName));
                }

                await _carRepository.DeleteAsync(id);
                TempDataHelper.Set(TempData, DeletedCarIdTempDataKey, id);

                if (TempDataHelper.TryGet(TempData, RedirectToPageAfterDeleteTempDataKey, out RedirectToActionData? data))
                {
                    return RedirectToAction(data.Action, data.Controller, data.RouteValues);
                }
                else
                {
                    return RedirectToActionInArea(nameof(List));
                }
            }

            throw new Exception($"Failed to delete the car with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        // GET: AdminCarController/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(new RedirectToActionData(nameof(Details), ControllerHelper.GetControllerName<AdminCarController>(), new RouteValueDictionary(new { id }), area: Area));
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
                    CarViewModel carViewModel = _mapper.Map<CarViewModel>(car);
                    SetImageUrls(carViewModel.Images);

                    if (TempDataHelper.TryGet(TempData, CreatedCarIdTempDataKey, out int carId))
                    {
                        carViewModel.Messages.Add(UserMesssageHelper.CreateCarCreationSuccessMessage(carId));
                    }

                    return View(carViewModel);
                }
            }

            throw new Exception($"Failed to show the car with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        // GET: AdminCarController/Edit/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(new RedirectToActionData(nameof(Edit), ControllerHelper.GetControllerName<AdminCarController>(), new RouteValueDictionary(new { id }), area: Area));
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
                    EditCarViewModel editCarViewModel = _mapper.Map<EditCarViewModel>(car);
                    editCarViewModel.Categories = _mapper.Map <List<CarCategoryViewModel>>(await _carCategoryRepository.GetAllAsync());
					TempDataHelper.Set(TempData, PageSubTitleTempDataKey, editCarViewModel.PageSubTitle!);

                    return View(editCarViewModel);
                }
            }

            throw new Exception($"Failed to show the car with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        // POST: AdminCarController/Edit/5
        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditCarViewModel editCarViewModel)
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(new RedirectToActionData(nameof(Edit), ControllerHelper.GetControllerName<AdminCarController>(), new RouteValueDictionary(new { id }), area: Area));
            }

            if (id <= 0 || id != editCarViewModel.CarId)
            {
                throw new Exception($"Invalid ID or ID mismatch - QueryParameter: {id} - ViewModel: {editCarViewModel.CarId}");
            }

            // The images is also needed for invalid submissions, so we fetch them up here. 
            var carImages = await _carRepository.GetCarImagesAsync(id);

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var car = _mapper.Map<CarEntity>(editCarViewModel);
                car.Category = await _carCategoryRepository.GetByIdAsync(editCarViewModel.SelectedCategoryId);
                car.Images.AddRange(carImages);

                if (editCarViewModel.UploadImages is not null && editCarViewModel.UploadImages.Count > 0)
                {
                    var savedImageFileNames = await _imageUploadService.SaveImagesToDisk(editCarViewModel.UploadImages!);
                    car.Images.AddRange(savedImageFileNames.Select(x => new ImageEntity(x, car)));
                }

                if (editCarViewModel.DeleteImages is not null && editCarViewModel.DeleteImages.Count > 0)
                {
                    var imagesToDelete = car.Images.IntersectBy(editCarViewModel.DeleteImages, x => x.ImageId).ToList();

                    if (imagesToDelete.Count > 0)
                    {
                        _imageUploadService.DeleteImagesFromDisk(imagesToDelete.Select(x => x.FileName));
                        imagesToDelete.ForEach(x => car.Images.Remove(x));
                    }
                }

                await _carRepository.UpdateAsync(car);
                EditCarViewModel newEditCarViewModel = _mapper.Map<EditCarViewModel>(car);
                newEditCarViewModel.Categories = _mapper.Map<List<CarCategoryViewModel>>(await _carCategoryRepository.GetAllAsync());
				newEditCarViewModel.Messages.Add(UserMesssageHelper.CreateCarUpdateSuccessMessage(id));

                return View(newEditCarViewModel);
            }

            List<ImageViewModel> imageViewModels = _mapper.Map<List<ImageViewModel>>(carImages);
            SetImageUrls(imageViewModels);
            editCarViewModel.Images = imageViewModels;

			if (TempDataHelper.TryGet(TempData, PageSubTitleTempDataKey, out string? pageSubTitle))
            {
				editCarViewModel.SetPageSubTitle(pageSubTitle);
                TempDataHelper.Set(TempData, PageSubTitleTempDataKey, editCarViewModel.PageSubTitle!); // The user can fail again.
            }

            return View(editCarViewModel);
        }

        // GET: AdminCarController/List
        public async Task<IActionResult> List()
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(new RedirectToActionData(nameof(List), ControllerHelper.GetControllerName<AdminCarController>(), area: Area));
            }

            List<CarViewModel> carViewModels = _mapper.Map<List<CarViewModel>>(await _carRepository.GetAllAsync());
            SetImageUrls(carViewModels.SelectMany(x => x.Images).ToList());
			ListViewModel<CarViewModel> carListViewModel = new(carViewModels);
			
			TempDataHelper.Set(TempData, RedirectToPageAfterDeleteTempDataKey, 
                new RedirectToActionData(nameof(List), ControllerHelper.GetControllerName<AdminCarController>(), area: Area));

            if (TempDataHelper.TryGet(TempData, DeletedCarIdTempDataKey, out int deletedCarId))
            {
                carListViewModel.Messages.Add(UserMesssageHelper.CreateCarDeletionSuccessMessage(deletedCarId));
            }

            return View(carListViewModel);
        }

        #endregion

        #region OtherMethods

        /// <summary>
        /// Sets the image urls for image view models.
        /// </summary>
        /// <param name="imageViewModels">A collection of image view models to process.</param>
        private void SetImageUrls(List<ImageViewModel> imageViewModels)
        {
			imageViewModels.ForEach(x => x.Url = _imageUploadService.GetImageUrl(x.FileName));
		}

        #endregion
    }
}
