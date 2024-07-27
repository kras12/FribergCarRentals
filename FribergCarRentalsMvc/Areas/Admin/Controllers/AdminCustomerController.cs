using Microsoft.AspNetCore.Mvc;
using MvcRazorPages.Shared.Helpers;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.ViewModels.Other;
using MvcRazorPages.Shared.Data;
using FribergCarRentals.Helpers;
using MvcRazorPages.Shared.ViewModels.Customer;
using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using FribergCarRentals.Data.Exceptions;

namespace FribergCarRentals.Areas.Admin.Controllers
{

    [Route($"{Area}/{CurrentControllerRoutePart}/[action]")]
    [Area(Area)]
    public class AdminCustomerController : AdminControllerBase
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
        /// The key for the deleted car redirect data stored in temp storage.
        /// </summary>
        public const string RedirectToPageAfterDeleteTempDataKey = "AdminDeletedCustomerRedirectToPage";

        /// <summary>
        /// The key for the ID of the user that got a new confirm email link sent to their email. 
        /// </summary>
        public const string ResentConfirmEmailLinkForCustomerIdTempDataKey = "ResentConfirmEmailLinkForCustomerIdTempDataKey";

        /// <summary>
        /// The route part for the controller.
        /// </summary>
        private const string CurrentControllerRoutePart = "Customers";

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
        /// Constructor
        /// </summary>
        /// <param name="customerRepository">The injected customer repository.</param>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="signInManager">The injected signin manager.</param>
        /// <param name="mapper">The injected Auto Mapper.</param>
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
                return RedirectToLogin(new RedirectToActionData(nameof(Create), ControllerHelper.GetControllerName<AdminCustomerController>(), area: Area));
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
                return RedirectToLogin(new RedirectToActionData(nameof(Create), ControllerHelper.GetControllerName<AdminCustomerController>(), area: Area));
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var customer = new CustomerEntity(_mapper.Map<ApplicationUser>(registerCustomerViewModel));

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

                        return RedirectToActionInArea(nameof(Details), new RouteValueDictionary(new { id = customer.CustomerId }));
                    }
                    catch (CreateUserException ex)
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
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
                return RedirectToLogin(new RedirectToActionData(nameof(Delete), ControllerHelper.GetControllerName<AdminCustomerController>(), new RouteValueDictionary(new { id }), area: Area));
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
                    return RedirectToActionInArea(nameof(List));
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
                return RedirectToLogin(new RedirectToActionData(nameof(Details), ControllerHelper.GetControllerName<AdminCustomerController>(), new RouteValueDictionary(new { id }), area: Area));
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
                    CustomerViewModel viewModel = new(customer);

                    if (TempDataHelper.TryGet(TempData, CreatedCustomerIdTempDataKey, out int createdCustomerId))
                    {
                        viewModel.Messages.Add(UserMesssageHelper.CreateCustomerCreationSuccessMessage(createdCustomerId));
                    }

                    if (TempDataHelper.TryGet(TempData, ResentConfirmEmailLinkForCustomerIdTempDataKey, out int resentConfirmEmailLinkCustomerId))
                    {
                        viewModel.Messages.Add(UserMesssageHelper.CreateResentConfirmEmailLinkToCustomerMessage(resentConfirmEmailLinkCustomerId));
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
                return RedirectToLogin(new RedirectToActionData(nameof(Edit), ControllerHelper.GetControllerName<AdminCustomerController>(), new RouteValueDictionary(new { id }), area: Area));
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
                return RedirectToLogin(new RedirectToActionData(nameof(Edit), ControllerHelper.GetControllerName<AdminCustomerController>(), new RouteValueDictionary(new { id }), area: Area));
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
                return RedirectToLogin(new RedirectToActionData(nameof(List), ControllerHelper.GetControllerName<AdminCustomerController>(), area: Area));
            }

            ListViewModel<CustomerViewModel> viewModel = new ListViewModel<CustomerViewModel>((await _customerRepository.GetAllAsync()).Select(x => new CustomerViewModel(x)));
            TempDataHelper.Set(TempData, RedirectToPageAfterDeleteTempDataKey,
                new RedirectToActionData(nameof(List), ControllerHelper.GetControllerName<AdminCustomerController>(), area: Area));

            if (TempDataHelper.TryGet(TempData, DeletedCustomerIdTempDataKey, out int deletedCustomerId))
            {
                viewModel.Messages.Add(UserMesssageHelper.CreateCustomerDeletionSuccessMessage(deletedCustomerId));
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ResendConfirmEmailLink(int id)
        {
            // Since we haven't implemented code for sending emails to the customer, 
            // we will manually confirm the emails ourselves for now. 

            var customer = await _customerRepository.GetByIdAsync(id) ?? throw new Exception($"Failed to fetch customer with ID: {id}");
            var user = await _userManager.FindByIdAsync(customer.User.Id) ?? throw new Exception($"Failed to find user with ID: {customer.User.Id}");
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmResult = await _userManager.ConfirmEmailAsync(user, code);

            if (confirmResult.Succeeded)
            {
                TempDataHelper.Set(TempData, ResentConfirmEmailLinkForCustomerIdTempDataKey, customer.CustomerId);
                return RedirectToActionInArea(nameof(Details), new RouteValueDictionary(new { id }));
            }

            throw new Exception($"Failed to confirm email for user with ID: {id}");
        }

        #endregion
    }
}
