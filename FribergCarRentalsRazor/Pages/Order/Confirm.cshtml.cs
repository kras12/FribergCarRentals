using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.DataAccess.Repositories;
using MvcRazorPages.Shared.Data;
using FribergCarRentals.DataAccess.Types;
using MvcRazorPages.Shared.Helpers;
using FribergCarRentals.Sessions;
using FribergCarRentals.Pages.Customer;
using FribergCarRentals.Models.Orders;

namespace FribergCarRentals.Pages.Order
{
    /// <summary>
    /// Page model for booking a car and creating an order. 
    /// </summary>
    public class ConfirmModel : PageModel
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
        public ConfirmModel(ICarRepository carRepository, ICustomerRepository customerRepository, ICarOrderRepository orderRepository)
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
        public IActionResult OnGet()
        {
            if (!UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin("../Order/Confirm");
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
            if (!UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin("../Order/Confirm");
            }

            if (TempDataHelper.TryGet(TempData, BookModel.PendingOrderTempDataKey, out CreateOrderViewModel? createOrderViewModel))
            {
                CreateOrderViewModel = createOrderViewModel;
                var customer = await _customerRepository.GetByIdAsync(UserSessionHandler.GetUserData(HttpContext.Session).UserId);
                var car = await _carRepository.GetByIdAsync(CreateOrderViewModel.CarId);

                if (customer is not null && car is not null)
                {
                    car.RentalStatus = CarRentalStatusEntity.CreateFromType(RentalCarStatus.PendingPickup);
                    var order = new CarOrderEntity(customer);
                    order.CarBookings.Add(
                        new CarBookingEntity(order, car, pickupDateUTC: CreateOrderViewModel.PickupDateLocalTime.Date,
                            returnDateUTC: CreateOrderViewModel.ReturnDateLocalTime.Date));

                    await _orderRepository.AddAsync(order);
                    TempDataHelper.Set(TempData, IsNewOrderTempDataKey, true);
                    return RedirectToPage("Details", new { id = order.CarOrderId });
                }

                throw new Exception($"Failed to retrieve car and/or customer from the database. - CarID: {CreateOrderViewModel.CarId} - CustomerID: {UserSessionHandler.GetUserData(HttpContext.Session).UserId}");
            }

            throw new Exception($"Failed to retrieve the pending order from temp storage.");
        }

        #endregion

        #region OtherMethods

        /// <summary>
        /// Redirects to the login page and request a redirect afterwards. 
        /// </summary>
        /// <param name="page">The page to redirect to.</param>
        /// <returns><see cref="IActionResult"/>.</returns>
        private IActionResult RedirectToLogin(string page)
        {
            TempDataHelper.Set(TempData, AuthenticateModel.RedirectInstructionsTempDataKey, new RedirectToPageData(page));
            return RedirectToPage("../Customer/Authenticate");
        }

        #endregion
    }
}
