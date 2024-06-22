using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.Data;
using MvcRazorPages.Shared.Helpers;
using MvcRazorPages.Shared.ViewModels.Customer;
using FribergFastigheter.Server.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AutoMapper;

namespace FribergCarRentals.Pages.Admin.Customer
{
    /// <summary>
    /// Page model for editing a customer in the admin back offcie. 
    /// </summary>
    public class EditModel : PageModelBase
    {
        #region Constants

        /// <summary>
        /// The key to use when storing page sub titles in temporary storage.
        /// </summary>
        private const string PageSubTitleTempStorageKey = "AdminCustomerEditPageSubTitleTempStorageKey";

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
        /// <param name="userManager">The injected user manager.</param>
        public EditModel(ICustomerRepository customerRepository, IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager, IMapper mapper, UserManager<ApplicationUser> userManager) : base(authorizationService, signInManager)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The view model used when editing a customer. 
        /// </summary>
        [BindProperty]
        public EditCustomerViewModel EditCustomerViewModel { get; set; } = new EditCustomerViewModel();

        #endregion

        #region HandlerMethods  

        /// <summary>
        /// Handler for GET requests. 
        /// </summary>
        /// <param name="id">The ID of the customer to edit.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing an <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (!await IsAdminLoggedIn())
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
                    EditCustomerViewModel = new EditCustomerViewModel(customer);
                    TempDataHelper.Set(TempData, PageSubTitleTempStorageKey, EditCustomerViewModel.PageSubTitle!);
                    return Page();
                }
            }

            throw new Exception($"Failed to show the customer with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        /// <summary>
        /// Handler for POST requests. 
        /// </summary>
        /// <param name="id">The ID of the customer to edit.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing an <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(id);
            }

            if (id <= 0 || id != EditCustomerViewModel.AccountId)
            {
                throw new Exception($"Invalid ID or ID mismatch - QueryParameter: {id} - ViewModel: {EditCustomerViewModel.AccountId}");
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var customer = await _customerRepository.GetByIdAsync(EditCustomerViewModel.AccountId);

                if (customer == null)
                {
                    return NotFound();
                }

                // TODO - Check if this is correct mapping
                _mapper.Map(EditCustomerViewModel, customer);
                _mapper.Map(EditCustomerViewModel, customer.User);

                await _userManager.UpdateAsync(customer.User);

                if (string.IsNullOrEmpty(EditCustomerViewModel.NewPassword))
                {
                    await _userManager.RemovePasswordAsync(customer.User);
                    await _userManager.AddPasswordAsync(customer.User, EditCustomerViewModel.NewPassword!);
                }

                await _customerRepository.UpdateAsync(customer);
                EditCustomerViewModel = new EditCustomerViewModel(customer);
                EditCustomerViewModel.Messages.Add(UserMesssageHelper.CreateCustomerUpdateSuccessMessage(id));

                return Page();
            }

            if (TempDataHelper.TryGet(TempData, PageSubTitleTempStorageKey, out string? pageSubTitle))
            {
                EditCustomerViewModel.PageSubTitle = pageSubTitle;
                TempDataHelper.Set(TempData, PageSubTitleTempStorageKey, EditCustomerViewModel.PageSubTitle!);  // The user can fail again.
            }

            return Page();
        }

        #endregion

        #region OtherMethods

        /// <summary>
        /// Redirects to the login page and request a redirect back afterwards. 
        /// </summary>
        /// <param name="id">The ID of the customer to edit.</param>
        /// <returns>An <see cref="IActionResult"/>.</returns>
        private IActionResult RedirectToLogin(int id)
        {
            TempDataHelper.Set(TempData, LoginModel.RedirectToPageTempDataKey, new RedirectToPageData(
                        "Customer/Edit",
                        new RouteValueDictionary(new { id = id })));

            return RedirectToPage("../Login");
        }

        #endregion
    }
}
