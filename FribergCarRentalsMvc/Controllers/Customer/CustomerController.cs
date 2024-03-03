using FribergCarRentals.Controllers.Customer;
using FribergCarRentals.Data;
using FribergCarRentals.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using NuGet.Protocol;
using System.Collections.Generic;
using System.Linq;
using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.DataAccess.Repositories;
using FribergCarRentals.Models.Customer;
using FribergCarRentals.Sessions;
using FribergCarRentalsRazor.Helpers;

namespace FribergCarRentals.Controllers
{
    [Route($"{CurrentControllerRoutePart}/[action]")]
    public class CustomerController : ViewControllerBase
    {
        #region Constants

        /// <summary>
        /// The key used by other classes for storing redirect instructions to apply after the user have logged in.
        /// </summary>
        public const string RedirectInstructionsTempDataKey = "CustomerLoginRedirectToPage";

        /// <summary>
        /// The route part for this controller.
        /// </summary>
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

        // GET: CustomerController
        [HttpGet]
        public ActionResult Authenticate()
        {
            if (UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                return TempDataOrHomeRedirect();
            }

            return View(new RegisterOrLoginCustomerViewModel());
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterCustomerViewModel registerCustomerViewModel)
        {
            if (UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                return TempDataOrHomeRedirect();
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (!DataTransferHelper.TryTransferData(registerCustomerViewModel, out CustomerEntity customer))
                {
                    throw new Exception("Failed to transfer data from the view model to the entity.");
                }

                if (await _customerRepository.CustomerExists(customer.Email))
                {
                    // The key needs to be the name of the view model (insted of empty string) because the error is shown in a partial view. 
                    ModelState.AddModelError(nameof(RegisterCustomerViewModel), "An account already exists with that email.");
                }
                else
                {
                    await _customerRepository.AddAsync(customer);
                    LoginCustomer(customer);
                    return TempDataOrHomeRedirect();
                }
            }

            return View(nameof(Authenticate), new RegisterOrLoginCustomerViewModel() { RegisterCustomerViewModel = registerCustomerViewModel });
        }

        // Post: CustomerController
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginCustomerViewModel loginCustomerViewModel)
        {
            if (UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                return TempDataOrHomeRedirect();
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var customer = await _customerRepository.GetMatchingCustomerAsync(loginCustomerViewModel.Email, loginCustomerViewModel.Password);

                if (customer is null)
                {
                    // The key needs to be the name of the view model (insted of empty string) because the error is shown in a partial view. 
                    ModelState.AddModelError(nameof(LoginCustomerViewModel), "No account matched the entered email/password.");
                    return View(loginCustomerViewModel);
                }
                else
                {
                    LoginCustomer(customer);
                    //return RedirectToAction(nameof(CustomerOrderController.Book), "CustomerOrder", new { carid = 2003 });
                    return TempDataOrHomeRedirect();
                }
            }

            return View(loginCustomerViewModel);
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

        #endregion

        #region Methods

        /// <summary>
        /// Saves the customer user data in the session storage. 
        /// </summary>
        /// <param name="customer">The customer to login.</param>
        [NonAction]
        private void LoginCustomer(CustomerEntity customer)
        {
            UserSessionHandler.SetUserData(HttpContext.Session,
                    new UserSessionData(customer.UserId, customer.Email, customer.UserRole));
        }

        /// <summary>
        /// Redirects the customer to the page stored in the temp storage if such data exists, else redirects the customer to the homepage. 
        /// </summary>
        /// <returns><see cref="IActionResult"/>.</returns>
        [NonAction]
        private ActionResult TempDataOrHomeRedirect()
        {
            if (TempDataHelper.TryGet<RedirectToActionData>(TempData, RedirectInstructionsTempDataKey, out var data))
            {
                return RedirectToAction(data.Action, data.Controller, data.RouteValues);
            }

            return RedirectToAction(nameof(Index), ControllerHelper.GetControllerName<HomeController>());
        }

        #endregion
    }
}
