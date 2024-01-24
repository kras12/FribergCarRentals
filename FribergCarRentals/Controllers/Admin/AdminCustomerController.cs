using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using FribergCarRentals.Data.Customer;
using FribergCarRentals.Helpers;
using FribergCarRentals.Session;
using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.DataAccess.Repositories;

namespace FribergCarRentals.Controllers.Admin
{

    [Route($"{CurrentControllerRoutePart}/[action]")]
    public class AdminCustomerController : ViewControllerBase
    {
        #region Constants

        private const string CurrentControllerRoutePart = "Admin/Customers";

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
        public ActionResult Create()
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                TempDataHelper.Set(TempData, AdminController.RedirectToActionTempDataKey, new LoginRedirectActionWithId(
                    nameof(Create),
                    ControllerHelper.GetControllerName<AdminCustomerController>()));

                return RedirectToAction(nameof(AdminController.Login), ControllerHelper.GetControllerName<AdminController>());
            }

            return View();
        }

        // POST: AdminCustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CustomerRegisterViewModel customerViewModel)
        {
            if (ModelState.Count > 0 && ModelState.IsValid && UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                if (DataTransferHelper.TryTransferData(customerViewModel, out CustomerEntity customer))
                {
                    await _customerRepository.Add(customer);
                    return RedirectToAction(nameof(List));
                }
            }

            return StatusCode(500);
        }

        // POST: AdminCustomerController/Delete/5
        [ActionName(nameof(Delete))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            if (ModelState.Count > 0 && ModelState.IsValid && id > 0 && UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                if (await _customerRepository.Delete(id))
                {
                    return RedirectToAction(nameof(List));
                }
            }

            return StatusCode(500);
        }

        // GET: AdminCustomerController/Details/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Details(int id)
        {
            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
                {
                    TempDataHelper.Set(TempData, AdminController.RedirectToActionTempDataKey, new LoginRedirectActionWithId(
                        nameof(Details),
                        ControllerHelper.GetControllerName<AdminCustomerController>(),
                        new RouteValueDictionary(new { id = id })));

                    return RedirectToAction(nameof(AdminController.Login), ControllerHelper.GetControllerName<AdminController>());
                }

                var customer = await _customerRepository.GetById(id);

                if (customer is not null)
                {
                    return View(new CustomerViewModel(customer) { IsRequestFromAnotherController = IsRequestFromAnotherController(CurrentControllerRoutePart) });
                }
            }

            return StatusCode(500);
        }

        // GET: AdminCustomerController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
                {
                    TempDataHelper.Set(TempData, AdminController.RedirectToActionTempDataKey, new LoginRedirectActionWithId(
                        nameof(Edit),
                        ControllerHelper.GetControllerName<AdminCustomerController>(),
                        new RouteValueDictionary(new { id = id })));

                    return RedirectToAction(nameof(AdminController.Login), ControllerHelper.GetControllerName<AdminController>());
                }

                var customer = await _customerRepository.GetById(id);

                if (customer is not null)
                {
                    return View(new CustomerEditViewModel(customer));
                }
            }

            return StatusCode(500);
        }

        // POST: AdminCustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int userId, CustomerEditViewModel customerViewModel)
        {
            if (string.IsNullOrEmpty(customerViewModel.Password))
            {
                customerViewModel.Password = "";
                ModelState[nameof(customerViewModel.Password)]!.ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Skipped;
            }

            if (ModelState.Count > 0 && ModelState.IsValid && userId > 0 && userId == customerViewModel.UserId &&
                UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                if (DataTransferHelper.TryTransferData(customerViewModel, out CustomerEntity customer))
                {
                    if (!string.IsNullOrEmpty(customerViewModel.Password))
                    {
                        await _customerRepository.Update(customer);
                    }
                    else
                    {
                        await _customerRepository.UpdateExcludePassword(customer);
                    }

                    return RedirectToAction(nameof(List));
                }
            }

            return StatusCode(500);
        }

        // GET: AdminCustomerController
        [HttpGet]
        public async Task<ActionResult> List()
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                TempDataHelper.Set(TempData, AdminController.RedirectToActionTempDataKey, new LoginRedirectActionWithId(
                    nameof(List),
                    ControllerHelper.GetControllerName<AdminCustomerController>()));

                return RedirectToAction(nameof(AdminController.Login), ControllerHelper.GetControllerName<AdminController>());
            }

            return View((await _customerRepository.GetAll()).Select(x => new CustomerViewModel(x)).ToList());
        }

        #endregion
    }
}
