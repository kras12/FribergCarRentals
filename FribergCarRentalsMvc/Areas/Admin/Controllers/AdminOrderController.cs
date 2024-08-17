using MvcRazorPages.Shared.Helpers;
using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.Data;
using FribergCarRentals.Helpers;
using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MvcRazorPages.Shared.Services;
using FribergCarRentals.Shared.Models.ViewModels.Order;
using FribergCarRentals.Shared.Models.ViewModels.Other;
using AutoMapper;
using FribergCarRentals.Shared.Models.ViewModels.Image;
using FribergCarRentals.Shared.Models.ViewModels.Message;

namespace FribergCarRentals.Areas.Admin.Controllers
{
    [Route($"{Area}/{CurrentControllerRoutePart}/[action]")]
    [Area(Area)]
    public class AdminOrderController : AdminControllerBase
    {
        #region Constants

        /// <summary>
        /// The key for the ID of the order that was completed. 
        /// </summary>
        public const string CompletedOrderIdTempDataKey = "AdminCompletedOrderId";

        /// <summary>
        /// The route part for the controller.
        /// </summary>
        private const string CurrentControllerRoutePart = "Orders";

        /// <summary>
        /// The key for the ID of the order that was deleted. 
        /// </summary>
        public const string DeletedOrderIdTempDataKey = "AdminDeletedOrderId";

        /// <summary>
        /// The key for the redirect data containing the page to redirect to after deleting an order.
        /// </summary>
        public const string RedirectToPageAfterOrderDeletionTempDataKey = "AdminDeletedOrderRedirectToPage";

        /// <summary>
        /// The key for the redirect data containing the page to redirect to after completing an order.
        /// </summary>
        public const string RedirectToPageAfterOrderCompletionTempDataKey = "AdminCompletedOrderRedirectToPage";

        #endregion

        #region Fields

        /// <summary>
        /// The injected order repository.
        /// </summary>
        private readonly ICarOrderRepository _orderRepository;

        /// <summary>
        /// The injected image download service.
        /// </summary>
        private readonly IImageDownloadService _imageDownloadService;

        /// <summary>
        /// The injected image upload service.
        /// </summary>
        private readonly IImageUploadService _imageUploadService;

		// The injected Auto Mapper.
		private readonly IMapper _mapper;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="orderRepository">The injected order repository.</param>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="signInManager">The injected signin manager.</param>
        /// <param name="imageUploadService">The injected image upload service.</param>
        /// <param name="mapper">The injected Auto Mapper.</param>
        /// <param name="imageDownloadService">The injected image download service.</param>
        public AdminOrderController(ICarOrderRepository orderRepository, IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager, IImageUploadService imageUploadService, IMapper mapper, 
            IImageDownloadService imageDownloadService) 
            : base(authorizationService, signInManager)
        {
            _orderRepository = orderRepository;
            _imageUploadService = imageUploadService;
            _mapper = mapper;
            _imageDownloadService = imageDownloadService;
        }

        #endregion

        #region Actions

        // POST: AdminOrder/Complete/5
        [ActionName(nameof(Complete))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(int id)
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(new RedirectToActionData(nameof(Complete), ControllerHelper.GetControllerName<AdminOrderController>(), area: Area));
            }

            if (id < 0)
            {
                throw new Exception($"Invalid ID: {id}");
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (await _orderRepository.TryCompleteOrderAsync(id))
                {
                    TempDataHelper.Set(TempData, CompletedOrderIdTempDataKey, id);

                    if (TempDataHelper.TryGet(TempData, RedirectToPageAfterOrderCompletionTempDataKey, out RedirectToActionData? data))
                    {
                        return RedirectToAction(data.Action, data.Controller, data.RouteValues);
                    }
                    else
                    {
                        return RedirectToActionInArea(nameof(Details), new RouteValueDictionary(new { id }));
                    }
                }
            }

            throw new Exception($"Failed to complete the order with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        // POST: AdminOrder/Delete/5
        [ActionName(nameof(Delete))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(new RedirectToActionData(nameof(Delete), ControllerHelper.GetControllerName<AdminOrderController>(), new RouteValueDictionary(new { id }), area: Area));
            }

            if (id < 0)
            {
                throw new Exception($"Invalid ID: {id}");
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                await _orderRepository.DeleteOrderAsync(id);
                TempDataHelper.Set(TempData, DeletedOrderIdTempDataKey, id);

                if (TempDataHelper.TryGet(TempData, RedirectToPageAfterOrderDeletionTempDataKey, out RedirectToActionData? data))
                {
                    return RedirectToAction(data.Action, data.Controller, data.RouteValues);
                }
                else
                {
                    return RedirectToActionInArea(nameof(List));
                }
            }

            throw new Exception($"Failed to delete the order with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        // GET: AdminOrder/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(new RedirectToActionData(nameof(Details), ControllerHelper.GetControllerName<AdminOrderController>(), new RouteValueDictionary(new { id }), area: Area));
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
                    OrderViewModel orderViewModel = _mapper.Map<OrderViewModel>(order);
                    SetImageUrls(orderViewModel.CarBooking.Car.Images);

                    // ============================================================================================================
                    // Set redirect back to action data for order manipulation
                    // ============================================================================================================
                    TempDataHelper.Set(TempData, RedirectToPageAfterOrderCompletionTempDataKey,
                        new RedirectToActionData(nameof(Details), ControllerHelper.GetControllerName<AdminOrderController>(), routeValues: new RouteValueDictionary(new { id }), area: Area));

                    // ============================================================================================================
                    // Check for manipulated orders and create user messages
                    // ============================================================================================================
                    if (TempDataHelper.TryGet(TempData, CompletedOrderIdTempDataKey, out int completedOrderId))
                    {
                        orderViewModel.Messages.Add(MessageViewModelHelper.CreateOrderCompletionSuccessMessage(completedOrderId));
                    }

                    return View(orderViewModel);
                }
            }

            throw new Exception($"Failed to show the order with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        // GET: AdminOrder
        public async Task<IActionResult> List()
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(new RedirectToActionData(nameof(List), ControllerHelper.GetControllerName<AdminOrderController>(), area: Area));
            }

            List<OrderViewModel> orderViewModels = _mapper.Map<List<OrderViewModel>>(await _orderRepository.GetAllAsync()).OrderByDescending(x => x.CarOrderId).ToList();
            SetImageUrls(orderViewModels.SelectMany(x => x.CarBooking.Car.Images).ToList());
			ListViewModel<OrderViewModel> viewModel = new(orderViewModels);

            // ============================================================================================================
            // Set redirect back to action data for order manipulation
            // ============================================================================================================
            TempDataHelper.Set(TempData, RedirectToPageAfterOrderCompletionTempDataKey,
                new RedirectToActionData(nameof(List), ControllerHelper.GetControllerName<AdminOrderController>(), area: Area));

            TempDataHelper.Set(TempData, RedirectToPageAfterOrderDeletionTempDataKey,
                new RedirectToActionData(nameof(List), ControllerHelper.GetControllerName<AdminOrderController>(), area: Area));

            // ============================================================================================================
            // Check for manipulated orders and create user messages
            // ============================================================================================================
            if (TempDataHelper.TryGet(TempData, CompletedOrderIdTempDataKey, out int completedOrderId))
            {
                viewModel.Messages.Add(MessageViewModelHelper.CreateOrderCompletionSuccessMessage(completedOrderId));
            }

            if (TempDataHelper.TryGet(TempData, DeletedOrderIdTempDataKey, out int deletedOrderId))
            {
                viewModel.Messages.Add(MessageViewModelHelper.CreateOrderDeletionSuccessMessage(deletedOrderId));
            }

            return View(viewModel);
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
