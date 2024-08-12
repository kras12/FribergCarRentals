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
using AutoMapper;
using FribergCarRentals.Shared.Models.ViewModels.Image;

namespace FribergCarRentals.Pages.Order
{
    /// <summary>
    /// Page model for showing order details in the customer back office. 
    /// </summary>
    public class DetailsModel : CustomerPageModelBase
    {
        #region Fields

        /// <summary>
        /// The injected order repository.
        /// </summary>
        private readonly ICarOrderRepository _orderRepository;

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
        /// <param name="orderRepository">Injected order repository.</param>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="signInManager">The injected signin manager.</param>
        /// <param name="mapper">The injected Auto Mapper.</param>
        /// <param name="imageDownloadService">The injected image download service.</param>
        public DetailsModel(ICarOrderRepository orderRepository, IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager, IMapper mapper, IImageDownloadService imageDownloadService) : base(authorizationService, signInManager)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _imageDownloadService = imageDownloadService;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The view model to use when presenting order details. 
        /// </summary>
        public OrderViewModel OrderViewModel { get; set; } = default!;

        #endregion

        #region HandlerMethods

        /// <summary>
        /// Handler for GET requests.
        /// </summary>
        /// <param name="id">The ID for the order to view.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing an <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (!await IsCustomerLoggedIn())
            {
                return RedirectToLogin(new RedirectToPageData(
                        "../Order/Details",
                        new RouteValueDictionary(new { id = id })));
            }

            if (id < 0)
            {
                throw new Exception($"Invalid ID: {id}");
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var order = await _orderRepository.GetByIdAsync(id);

                if (order is not null)
                {
                    TempDataHelper.TryGet(TempData, BookModel.IsNewOrderTempDataKey, out bool orderWasCreated);
                    OrderViewModel = _mapper.Map<OrderViewModel>(order);
                    OrderViewModel.IsNewOrder = orderWasCreated;
                    SetImageUrls(OrderViewModel.CarBooking.Car.Images);

                    TempDataHelper.Set(TempData, CancelModel.CanceledOrderRedirectToPageTempDataKey, new RedirectToPageData(
                        "Details",
                        new RouteValueDictionary(new { id = id })));

                    if (TempDataHelper.TryGet(TempData, CancelModel.CanceledOrderIdTempDataKey, out int canceledOrderId))
                    {
                        OrderViewModel.Messages.Add(UserMesssageHelper.CreateOrderCancellationSuccessMessage(canceledOrderId));
                    }
                    
                    return Page();
                }

                throw new Exception($"Failed to retrieve the order from the database. - OrderID: {id}");
            }

            throw new Exception($"Model validation failed: - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
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
