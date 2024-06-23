using Microsoft.AspNetCore.Mvc;
using MvcRazorPages.Shared.Helpers;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.ViewModels.Other;
using MvcRazorPages.Shared.Data;
using FribergCarRentals.Helpers;
using MvcRazorPages.Shared.ViewModels.Customer;
using FribergFastigheter.Server.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using FribergFastigheter.Shared.Constants;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace FribergCarRentals.Controllers.Admin
{

    [Route($"{CurrentControllerRoutePart}/[action]")]
    public class AdminCustomerController : ViewControllerBase
    {
        #region Constants

        /// <summary>
        /// The key for the ID of the customer that was created.
        /// </summary>
        public const string CreatedCustomerIdTempDataKey = "AdminCreatedCustomerId";

        /// <summary>
        /// The route part for the controller.
        /// </summary>
        private const string CurrentControllerRoutePart = "Admin/Customers";

        /// <summary>
        /// The key for the ID of the customer that was deleted. 
        /// </summary>
        public const string DeletedCustomerIdTempDataKey = "AdminDeletedCustomerId";

        /// <summary>
        /// The key to use when storing page sub titles in temporary storage.
        /// </summary>
        private const string PageSubTitleTempStorageKey = "AdminCustomerEditPageSubTitleTempStorageKey";

        /// <summary>
        /// The key for the deleted car redirect data stored in temp storage.
        /// </summary>
        public const string RedirectToPageAfterDeleteTempDataKey = "AdminDeletedCustomerRedirectToPage";

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
        /// Constructor
        /// </summary>
        /// <param name="customerRepository">The injected customer repository.</param>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="signInManager">The injected signin manager.</param>
        /// <param name="mapper">The injected Auto Mapper.</param>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="userManager">The injected user manager.</param>
        /// <param name="userStore">The injected user store.</param>
        /// <param name="emailStore">The injected email store.</param>
        public AdminCustomerController(ICustomerRepository customerRepository, IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager, IMapper mapper, UserManager<ApplicationUser> userManager) : base(authorizationService, signInManager)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        #endregion

        #region Actions        

        // GET: AdminCustomerController/Create
        public async Task<IActionResult> Create()
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(nameof(Create));
            }

            return View();            
        }

        // POST: AdminCustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterCustomerViewModel registerCustomerViewModel)
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(nameof(Create));
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                ApplicationUser user = _mapper.Map<ApplicationUser>(registerCustomerViewModel);

                if (await _customerRepository.CustomerExists(user.Email!))
                {
                    ModelState.AddModelError("", "A customer already exists with that email.");
                    return View(registerCustomerViewModel);
                }
                else
                {
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

                            var customer = new CustomerEntity(user!);
                            await _customerRepository.AddAsync(customer);
                            TempDataHelper.Set(TempData, CreatedCustomerIdTempDataKey, customer.CustomerId);

                            return RedirectToAction(nameof(Details), new { id = customer.CustomerId });
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

            return View(registerCustomerViewModel);
        }

        // POST: AdminCustomerController/Delete/5
        [ActionName(nameof(Delete))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(nameof(Delete), id);
            }

            if (id < 0)
            {
                throw new Exception($"Invalid ID: {id}");
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                await _customerRepository.DeleteAsync(id);
                TempDataHelper.Set(TempData, DeletedCustomerIdTempDataKey, id);

                if (TempDataHelper.TryGet(TempData, RedirectToPageAfterDeleteTempDataKey, out RedirectToActionData? data))
                {
                    return RedirectToAction(data.Action, data.Controller, data.RouteValues);
                }
                else
                {
                    return RedirectToAction(nameof(List));
                }
            }

            throw new Exception($"Failed to delete the customer with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        // GET: AdminCustomerController/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(nameof(Details), id);
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
                    CustomerViewModel viewModel = new (customer);

                    if (TempDataHelper.TryGet(TempData, CreatedCustomerIdTempDataKey, out int createdCustomerId))
                    {
                        viewModel.Messages.Add(UserMesssageHelper.CreateCustomerCreationSuccessMessage(createdCustomerId));
                    }

                    return View(viewModel);
                }
            }

            throw new Exception($"Failed to show the customer with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        // GET: AdminCustomerController/Edit/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(nameof(Edit), id);
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
                    EditCustomerViewModel viewModel = new EditCustomerViewModel(customer);
                    TempDataHelper.Set(TempData, PageSubTitleTempStorageKey, viewModel.PageSubTitle!);
                    return View(viewModel);
                }
            }

            throw new Exception($"Failed to show the customer with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        // POST: AdminCustomerController/Edit/5
        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditCustomerViewModel editCustomerViewModel)
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(nameof(Edit), id);
            }

            if (id <= 0 || id != editCustomerViewModel.AccountId)
            {
                throw new Exception($"Invalid ID or ID mismatch - QueryParameter: {id} - ViewModel: {editCustomerViewModel.AccountId}");
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var userId = await _customerRepository.GetUserId(editCustomerViewModel.AccountId) ?? throw new Exception($"Failed to find customer with ID: {editCustomerViewModel.AccountId}");

                // Must fetch the user this way instead of fetching the customer entity and use the included user entity there.
                // This is to avoid tracking conflicts with EF Core that occurs despite the fact that the customer retrieval is done as no tracking. 
                var user = await _userManager.FindByIdAsync(userId) ?? throw new Exception($"Failed to find user with ID: {userId}"); 

                _mapper.Map(editCustomerViewModel, user);
                await _userManager.UpdateAsync(user);

                if (!string.IsNullOrEmpty(editCustomerViewModel.NewPassword))
                {
                    await _userManager.RemovePasswordAsync(user);
                    await _userManager.AddPasswordAsync(user, editCustomerViewModel.NewPassword!);
                }

                var customer = await _customerRepository.GetByUserIdAsync(user.Id) ?? throw new Exception($"Failed to find customer with ID: {editCustomerViewModel.AccountId}");
                EditCustomerViewModel viewModel = new EditCustomerViewModel(customer);
                viewModel.Messages.Add(UserMesssageHelper.CreateCustomerUpdateSuccessMessage(id));

                return View(viewModel);
            }

            if (TempDataHelper.TryGet(TempData, PageSubTitleTempStorageKey, out string? pageSubTitle))
            {
                editCustomerViewModel.PageSubTitle = pageSubTitle;
                TempDataHelper.Set(TempData, PageSubTitleTempStorageKey, editCustomerViewModel.PageSubTitle!);  // The user can fail again.
            }

            return View(editCustomerViewModel);
        }

        // GET: AdminCustomerController
        [HttpGet]
        public async Task<IActionResult> List()
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(nameof(List));
            }

            ListViewModel<CustomerViewModel> viewModel = new ListViewModel<CustomerViewModel>((await _customerRepository.GetAllAsync()).Select(x => new CustomerViewModel(x)));
            SaveRedirectBackInstructionsForDeleteCustomerAction(nameof(List));

            if (TempDataHelper.TryGet(TempData, DeletedCustomerIdTempDataKey, out int deletedCustomerId))
            {
                viewModel.Messages.Add(UserMesssageHelper.CreateCustomerDeletionSuccessMessage(deletedCustomerId));
            }

            return View(viewModel);
        }

        #endregion

        #region OtherMethods

        /// <summary>
        /// Redirects to the login page and request a redirect back afterwards. 
        /// </summary>
        /// <param name="action">The action to redirect to.</param>
        /// <param name="id">An optional ID for the customer.</param>
        /// <returns><see cref="IActionResult"/>.</returns>
        private IActionResult RedirectToLogin(string action, int? id = null)
        {
            RouteValueDictionary? routeValues = id is not null ? new RouteValueDictionary(new { id = id }) : null;

            TempDataHelper.Set(TempData, AdminController.RedirectToPageTempDataKey, new RedirectToActionData(
                    action, ControllerHelper.GetControllerName<AdminCustomerController>(), routeValues: routeValues));

            return RedirectToAction(nameof(AdminController.Login), ControllerHelper.GetControllerName<AdminController>());
        }

        /// <summary>
        /// Saves data for redirecting back to an action after a customer has been deleted. 
        /// </summary>
        /// <param name="redirectToAction">The action to redirect to.</param>
        private void SaveRedirectBackInstructionsForDeleteCustomerAction(string redirectToAction)
        {
            TempDataHelper.Set(TempData, RedirectToPageAfterDeleteTempDataKey, new RedirectToActionData(
                    redirectToAction, ControllerHelper.GetControllerName<AdminCustomerController>()));
        }

        #endregion
    }
}
