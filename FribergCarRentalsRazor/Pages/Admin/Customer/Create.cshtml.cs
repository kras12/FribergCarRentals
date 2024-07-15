using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.Helpers;
using MvcRazorPages.Shared.ViewModels.Customer;
using MvcRazorPages.Shared.Data;
using FribergFastigheter.Server.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using FribergCarRentals.Data.Exceptions;

namespace FribergCarRentals.Pages.Admin.Customer
{
    /// <summary>
    /// Page model class for creating customers in the admin back office. 
    /// </summary>
    public class CreateModel : PageModelBase
    {
        #region Constants

        /// <summary>
        /// The key for the ID of the customer that was created.
        /// </summary>
        public const string CreatedCustomerIdTempDataKey = "AdminCreatedCustomerId";

        #endregion

        #region Fields

        /// <summary>
        /// The injected customer repository.
        /// </summary>
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
        /// <param name="customerRepository">Injected customer repository.</param>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="signInManager">The injected signin manager.</param>
        /// <param name="mapper">The injected Auto Mapper.</param>
        /// <param name="userManager"> The injected user manager.</param>
        public CreateModel(ICustomerRepository customerRepository, IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager, IMapper mapper, UserManager<ApplicationUser> userManager) : base(authorizationService, signInManager)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The view model used when creating a customer. 
        /// </summary>
        [BindProperty]
        public RegisterCustomerViewModel RegisterCustomerViewModel { get; set; } = new RegisterCustomerViewModel();

        #endregion

        #region HandlerMethods

        /// <summary>
        /// Handler for GET requests. 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGet()
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin();
            }

            return Page();
        }

        /// <summary>
        /// Handler for POST requests. 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin();
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var customer = new CustomerEntity(_mapper.Map<ApplicationUser>(RegisterCustomerViewModel));

                if (await _customerRepository.CustomerExists(customer.User.Email!))
                {
                    ModelState.AddModelError("", "An account already exists with that email.");
                }
                else
                {
                    try
                    {
                        await _customerRepository.AddAsync(customer);
                        TempDataHelper.Set(TempData, CreatedCustomerIdTempDataKey, customer.CustomerId);

                        return RedirectToPage("Details", new { id = customer.CustomerId });
                    }
                    catch (CreateUserException ex)
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                } 
            }

            return Page();
        }

        #endregion

        #region OtherMethods

        /// <summary>
        /// Redirects to the login page and request a redirect back afterwards. 
        /// </summary>
        /// <returns></returns>
        private IActionResult RedirectToLogin()
        {
            TempDataHelper.Set(TempData, LoginModel.RedirectToPageTempDataKey, new RedirectToPageData(
                    "Customer/Create"));

            return RedirectToPage("../Login");
        }

        #endregion
    }
}
