using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.Data;
using MvcRazorPages.Shared.Helpers;
using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FribergCarRentals.Shared.Models.ViewModels.Order;
using FribergCarRentals.Shared.Types.Enums;
using FribergCarRentals.Shared.Constants;

namespace FribergCarRentals.Pages.Order
{
    /// <summary>
    /// Page model for booking a car and creating an order. 
    /// </summary>
    public class ConfirmModel : CustomerPageModelBase
    {
        #region Constants

        /// <summary>
        /// The key for the created order flag stored in temp storage.
        /// </summary>
        public const string IsNewOrderTempDataKey = "CustomerBookModelIsNewOrder";

        #endregion

        #region Fields

        /// <summary>
        /// Injected car repository. 
        /// </summary>
        private readonly ICarRepository _carRepository;

        /// <summary>
        /// Injected customer repository. 
        /// </summary>
        private readonly ICustomerRepository _customerRepository;


        /// <summary>
        /// Injected order repository. 
        /// </summary>
        private readonly ICarOrderRepository _orderRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="carRepository">Injected car repository. </param>
        /// <param name="customerRepository">Injected customer repository. </param>
        /// <param name="orderRepository">Injected order repository. </param>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="signInManager">The injected signin manager.</param>
        public ConfirmModel(ICarRepository carRepository, ICustomerRepository customerRepository, ICarOrderRepository orderRepository, 
            IAuthorizationService authorizationService, SignInManager<ApplicationUser> signInManager) : base(authorizationService, signInManager)
        {
            _carRepository = carRepository;
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The view model used to bind order creation data. 
        /// </summary>
        [BindProperty]
        public CreateOrderViewModel CreateOrderViewModel { get; set; } = new();

        #endregion

        #region HandlerMethods

        /// <summary>
        /// Handler for POST requests. 
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> containing an <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnGet()
        {
            if (!await IsCustomerLoggedIn())
            {
                return RedirectToLogin(new RedirectToPageData("../Order/Confirm"));
            }

            if (TempDataHelper.TryGet(TempData, BookModel.PendingOrderTempDataKey, out CreateOrderViewModel? createOrderViewModel))
            {
                // Customer may refresh the page, and we need it in order to complete the ordesr.
                TempDataHelper.Set(TempData, BookModel.PendingOrderTempDataKey, createOrderViewModel);
                CreateOrderViewModel = createOrderViewModel;
                return Page();
            }

            throw new Exception($"Failed to retrieve the pending order from temp storage.");
        }

        /// <summary>
        /// Handler for creating orders.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> containing an <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!await IsCustomerLoggedIn())
            {
                return RedirectToLogin(new RedirectToPageData("../Order/Confirm"));
            }

            if (TempDataHelper.TryGet(TempData, BookModel.PendingOrderTempDataKey, out CreateOrderViewModel? createOrderViewModel))
            {
                CreateOrderViewModel = createOrderViewModel;
                var userId = User.FindFirst(x => x.Type == ApplicationUserClaims.UserId)!.Value;
                var customer = await _customerRepository.GetByUserIdAsync(userId);
                var car = await _carRepository.GetByIdAsync(CreateOrderViewModel.CarId);

                if (customer == null)
                {
                    throw new Exception("Customer was not found.");
                }

                if (car == null)
                {
                    throw new Exception("Car was not found.");
                }

                car.RentalStatus = CarRentalStatusEntity.CreateFromType(RentalCarStatus.PendingPickup);
                var order = new CarOrderEntity(customer);
                order.CarBookings.Add(
                    new CarBookingEntity(order, car, pickupDateUTC: CreateOrderViewModel.PickupDateLocalTime.Date,
                        returnDateUTC: CreateOrderViewModel.ReturnDateLocalTime.Date));

                await _orderRepository.AddAsync(order);
                TempDataHelper.Set(TempData, IsNewOrderTempDataKey, true);
                return RedirectToPage("Details", new { id = order.CarOrderId });
            }

            throw new Exception($"Failed to retrieve the pending order from temp storage.");
        }

        #endregion
    }
}
