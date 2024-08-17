using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.Data;
using FribergCarRentals.Pages.Customer;
using MvcRazorPages.Shared.Helpers;
using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MvcRazorPages.Shared.Services;
using FribergCarRentals.Shared.Models.ViewModels.Order;
using FribergCarRentals.Shared.Models.ViewModels.Other;
using AutoMapper;
using FribergCarRentals.Shared.Models.ViewModels.Image;
using FribergCarRentals.Shared.Constants;
using FribergCarRentals.Shared.Models.ViewModels.Message;

namespace FribergCarRentals.Pages.Order
{
    /// <summary>
    /// Page model for listing orders in the customer back office.
    /// </summary>
    public class ListModel : CustomerPageModelBase
    {
        #region Fields

        /// <summary>
        /// The injected customer repository.
        /// </summary>
        private readonly ICustomerRepository _customerRepository;

        /// <summary>
        /// The injected image download service.
        /// </summary>
        private readonly IImageDownloadService _imageDownloadService;

        // The injected Auto Mapper.
        private readonly IMapper _mapper;

        #endregion

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="signInManager">The injected signin manager.</param>
        /// <param name="customerRepository">The injected customer repository.</param>
        /// <param name="mapper">The injected Auto Mapper.</param>
        /// <param name="imageDownloadService">The injected image download service.</param>
        public ListModel(IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager, ICustomerRepository customerRepository, IMapper mapper, IImageDownloadService imageDownloadService) : base(authorizationService, signInManager)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _imageDownloadService = imageDownloadService;
        }

        #region Properties

        /// <summary>
        /// View model used for listing customer orders. 
        /// </summary>
        public ListViewModel<OrderViewModel> OrderListViewModel { get; private set; } = new();

        #endregion

        #region HandlerMethods

        /// <summary>
        /// Handler for GET requests. 
        /// </summary>
        /// <returns><see cref="Task{TResult}"/> containing an <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnGetAsync()
        {
            if (!await IsCustomerLoggedIn())
            {
                return RedirectToLogin(new RedirectToPageData("../Order/List"));
            }

            var userId = User.FindFirst(x => x.Type == ApplicationUserClaims.UserId)!.Value;
            var customer = await _customerRepository.GetByUserIdAsync(userId) ?? throw new Exception($"Failed to find customer with user ID: {userId}");
            List<OrderViewModel> orders = _mapper.Map<List<OrderViewModel>>(customer.Orders).OrderByDescending(x => x.CarOrderId).ToList();
            SetImageUrls(orders.SelectMany(x => x.CarBooking.Car.Images).ToList());

			OrderListViewModel = new ListViewModel<OrderViewModel>(orders);

            if (TempDataHelper.TryGet(TempData, CancelModel.CanceledOrderIdTempDataKey, out int canceledOrderId))
            {
                OrderListViewModel.Messages.Add(MessageViewModelHelper.CreateOrderCancellationSuccessMessage(canceledOrderId));
            }

            TempDataHelper.Set(TempData, CancelModel.CanceledOrderRedirectToPageTempDataKey, new RedirectToPageData(
                    "List"));

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
