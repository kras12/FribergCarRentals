using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.Helpers;
using MvcRazorPages.Shared.Data;
using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MvcRazorPages.Shared.Services;
using FribergCarRentals.Shared.Models.ViewModels.Order;
using FribergCarRentals.Shared.Models.ViewModels.Other;
using AutoMapper;

namespace FribergCarRentals.Areas.Admin.Pages.Order
{
    /// <summary>
    /// Page model for listing customer orders in the admin back office. 
    /// </summary>
    public class ListModel : AdminPageModelBase
    {
        #region Constants

        /// <summary>
        /// The page URL relative to the login page
        /// </summary>
        public const string PageUrlRelativeToLoginPage = "Order/List";

        #endregion

        #region Fields

        /// <summary>
        /// The injected order repository.
        /// </summary>
        private readonly ICarOrderRepository _orderRepository;

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
		/// <param name="orderRepository">Injected order repository.</param>
		/// <param name="authorizationService">The injected authorization service.</param>
		/// <param name="signInManager">The injected signin manager.</param>
		/// <param name="imageUploadService">The injected image upload service.</param>
		/// <param name="mapper">The injected Auto Mapper.</param>
		public ListModel(ICarOrderRepository orderRepository, IAuthorizationService authorizationService,
			SignInManager<ApplicationUser> signInManager, IImageUploadService imageUploadService, IMapper mapper) : base(authorizationService, signInManager)
		{
			_orderRepository = orderRepository;
			_imageUploadService = imageUploadService;
			_mapper = mapper;
		}

		#endregion

		#region Properties

		/// <summary>
		/// The view model used for listing customer orders. 
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
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(new RedirectToPageData(PageUrlRelativeToLoginPage, area: Area));
            }

			List<OrderViewModel> orderViewModels = _mapper.Map<List<OrderViewModel>>(await _orderRepository.GetAllAsync()).OrderByDescending(x => x.CarOrderId).ToList();

			OrderListViewModel = new ListViewModel<OrderViewModel>();
            
            TempDataHelper.Set(TempData, CompleteModel.RedirectToPageAfterOrderCompletionTempDataKey, new RedirectToPageData(
                    "List", area: Area));

            TempDataHelper.Set(TempData, DeleteModel.RedirectToPageAfterDeleteTempDataKey, new RedirectToPageData(
                    "List", area: Area));

            if (TempDataHelper.TryGet(TempData, CompleteModel.CompletedOrderIdTempDataKey, out int completedOrderId))
            {
                OrderListViewModel.Messages.Add(UserMesssageHelper.CreateOrderCompletionSuccessMessage(completedOrderId));
            }

            if (TempDataHelper.TryGet(TempData, DeleteModel.DeletedOrderIdTempDataKey, out int deletedOrderId))
            {
                OrderListViewModel.Messages.Add(UserMesssageHelper.CreateOrderDeletionSuccessMessage(deletedOrderId));
            }
            
            return Page();
        }

        #endregion
    }
}
