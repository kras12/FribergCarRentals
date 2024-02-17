using FribergCarRentals.Controllers.Customer;
using FribergCarRentals.Data;
using FribergCarRentals.Helpers;
using FribergCarRentals.Data.Admin;
using FribergCarRentals.Data.Customer;
using FribergCarRentals.Data.Order;
using FribergCarRentals.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using NuGet.Protocol;
using System.Collections.Generic;
using System.Linq;
using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.DataAccess.Repositories;

namespace FribergCarRentals.Controllers
{
    [Route($"{CurrentControllerRoutePart}/[action]")]
    public class CustomerController : ViewControllerBase
    {
        #region Constants

        public const string RedirectToActionTempDataKey = "CustomerLoginRedirectToAction";
        private const string CurrentControllerRoutePart = "Customer";

        #endregion

        #region Fields

        private readonly ICustomerRepository _customerRepository;

        #endregion

        #region Constructors

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        #endregion

        #region Actions

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CustomerRegisterViewModel customerViewModel)
        {
            if (ModelState.Count > 0 && ModelState.IsValid && !UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                if (DataTransferHelper.TryTransferData(customerViewModel, out CustomerEntity customer))
                {
                    var addedCustomer = await _customerRepository.Add(customer);
                    LoginCustomer(addedCustomer);
                    return TempDataOrHomeRedirect();
                }                
            }

            return StatusCode(500);
        }

        // Post: CustomerController
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(CustomerLoginViewModel customerModel)
        {
            if (ModelState.Count > 0 && ModelState.IsValid && !UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                var customer = await _customerRepository.GetMatchingCustomer(customerModel.Email, customerModel.Password);                

                if (customer is not null)
                {
                    LoginCustomer(customer);
                    return TempDataOrHomeRedirect();
                }
            }

            return RedirectToAction(nameof(CustomerOrderController.List), ControllerHelper.GetControllerName<CustomerOrderController>());
        }

        // GET: CustomerController
        //[Route("Logout")]
        public ActionResult Logout()
        {
            if (UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                UserSessionHandler.RemoveUserData(HttpContext.Session);
            }

            return RedirectToAction(nameof(Index), ControllerHelper.GetControllerName<HomeController>());
        }

        // GET: CustomerController
        [HttpGet]
        public ActionResult Authenticate()
        {
            if (UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                return RedirectToAction(nameof(HomeController.Index), ControllerHelper.GetControllerName<HomeController>());
            }

            return View(new CustomerRegisterOrLoginViewModel());
        }
        #endregion

        #region Methods

        [NonAction]
        private void LoginCustomer(CustomerEntity customer)
        {
            UserSessionHandler.SetUserData(HttpContext.Session,
                    new UserSessionData(customer.UserId, customer.Email, customer.UserRole));
        }

        [NonAction]
        private ActionResult TempDataOrHomeRedirect()
        {
            if (TempDataHelper.TryGet<RedirectToAction>(TempData, RedirectToActionTempDataKey, out var data))
            {
                return RedirectToAction(data.Action, data.Controller, data.RouteValues);
            }

            return RedirectToAction(nameof(Index), ControllerHelper.GetControllerName<HomeController>());
        }

        #endregion
    }
}
