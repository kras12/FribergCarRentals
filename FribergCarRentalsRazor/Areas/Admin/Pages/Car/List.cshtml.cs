using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.Data;
using MvcRazorPages.Shared.Helpers;
using MvcRazorPages.Shared.ViewModels.Other;
using MvcRazorPages.Shared.ViewModels.Car;
using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MvcRazorPages.Shared.Services;

namespace FribergCarRentals.Areas.Admin.Pages.Car
{
    /// <summary>
    /// Page model class for listing cars in the admin back office.
    /// </summary>
    public class ListModel : AdminPageModelBase
    {
        #region Constants

        /// <summary>
        /// The page URL relative to the login page
        /// </summary>
        public const string PageUrlRelativeToLoginPage = "Car/List";

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
        public ListModel(ICarRepository carRepository, IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager, IImageUploadService imageUploadService) : base(authorizationService, signInManager)
        {
            _carRepository = carRepository;
            _imageUploadService = imageUploadService;
        }

        #endregion

        #region Properties

        /// <summary>
        /// A view model used to present a list of cars. 
        /// </summary>
        public ListViewModel<CarViewModel> CarListViewModel { get; private set; } = new();

        #endregion

        #region HandlerMethods       

        /// <summary>
        /// Handler for GET requests.
        /// </summary>
        /// <returns><see cref="Task{TResult}"/> containing an <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnGetAsync()
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(new RedirectToPageData(PageUrlRelativeToLoginPage, area: Area));
            }

            CarListViewModel = new ListViewModel<CarViewModel>((await _carRepository.GetAllAsync()).Select(x => new CarViewModel(x, _imageUploadService)));
            TempDataHelper.Set(TempData, DeleteModel.RedirectToPageAfterDeleteTempDataKey, 
                new RedirectToPageData("List", area: Area));

            if (TempDataHelper.TryGet(TempData, DeleteModel.DeletedCarIdTempDataKey, out int deletedCarId))
            {
                CarListViewModel.Messages.Add(UserMesssageHelper.CreateCarDeletionSuccessMessage(deletedCarId));
            }

            return Page();
        }

        #endregion
    }
}
