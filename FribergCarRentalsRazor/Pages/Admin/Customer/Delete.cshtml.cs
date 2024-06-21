using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FribergCarRentals.DataAccess.Repositories;
using FribergCarRentals.Sessions;
using MvcRazorPages.Shared.Data;
using FribergCarRentals.Helpers;

namespace FribergCarRentals.Pages.Admin.Customer
{
    /// <summary>
    /// Page model for deleting a customer in the admin back office. 
    /// </summary>
    public class DeleteModel : PageModel
    {
        #region Constants

        /// <summary>
        /// The key for the ID of the customer that was created.
        /// </summary>
        public const string CreatedCustomerIdTempDataKey = "AdminCreatedCustomerId";

        /// <summary>
        /// The key for the ID of the customer that was deleted. 
        /// </summary>
        public const string DeletedCustomerIdTempDataKey = "AdminDeletedCustomerId";

        /// <summary>
        /// The key for the redirect data containing the page to redirect to after deleting a customer.
        /// </summary>
        public const string RedirectToPageAfterDeleteTempDataKey = "AdminDeletedCustomerRedirectToPage";


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
        public DeleteModel(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        #endregion

        #region HandlerMethods        

        /// <summary>
        /// Handler for POST requests.
        /// </summary>
        /// <param name="id">The ID of the customer to delete.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnPostAsync(int id)
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
                await _customerRepository.DeleteAsync(id);
                TempDataHelper.Set(TempData, DeletedCustomerIdTempDataKey, id);

                if (TempDataHelper.TryGet(TempData, RedirectToPageAfterDeleteTempDataKey, out RedirectToPageData? data))
                {
                    return RedirectToPage(data.Page, data.RouteValues);
                }
                else
                {
                    return RedirectToPage("List");
                }
            }

            throw new Exception($"Failed to delete the customer with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid} - IsAdminLoggedIn: {UserSessionHandler.IsAdminLoggedIn(HttpContext.Session)}");
        }

        #endregion

        #region OtherMethods

        /// <summary>
        /// Redirects to the login page and request a redirect to the customer details page. 
        /// </summary>
        /// <param name="id">The ID of the customer to delete.</param>
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
