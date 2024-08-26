using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.Repositories;
using FribergCarRentals.Shared.Mvc.Data;
using FribergCarRentals.Shared.Mvc.Helpers;
using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FribergCarRentals.Shared.Mvc.Services;
using FribergCarRentals.Shared.Models.ViewModels.Car;
using FribergCarRentals.Shared.Models.ViewModels.Other;
using FribergCarRentals.Shared.Models.ViewModels.Image;
using AutoMapper;
using FribergCarRentals.Shared.Models.ViewModels.Message;

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
        /// The injected image download service.
        /// </summary>
        private readonly IImageDownloadService _imageDownloadService;

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
        /// <param name="mapper">The injected Auto Mapper.</param>
        /// <param name="imageDownloadService">The injected image download service.</param>
        public ListModel(ICarRepository carRepository, IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager, IMapper mapper, IImageDownloadService imageDownloadService) 
            : base(authorizationService, signInManager)
        {
            _carRepository = carRepository;
            _mapper = mapper;
            _imageDownloadService = imageDownloadService;
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
                CarListViewModel.Messages.Add(MessageViewModelHelper.CreateCarDeletionSuccessMessage(deletedCarId));
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
			imageViewModels.ForEach(x => x.Url = _imageDownloadService.GetImageUrl(x.FileName));
		}

		#endregion
	}
}
