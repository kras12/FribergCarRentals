using FribergCarRentals.Data.Repositories;
using FribergFastigheter.Server.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcRazorPages.Shared.Data;
using MvcRazorPages.Shared.Helpers;

namespace FribergCarRentals.Areas.Admin.Pages.Customer
{
    /// <summary>
    /// Page model for showing sending confirm email links to customers. 
    /// </summary>
    public class ResendConfirmEmailLinkModel : AdminPageModelBase
    {
        #region Constants

        /// <summary>
        /// The key for the ID of the user that got a new confirm email link sent to their email. 
        /// </summary>
        public const string ResentConfirmEmailLinkForCustomerIdTempDataKey = "ResentConfirmEmailLinkForCustomerIdTempDataKey";

        #endregion

        #region Fields

        /// <summary>
        /// The injected customer repository.
        /// </summary>
        private readonly ICustomerRepository _customerRepository;

        // The injected user manager.
        private readonly UserManager<ApplicationUser> _userManager;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="signInManager">The injected signin manager.</param>
        /// <param name="customerRepository">The injected customer repository.</param>
        /// <param name="userManager">The injected user manager.</param>
        public ResendConfirmEmailLinkModel(IAuthorizationService authorizationService, SignInManager<ApplicationUser> signInManager, 
            ICustomerRepository customerRepository, UserManager<ApplicationUser> userManager)
            : base(authorizationService, signInManager)
        {
            _customerRepository = customerRepository;
            _userManager = userManager;
        }

        #endregion

        #region HandlerMethods

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(new RedirectToPageData("Customer/List", area: Area));
            }

            // Since we haven't implemented code for sending emails to the customer, 
            // we will manually confirm the emails ourselves for now. 

            var customer = await _customerRepository.GetByIdAsync(id) ?? throw new Exception($"Failed to fetch customer with ID: {id}");
            var user = await _userManager.FindByIdAsync(customer.User.Id) ?? throw new Exception($"Failed to find user with ID: {customer.User.Id}");
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmResult = await _userManager.ConfirmEmailAsync(user, code);

            if (confirmResult.Succeeded)
            {
                TempDataHelper.Set(TempData, ResentConfirmEmailLinkForCustomerIdTempDataKey, customer.CustomerId);
                return RedirectToPage("Details", new { id = id });
            }

            throw new Exception($"Failed to confirm email for user with ID: {id}");
        }

        #endregion
    }
}
