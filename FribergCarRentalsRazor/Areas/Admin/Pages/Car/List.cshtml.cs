using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.Data;
using MvcRazorPages.Shared.Helpers;
using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MvcRazorPages.Shared.Services;
using FribergCarRentals.Shared.Models.ViewModels.Car;
using FribergCarRentals.Shared.Models.ViewModels.Other;
using FribergCarRentals.Shared.Models.ViewModels.Image;
using AutoMapper;

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

		// The injected Auto Mapper.
		private readonly IMapper _mapper;

		#endregion

		#region Constructors

		/// <summary>
		/// A constructor.
		/// </summary>
		/// <param name="carRepository">Injected car repository.</param>
		/// <param name="authorizationService">The injected authorization service.</param>
		/// <param name="signInManager">The injected signin manager.</param>
		/// <param name="imageUploadService">The injected image upload service.</param>
		/// <param name="mapper">The injected Auto Mapper.</param>
		public ListModel(ICarRepository carRepository, IAuthorizationService authorizationService,
			SignInManager<ApplicationUser> signInManager, IImageUploadService imageUploadService, IMapper mapper) : base(authorizationService, signInManager)
		{
			_carRepository = carRepository;
			_imageUploadService = imageUploadService;
			_mapper = mapper;
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

			List<CarViewModel> carViewModels = _mapper.Map<List<CarViewModel>>(await _carRepository.GetAllAsync());
			SetImageUrls(carViewModels.SelectMany(x => x.Images).ToList());
			CarListViewModel = new ListViewModel<CarViewModel>(carViewModels);

            TempDataHelper.Set(TempData, DeleteModel.RedirectToPageAfterDeleteTempDataKey, 
                new RedirectToPageData("List", area: Area));

            if (TempDataHelper.TryGet(TempData, DeleteModel.DeletedCarIdTempDataKey, out int deletedCarId))
            {
                CarListViewModel.Messages.Add(UserMesssageHelper.CreateCarDeletionSuccessMessage(deletedCarId));
            }

            return Page();
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
