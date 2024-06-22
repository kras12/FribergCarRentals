using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.Data;
using MvcRazorPages.Shared.Helpers;
using MvcRazorPages.Shared.ViewModels.Customer;
using MvcRazorPages.Shared.ViewModels.Other;
using FribergFastigheter.Server.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace FribergCarRentals.Pages.Admin.Customer
{
    /// <summary>
    /// Page model for listing customers in the admin back office.
    /// </summary>
    public class ListModel : PageModelBase
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
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="signInManager">The injected signin manager.</param>
        public ListModel(ICustomerRepository customerRepository, IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager) : base(authorizationService, signInManager) 
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
            if (!await IsAdminLoggedIn())
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
