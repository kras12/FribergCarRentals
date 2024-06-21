using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FribergCarRentals.DataAccess.Repositories;
using MvcRazorPages.Shared.Data;
using FribergCarRentals.Sessions;
using MvcRazorPages.Shared.Helpers;
using FribergCarRentals.Models.Customer;

namespace FribergCarRentals.Pages.Admin.Customer
{
    /// <summary>
    /// Page model for showing details about customer in the admin back office. 
    /// </summary>
    public class DetailsModel : PageModel
    {
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
        public DetailsModel(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The view model used for presenting customer details. 
        /// </summary>
        public CustomerViewModel CustomerViewModel { get; set; } = default!;

        #endregion

        #region HandlerMethods       

        /// <summary>
        /// Handler for GET requests. 
        /// </summary>
        /// <param name="id">The customer ID.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnGetAsync(int id)
        {            
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(id);
            }

            if (id < 0)
            {
                throw new Exception($"Invalid ID: {id}");
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var customer = await _customerRepository.GetByIdAsync(id);

                if (customer is not null)
                {
                    CustomerViewModel = new CustomerViewModel(customer);

                    if (TempDataHelper.TryGet(TempData, CreateModel.CreatedCustomerIdTempDataKey, out int createdCustomerId))
                    {
                        CustomerViewModel.Messages.Add(UserMesssageHelper.CreateCustomerCreationSuccessMessage(createdCustomerId));
                    }

                    return Page();
                }
            }

            throw new Exception($"Failed to show the customer with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        #endregion

        #region OtherMethods

        /// <summary>
        /// Redirects to the login page and request a redirect back afterwards. 
        /// </summary>
        /// <param name="id">The ID of the customer to view.</param>
        /// <returns>A <see cref="IActionResult"/>.</returns>
        private IActionResult RedirectToLogin(int id)
        {
            TempDataHelper.Set(TempData, LoginModel.RedirectToPageTempDataKey, new RedirectToPageData(
                        "Customer/Details",
                        new RouteValueDictionary(new { id = id })));

            return RedirectToPage("../Login");
        }

        #endregion
    }
}
