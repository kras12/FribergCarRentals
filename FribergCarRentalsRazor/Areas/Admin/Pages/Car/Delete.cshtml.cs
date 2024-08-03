using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.Data;
using MvcRazorPages.Shared.Helpers;
using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MvcRazorPages.Shared.Services;

namespace FribergCarRentals.Areas.Admin.Pages.Car
{
    /// <summary>
    /// Page model for deleting a car in the admin back office.
    /// </summary>
    public class DeleteModel : AdminPageModelBase
    {
        #region Constants

        /// <summary>
        /// The key for the ID of the car that was deleted. 
        /// </summary>
        public const string DeletedCarIdTempDataKey = "AdminDeletedCarId";

        /// <summary>
        /// The key for the redirect data containing the page to redirect to after deleting a car.
        /// </summary>
        public const string RedirectToPageAfterDeleteTempDataKey = "AdminDeletedCarRedirectToPage";

        #endregion

        #region Fields

        /// <summary>
        /// The injected car repository.
        /// </summary>
        private readonly ICarRepository _carRepository;

        /// <summary>
        /// The injected image upload service.
        /// </summary>
        private readonly IImageUploadService _imageUploadService;

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="carRepository">Injected car repository.</param>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="signInManager">The injected signin manager.</param>
        /// <param name="imageUploadService">The injected image upload service.</param>
        public DeleteModel(ICarRepository carRepository, IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager, IImageUploadService imageUploadService) : base(authorizationService, signInManager)
        {
            _carRepository = carRepository;
            _imageUploadService = imageUploadService;
        }

        #endregion

        #region HandlerMethods        

        /// <summary>
        /// Handler for POST requests. 
        /// </summary>
        /// <param name="id">The ID of the car to delete.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(new RedirectToPageData("Car/Details", new RouteValueDictionary(new { id = id }), area: Area));
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

                if (TempDataHelper.TryGet(TempData, RedirectToPageAfterDeleteTempDataKey, out RedirectToPageData? data))
                {
                    return RedirectToPage(data.Page, data.RouteValues);
                }
                else
                {
                    return RedirectToPage("List");
                }
            }

            throw new Exception($"Failed to delete the car with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        #endregion
    }
}
