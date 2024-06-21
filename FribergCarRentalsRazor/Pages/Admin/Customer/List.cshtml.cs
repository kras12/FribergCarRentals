using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FribergCarRentals.DataAccess.Repositories;
using FribergCarRentals.Data;
using FribergCarRentals.Helpers;
using FribergCarRentals.Sessions;
using FribergCarRentalsRazor.Helpers;
using FribergCarRentals.Models.Customer;
using FribergCarRentals.Models.Other;

namespace FribergCarRentals.Pages.Admin.Customer
{
    /// <summary>
    /// Page model for listing customers in the admin back office.
    /// </summary>
    public class ListModel : PageModel
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
        public ListModel(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        #endregion

        #region Properties

        /// <summary>
        /// A view model used to present a list of customers. 
        /// </summary>
        public ListViewModel<CustomerViewModel> CustomerListViewModel { get; private set; } = new();

        #endregion

        #region HandlerMethods

        /// <summary>
        /// Handler for GET Requests. 
        /// </summary>
        /// <returns><see cref="Task{TResult}"/> containing an <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnGetAsync()
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin();
            }

            CustomerListViewModel = new ListViewModel<CustomerViewModel>((await _customerRepository.GetAllAsync()).Select(x => new CustomerViewModel(x)));
            SaveRedirectBackInstructionsForDeleteCarAction();

            if (TempDataHelper.TryGet(TempData, DeleteModel.DeletedCustomerIdTempDataKey, out int deletedCustomerId))
            {
                CustomerListViewModel.Messages.Add(UserMesssageHelper.CreateCustomerDeletionSuccessMessage(deletedCustomerId));
            }            

            return Page();
        }

        #endregion

        #region OtherMethods

        /// <summary>
        /// Redirects to the login page and request a redirect back afterwards. 
        /// </summary>
        /// <returns><see cref="IActionResult"/>.</returns>
        private IActionResult RedirectToLogin()
        {
            TempDataHelper.Set(TempData, LoginModel.RedirectToPageTempDataKey, new RedirectToPageData(
                    "Customer/List"));

            return RedirectToPage("../Login");
        }

        /// <summary>
        /// Saves data for redirecting back to this page after a car has been deleted by the delete page.
        /// </summary>
        private void SaveRedirectBackInstructionsForDeleteCarAction()
        {
            TempDataHelper.Set(TempData, DeleteModel.RedirectToPageAfterDeleteTempDataKey, new RedirectToPageData(
                    "List"));
        }

        #endregion

    }
}
