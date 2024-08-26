using FribergCarRentals.Shared.Mvc.Helpers;
using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Data.Repositories;
using FribergCarRentals.Shared.Mvc.Data;
using FribergCarRentals.Shared.Mvc.Helpers;
using Microsoft.AspNetCore.Identity;
using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using AutoMapper;
using FribergCarRentals.Data.Exceptions;
using FribergCarRentals.Controllers.Customer;
using FribergCarRentals.Models.Customer;
using FribergCarRentals.Shared.Models.ViewModels.Customer;

namespace FribergCarRentals.Controllers
{
    [Route($"{CurrentControllerRoutePart}/[action]")]
    public class CustomerController : CustomerControllerBase
    {
        #region Constants

        /// <summary>
        /// The route part for this controller.
        /// </summary>
        private const string CurrentControllerRoutePart = "Customer";

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
        public CustomerController(ICustomerRepository customerRepository, SignInManager<ApplicationUser> signInManager,
            IAuthorizationService authorizationService, UserManager<ApplicationUser> userManager, 
            IMapper mapper) : base(authorizationService, signInManager)
        {
            _customerRepository = customerRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        #endregion

        #region Actions

        [HttpGet]
        public async Task<IActionResult> Authenticate()
        {
            if (await IsCustomerLoggedIn())
            {
                return TempDataOrHomeRedirect();
            }
            else if (_signInManager.IsSignedIn(User))
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction(nameof(Authenticate));
            }

            return View(new RegisterOrLoginCustomerViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId) ?? throw new Exception($"Failed to find user with ID: {userId}");
            string decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var confirmResult = await _userManager.ConfirmEmailAsync(user, decodedCode);

            if (confirmResult.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return TempDataOrHomeRedirect();
            }

            throw new Exception($"Failed to confirm email for user with ID: {userId}");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterCustomerViewModel registerCustomerViewModel)
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
                            TempDataHelper.TryRenew<RedirectToActionData>(TempData, LoginRedirectToPageTempDataKey);
                            return RedirectToAction(nameof(RegistrationConfirmation), new { userId = customer.User.Id });
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

            return View(nameof(Authenticate), new RegisterOrLoginCustomerViewModel() { RegisterCustomerViewModel = registerCustomerViewModel });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginCustomerViewModel loginCustomerViewModel)
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

            return View(nameof(Authenticate), new RegisterOrLoginCustomerViewModel() { LoginCustomerViewModel = loginCustomerViewModel });
        }

        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index), ControllerHelper.GetControllerName<HomeController>());
        }

        [HttpGet]
        public async Task<IActionResult> RegistrationConfirmation(string userId)
        {
            TempDataHelper.TryRenew<RedirectToActionData>(TempData, LoginRedirectToPageTempDataKey);

            var user = await _userManager.FindByIdAsync(userId) ?? throw new Exception($"Failed to find user with ID: {userId}");
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            string? confirmEmailLink = Url.Action(nameof(ConfirmEmail), new { userId = userId, code = code });

            return View(new ConfirmCustomerRegistrationViewModel(confirmEmailLink));
        }
        #endregion

        #region Methods        

        /// <summary>
        /// Redirects the customer to the page stored in the temp storage if such data exists, else redirects the customer to the homepage. 
        /// </summary>
        /// <returns><see cref="IActionResult"/>.</returns>
        [NonAction]
        private ActionResult TempDataOrHomeRedirect()
        {
            if (TempDataHelper.TryGet<RedirectToActionData>(TempData, LoginRedirectToPageTempDataKey, out var data))
            {
                return RedirectToAction(data.Action, data.Controller, data.RouteValues);
            }

            return RedirectToAction(nameof(Index), ControllerHelper.GetControllerName<HomeController>());
        }

        #endregion
    }
}
