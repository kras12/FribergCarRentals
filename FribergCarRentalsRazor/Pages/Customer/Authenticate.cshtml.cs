using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.Sessions;
using FribergCarRentals.DataAccess.Repositories;
using MvcRazorPages.Shared.Helpers;
using FribergCarRentals.Models.Customer;
using MvcRazorPages.Shared.Data;

namespace FribergCarRentals.Pages.Customer
{
    /// <summary>
    /// Page model for customer registration and login.
    /// </summary>
    public class AuthenticateModel : PageModel
    {
        #region Constants

        /// <summary>
        /// The key used by other classes for storing redirect instructions to apply after the user have logged in.
        /// </summary>
        public const string RedirectInstructionsTempDataKey = "CustomerLoginRedirectToPage";

        #endregion

        #region Fields

        /// <summary>
        /// The injected customer repository.
        /// </summary>
        private readonly ICustomerRepository _customerRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor. 
        /// </summary>
        /// <param name="customerRepository">Injected customer repository.</param>
        public AuthenticateModel(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The view model used for customer registration.
        /// </summary>
        public RegisterCustomerViewModel RegisterCustomerViewModel { get; set; } = new();

        /// <summary>
        /// The view model used for customer logins.
        /// </summary>
        public LoginCustomerViewModel LoginCustomerViewModel { get; set; } = new();

        #endregion

        #region HandlerMethods

        /// <summary>
        /// Handler for GET requests.
        /// </summary>
        /// <returns><see cref="Task{TResult}"/> containing an <see cref="IActionResult"/>.</returns>
        public IActionResult OnGet()
        {
            if (UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                return TempDataOrHomeRedirect();
            }

            return Page();
        }

        /// <summary>
        /// Handler for Post requests dealing with customer registration.
        /// </summary>
        /// <param name="registerCustomerViewModel">The view model to bind data to.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing an <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnPostCreateAsync(RegisterCustomerViewModel registerCustomerViewModel)
        {
            if (UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                return TempDataOrHomeRedirect();
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (!DataTransferHelper.TryTransferData(registerCustomerViewModel, out CustomerEntity customer))
                {
                    throw new Exception("Failed to transfer data from the view model to the entity.");
                }

                if (await _customerRepository.CustomerExists(customer.Email))
                {
                    // The key needs to be the name of the view model (insted of empty string) because the error is shown in a partial view. 
                    ModelState.AddModelError(nameof(RegisterCustomerViewModel), "An account already exists with that email.");
                    return Page();
                }

                await _customerRepository.AddAsync(customer);
                LoginCustomer(customer);
                return TempDataOrHomeRedirect();
            }

            return Page();
        }

        /// <summary>
        /// Handler for Post requests dealing with customer logins.
        /// </summary>
        /// <param name="loginCustomerViewModel">The view model to bind data to.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing an <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnPostLoginAsync(LoginCustomerViewModel loginCustomerViewModel)
        {
            if (UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                return TempDataOrHomeRedirect();
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var customer = await _customerRepository.GetMatchingCustomerAsync(loginCustomerViewModel.Email, loginCustomerViewModel.Password);

                if (customer is null)
                {
                    // The key needs to be the name of the view model (insted of empty string) because the error is shown in a partial view. 
                    ModelState.AddModelError(nameof(LoginCustomerViewModel), "No account matched the entered email/password.");
                    return Page();
                }
                else
                {
                    LoginCustomer(customer);
                    return TempDataOrHomeRedirect();
                }
            }

            return Page();
        }

        #endregion

        #region OtherMethods

        /// <summary>
        /// Saves the customer user data in the session storage. 
        /// </summary>
        /// <param name="customer">The customer to login.</param>
        [NonAction]
        private void LoginCustomer(CustomerEntity customer)
        {
            UserSessionHandler.SetUserData(HttpContext.Session,
                    new UserSessionData(customer.UserId, customer.Email, customer.UserRole));
        }

        /// <summary>
        /// Redirects the user to the page stored in the temp storage if such data exists, else redirects the user to the homepage. 
        /// </summary>
        /// <returns><see cref="IActionResult"/>.</returns>
        [NonAction]
        private ActionResult TempDataOrHomeRedirect()
        {
            if (TempDataHelper.TryGet<RedirectToPageData>(TempData, RedirectInstructionsTempDataKey, out var data))
            {
                return RedirectToPage(data.Page, data.RouteValues);
            }

            return RedirectToPage("/Index");
        }

        #endregion
    }
}
