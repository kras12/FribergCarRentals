using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.Repositories;
using FribergCarRentals.Shared.Mvc.Helpers;
using FribergCarRentals.Shared.Mvc.Data;
using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FribergCarRentals.Shared.Mvc.Services;
using FribergCarRentals.Shared.Models.ViewModels.Car;
using FribergCarRentals.Shared.Models.ViewModels.Image;
using AutoMapper;
using FribergCarRentals.Shared.Models.ViewModels.Message;

namespace FribergCarRentals.Areas.Admin.Pages.Car
{
    /// <summary>
    /// Page model for showing details for a car in the admin back office. 
    /// </summary>
    public class DetailsModel : AdminPageModelBase
    {
        #region Constants

        /// <summary>
        /// The page URL relative to the login page
        /// </summary>
        public const string PageUrlRelativeToLoginPage = "Car/Details";

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
        public DetailsModel(ICarRepository carRepository, IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager, IMapper mapper, IImageDownloadService imageDownloadService) : base(authorizationService, signInManager)
        {
            _carRepository = carRepository;
            _mapper = mapper;
            _imageDownloadService = imageDownloadService;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The view model used for showing car details. 
        /// </summary>
        public CarViewModel CarViewModel { get; set; } = null!;

        #endregion

        #region HandlerMethods        

        /// <summary>
        /// Handler for GET requests. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A <see cref="Task{TResult}"/> containing <see cref="IActionResult"/>.</returns>
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
                    CarViewModel = _mapper.Map<CarViewModel>(car);
                    SetImageUrls(CarViewModel.Images);

					if (TempDataHelper.TryGet(TempData, CreateModel.CreatedCarIdTempDataKey, out int carId))
                    {
                        CarViewModel.Messages.Add(MessageViewModelHelper.CreateCarCreationSuccessMessage(carId));
                    }

                    return Page();
                }
            }

            throw new Exception($"Failed to show the car with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
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
