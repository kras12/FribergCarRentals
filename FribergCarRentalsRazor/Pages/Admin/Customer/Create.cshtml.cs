using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.DataAccess.Repositories;
using FribergCarRentals.Sessions;
using MvcRazorPages.Shared.Helpers;
using FribergCarRentals.Models.Customer;
using MvcRazorPages.Shared.Data;

namespace FribergCarRentals.Pages.Admin.Customer
{
    /// <summary>
    /// Page model class for creating customers in the admin back office. 
    /// </summary>
    public class CreateModel : PageModel
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

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="customerRepository">Injected customer repository.</param>
        public CreateModel(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The view model used when creating a customer. 
        /// </summary>
        [BindProperty]
        public RegisterCustomerViewModel CreateCustomerViewModel { get; set; } = new RegisterCustomerViewModel();

        #endregion

        #region HandlerMethods

        /// <summary>
        /// Handler for GET requests. 
        /// </summary>
        /// <returns></returns>
        public IActionResult OnGet()
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
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
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin();
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (!DataTransferHelper.TryTransferData(CreateCustomerViewModel, out CustomerEntity customer))
                {
                    throw new Exception("Failed to transfer data from the view model to the entity");
                }

                if (await _customerRepository.CustomerExists(customer.Email))
                {
                    ModelState.AddModelError("", "A customer already exists with that email.");
                    return Page();
                }

                await _customerRepository.AddAsync(customer);
                TempDataHelper.Set(TempData, CreatedCustomerIdTempDataKey, customer.UserId);
                return RedirectToPage("Details", new { id = customer.UserId }); 
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
