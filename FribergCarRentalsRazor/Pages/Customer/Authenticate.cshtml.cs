using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.Helpers;
using MvcRazorPages.Shared.Data;
using MvcRazorPages.Shared.ViewModels.Customer;
using AutoMapper;
using FribergFastigheter.Server.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FribergCarRentals.Data.Exceptions;

namespace FribergCarRentals.Pages.Customer
{
    /// <summary>
    /// Page model for customer registration and login.
    /// </summary>
    public class AuthenticateModel : PageModelBase
    {
        #region Constants

        /// <summary>
        /// The key used by other classes for storing redirect instructions to apply after the user have logged in.
        /// </summary>
        public const string RedirectInstructionsTempDataKey = "CustomerLoginRedirectToPage";

        #endregion

        #region Fields

        // The injected customer repository.
        private readonly ICustomerRepository _customerRepository;

        // The injected Auto Mapper.
        private readonly IMapper _mapper;

        // The injected user manager.
        private readonly UserManager<ApplicationUser> _userManager;

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="customerRepository">The injected customer repository.</param>
        /// <param name="signInManager">The injected signin manager.</param>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="userManager">The injected user manager.</param>
        /// <param name="mapper">The injected Auto Mapper.</param>
        public AuthenticateModel(ICustomerRepository customerRepository, SignInManager<ApplicationUser> signInManager,
            IAuthorizationService authorizationService, UserManager<ApplicationUser> userManager, IMapper mapper) : base(authorizationService, signInManager)
        {
            _customerRepository = customerRepository;
            _userManager = userManager;
            _mapper = mapper;
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
        public async Task<IActionResult> OnGet()
        {
            if (await IsCustomerLoggedIn())
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
            if (await IsCustomerLoggedIn())
            {
                return TempDataOrHomeRedirect();
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var customer = new CustomerEntity(_mapper.Map<ApplicationUser>(registerCustomerViewModel));

                if (await _customerRepository.CustomerExists(customer.User.Email!))
                {
                    // The key needs to be the name of the view model (insted of empty string) because the error is shown in a partial view. 
                    ModelState.AddModelError(nameof(RegisterCustomerViewModel), "An account already exists with that email.");
                }
                else
                {
                    try
                    {
                        await _customerRepository.AddAsync(customer);

                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            TempDataHelper.TryRenew<RedirectToPageData>(TempData, RedirectInstructionsTempDataKey);
                            return RedirectToPage("RegistrationConfirmation", new { userId = customer.User.Id });
                        }
                        else
                        {
                            await _signInManager.SignInAsync(customer.User, isPersistent: false);
                            return TempDataOrHomeRedirect();
                        }
                    }
                    catch (CreateUserException ex)
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
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
            if (await IsCustomerLoggedIn())
            {
                return TempDataOrHomeRedirect();
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginCustomerViewModel.Email, loginCustomerViewModel.Password, isPersistent: true, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return TempDataOrHomeRedirect();
                }
                else
                {
                    // The key needs to be the name of the view model (insted of empty string) because the error is shown in a partial view. 
                    ModelState.AddModelError(nameof(LoginCustomerViewModel), "No account matched the entered email/password.");
                }
            }

            return Page();
        }

        #endregion

        #region OtherMethods

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
