using MvcRazorPages.Shared.Helpers;
using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.ViewModels.Customer;
using MvcRazorPages.Shared.Data;
using FribergCarRentals.Helpers;
using Microsoft.AspNetCore.Identity;
using FribergFastigheter.Server.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using FribergFastigheter.Shared.Constants;
using FribergCarRentals.Data.Migrations;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using AutoMapper;

namespace FribergCarRentals.Controllers
{
    [Route($"{CurrentControllerRoutePart}/[action]")]
    public class CustomerController : ViewControllerBase
    {
        #region Constants

        /// <summary>
        /// The key used by other classes for storing redirect instructions to apply after the user have logged in.
        /// </summary>
        public const string RedirectInstructionsTempDataKey = "CustomerLoginRedirectToPage";

        /// <summary>
        /// The route part for this controller.
        /// </summary>
        private const string CurrentControllerRoutePart = "Customer";
        #endregion

        #region Fields

        // The injected authorization service.
        private readonly IAuthorizationService _authorizationService;

        // The injected customer repository.
        private readonly ICustomerRepository _customerRepository;

        // The injected email store.
        private readonly IUserEmailStore<ApplicationUser> _emailStore;

        // The injected Auto Mapper.
        private readonly IMapper _mapper;

        // The injected signin manager.
        private readonly SignInManager<ApplicationUser> _signInManager;

        // The injected user manager.
        private readonly UserManager<ApplicationUser> _userManager;

        // The injected user store.
        private readonly IUserStore<ApplicationUser> _userStore;

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="customerRepository">The injected customer repository.</param>
        /// <param name="signInManager">The injected signin manager.</param>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="userManager">The injected user manager.</param>
        /// <param name="userStore">The injected user store.</param>
        /// <param name="emailStore">The injected email store.</param>
        /// <param name="mapper">The injected Auto Mapper.</param>
        public CustomerController(ICustomerRepository customerRepository, SignInManager<ApplicationUser> signInManager,
            IAuthorizationService authorizationService, UserManager<ApplicationUser> userManager, IUserStore<ApplicationUser> userStore, 
            IUserEmailStore<ApplicationUser> emailStore, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _signInManager = signInManager;
            _authorizationService = authorizationService;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = emailStore;
            _mapper = mapper;
        }

        #endregion

        #region Actions

        // GET: CustomerController
        [HttpGet]
        public async Task<IActionResult> Authenticate()
        {
            if (await IsCustomerLoggedIn())
            {
                return TempDataOrHomeRedirect();
            }

            return View(new RegisterOrLoginCustomerViewModel());
        }

        // POST: CustomerController/Create
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
                ApplicationUser user = _mapper.Map<ApplicationUser>(registerCustomerViewModel);

                if (await _customerRepository.CustomerExists(user.Email!))
                {
                    // The key needs to be the name of the view model (insted of empty string) because the error is shown in a partial view. 
                    ModelState.AddModelError(nameof(RegisterCustomerViewModel), "An account already exists with that email.");
                }
                else
                {
                    await _userStore.SetUserNameAsync(user, user.Email, CancellationToken.None);
                    await _emailStore.SetEmailAsync(user, user.Email, CancellationToken.None);

                    var createUserResult = await _userManager.CreateAsync(user, registerCustomerViewModel.Password);
                    IdentityResult? addRoleResult = null;

                    if (createUserResult.Succeeded)
                    {
                        addRoleResult = await _userManager.AddToRoleAsync(user, ApplicationUserRoles.Customer);

                        if (addRoleResult.Succeeded)
                        {
                            var userId = await _userManager.GetUserIdAsync(user);
                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));                            

                            if (_userManager.Options.SignIn.RequireConfirmedAccount)
                            {
                                // TODO - Create page to fake email confirmation
                                await _userManager.ConfirmEmailAsync(user, code);
                               // return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                            }
                            else
                            {
                                await _signInManager.SignInAsync(user, isPersistent: false);
                            }

                            string createdUserId = User.FindFirst(x => x.Type == ApplicationUserClaims.UserId)!.Value;
                            var createdUser = await _userManager.FindByIdAsync(createdUserId);

                            var customer = new CustomerEntity(createdUser!);
                            await _customerRepository.AddAsync(customer);

                            return TempDataOrHomeRedirect();
                        }
                    }

                    foreach (var error in createUserResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    if (addRoleResult != null)
                    {
                        foreach (var error in addRoleResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }

            return View(nameof(Authenticate), new RegisterOrLoginCustomerViewModel() { RegisterCustomerViewModel = registerCustomerViewModel });
        }

        // Post: CustomerController
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

        // GET: CustomerController
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index), ControllerHelper.GetControllerName<HomeController>());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks whether the current user is a logged in customer. 
        /// </summary>
        /// <returns>True if the user is a logged in customer.</returns>
        private async Task<bool> IsCustomerLoggedIn()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var authorizationResult = await _authorizationService.AuthorizeAsync(User, ApplicationUserPolicies.Customer);

                if (authorizationResult.Succeeded)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Redirects the customer to the page stored in the temp storage if such data exists, else redirects the customer to the homepage. 
        /// </summary>
        /// <returns><see cref="IActionResult"/>.</returns>
        [NonAction]
        private ActionResult TempDataOrHomeRedirect()
        {
            if (TempDataHelper.TryGet<RedirectToActionData>(TempData, RedirectInstructionsTempDataKey, out var data))
            {
                return RedirectToAction(data.Action, data.Controller, data.RouteValues);
            }

            return RedirectToAction(nameof(Index), ControllerHelper.GetControllerName<HomeController>());
        }

        #endregion
    }
}
