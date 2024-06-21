using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.DataAccess.Repositories;
using FribergCarRentals.Data;
using FribergCarRentals.Helpers;
using FribergCarRentals.Sessions;
using FribergCarRentalsRazor.Helpers;
using FribergCarRentals.Models.Car;
using FribergCarRentals.Models.Other;

namespace FribergCarRentals.Pages.Admin.Car
{
    /// <summary>
    /// Page model class for editing a car in the admin back office. 
    /// </summary>
    public class EditModel : PageModel
    {
        #region Constants

        /// <summary>
        /// The key to use when storing page sub titles in temporary storage.
        /// </summary>
        private const string PageSubTitleTempStorageKey = "AdminCarEditPageSubTitleTempStorageKey";

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

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="carRepository">The injected car repository.</param>
        /// <param name="carCategoryRepository">The injected car category repository.</param>
        public EditModel(ICarRepository carRepository, ICarCategoryRepository carCategoryRepository)
        {
            _carRepository = carRepository;
            _carCategoryRepository = carCategoryRepository;
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
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(id);
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
                    EditCarViewModel = new EditCarViewModel(car, carCategories);
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
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(id);
            }

            if (id <= 0 || id != EditCarViewModel.CarId)
            {
                throw new Exception($"Invalid ID or ID mismatch - QueryParameter: {id} - ViewModel: {EditCarViewModel.CarId}");
            }

            // The images is also needed for invalid submissions, so we fetch them up here. 
            var carImages = await _carRepository.GetCarImagesAsync(id);

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (DataTransferHelper.TryTransferData(EditCarViewModel, out CarEntity car))
                {
                    car.Category = await _carCategoryRepository.GetByIdAsync(EditCarViewModel.SelectedCategoryId);
                    car.Images.AddRange(carImages);

                    if (EditCarViewModel.UploadImages is not null && EditCarViewModel.UploadImages.Count > 0)
                    {
                        var savedImageFileNames = await ImageHelper.SaveUploadedImagesToDisk(EditCarViewModel.UploadImages!);
                        car.Images.AddRange(savedImageFileNames.Select(x => new ImageEntity(x, car)));
                    }

                    if (EditCarViewModel.DeleteImages is not null && EditCarViewModel.DeleteImages.Count > 0)
                    {
                        var imagesToDelete = car.Images.IntersectBy(EditCarViewModel.DeleteImages, x => x.ImageId).ToList();

                        if (imagesToDelete.Count > 0)
                        {
                            ImageHelper.DeleteImagesFromDisk(imagesToDelete.Select(x => x.FileName));
                            imagesToDelete.ForEach(x => car.Images.Remove(x));
                        }
                    }

                    await _carRepository.UpdateAsync(car);
                    var carCategories = await _carCategoryRepository.GetAllAsync();
                    EditCarViewModel = new EditCarViewModel(car, carCategories);
                    EditCarViewModel.Messages.Add(UserMesssageHelper.CreateCarUpdateSuccessMessage(id));
                    return Page();
                }

                throw new Exception("Failed to transfer data from view model to entity.");
            }

            EditCarViewModel.Images = carImages.Select(x => new ImageViewModel(x)).ToList();

            if (TempDataHelper.TryGet(TempData, PageSubTitleTempStorageKey, out string? pageSubTitle))
            {
                EditCarViewModel.PageSubTitle = pageSubTitle;
                TempDataHelper.Set(TempData, PageSubTitleTempStorageKey, EditCarViewModel.PageSubTitle!); // The user can fail again.
            }

            return Page();
        }

        #endregion

        #region OtherMethods

        /// <summary>
        /// Redirects to the login page and request a redirect back afterwards. 
        /// </summary>
        /// <param name="id">The ID of the car to edit.</param>
        /// <returns>An <see cref="IActionResult"/>.</returns>
        private IActionResult RedirectToLogin(int id)
        {
            TempDataHelper.Set(TempData, LoginModel.RedirectToPageTempDataKey, new RedirectToPageData(
                    "Car/Edit",
                    new RouteValueDictionary(new { id = id })));

            return RedirectToPage("../Login");
        }

        #endregion
    }
}
