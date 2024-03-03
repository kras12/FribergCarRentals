using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using FribergCarRentals.Helpers;
using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.DataAccess.Repositories;
using FribergCarRentals.Models.Customer;
using FribergCarRentals.Sessions;
using FribergCarRentalsRazor.Helpers;
using FribergCarRentals.Models.Car;
using FribergCarRentals.Models.Other;

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

        private readonly ICustomerRepository _customerRepository;

        #endregion

        #region Constructors

        public AdminCustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        #endregion

        #region Actions        

        // GET: AdminCustomerController/Create
        public IActionResult Create()
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
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
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Create));
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (!DataTransferHelper.TryTransferData(registerCustomerViewModel, out CustomerEntity customer))
                {
                    throw new Exception("Failed to transfer data from the view model to the entity");
                }

                if (await _customerRepository.CustomerExists(customer.Email))
                {
                    ModelState.AddModelError("", "A customer already exists with that email.");
                    return View(registerCustomerViewModel);
                }

                await _customerRepository.AddAsync(customer);
                TempDataHelper.Set(TempData, CreatedCustomerIdTempDataKey, customer.UserId);
                return RedirectToAction(nameof(Details), new { id = customer.UserId });
            }

            return View(registerCustomerViewModel);
        }

        // POST: AdminCustomerController/Delete/5
        [ActionName(nameof(Delete))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
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

            throw new Exception($"Failed to delete the customer with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid} - IsAdminLoggedIn: {UserSessionHandler.IsAdminLoggedIn(HttpContext.Session)}");
        }

        // GET: AdminCustomerController/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
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
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
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
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Edit), id);
            }

            if (id <= 0 || id != editCustomerViewModel.UserId)
            {
                throw new Exception($"Invalid ID or ID mismatch - QueryParameter: {id} - ViewModel: {editCustomerViewModel.UserId}");
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (DataTransferHelper.TryTransferData(editCustomerViewModel, out CustomerEntity customer))
                {
                    if (string.IsNullOrEmpty(editCustomerViewModel.Password))
                    {
                        await _customerRepository.UpdateExcludePasswordAsync(customer);
                    }
                    else
                    {
                        await _customerRepository.UpdateAsync(customer);
                    }

                    EditCustomerViewModel viewModel = new EditCustomerViewModel(customer);
                    viewModel.Messages.Add(UserMesssageHelper.CreateCustomerUpdateSuccessMessage(id));
                    return View(viewModel);
                }
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
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
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
