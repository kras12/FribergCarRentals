using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.Helpers;
using MvcRazorPages.Shared.Data;
using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using FribergCarRentals.Data.Exceptions;
using FribergCarRentals.Shared.Models.ViewModels.Customer;

namespace FribergCarRentals.Areas.Admin.Pages.Customer
{
    /// <summary>
    /// Page model class for creating customers in the admin back office. 
    /// </summary>
    public class CreateModel : AdminPageModelBase
    {
        #region Constants

        /// <summary>
        /// The key for the ID of the customer that was created.
        /// </summary>
        public const string CreatedCustomerIdTempDataKey = "AdminCreatedCustomerId";

        /// <summary>
        /// The page URL relative to the login page
        /// </summary>
        public const string PageUrlRelativeToLoginPage = "Customer/Create";

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
                return RedirectToLogin(new RedirectToPageData(PageUrlRelativeToLoginPage, area: Area));
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
                return RedirectToLogin(new RedirectToPageData(PageUrlRelativeToLoginPage, area: Area));
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
    }
}
