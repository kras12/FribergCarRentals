using FribergCarRentals.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Models;
using FribergCarRentals.Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FribergCarRentals.Controllers
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

        // GET: AdminCustomerController
        public async Task<ActionResult> List()
        {
            return View((await _customerRepository.GetAll()).Select(x => new CustomerViewModel(x)).ToList());
        }

        // GET: AdminCustomerController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var customer = await _customerRepository.GetById(id);

            if (customer is not null)
            {
                return View(new CustomerViewModel(customer) { IsRequestFromAnotherController = IsRequestFromAnotherController(CurrentControllerRoutePart) });
            }
            else
            {
                return RedirectToAction(nameof(List));
            }
        }

        // GET: AdminCustomerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminCustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CustomerFormViewModel customerViewModel)
        {
            try
            {
                if (ModelState.IsValid && DataTransferHelper.TryTransferData(customerViewModel, out CustomerEntity customer))
                {
                    customer.HashedPassword = PasswordHelper.CreateHashedPassword(customerViewModel.InputPassword);
                    await _customerRepository.Add(customer);
                    return RedirectToAction(nameof(List));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return View();
        }

        // GET: AdminCustomerController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var customer = await _customerRepository.GetById(id);

            if (customer is not null)
            {
                return View(new CustomerFormViewModel(customer));
            }
            else
            {
                return RedirectToAction(nameof(List));
            }
        }

        // POST: AdminCustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int userId, IFormCollection collection, CustomerFormViewModel customerViewModel)
        {
            if (ModelState.IsValid && userId > 0 && userId == customerViewModel.UserId &&
                DataTransferHelper.TryTransferData(customerViewModel, out CustomerEntity customer))
            {
                if (!string.IsNullOrEmpty(customerViewModel.InputPassword))
                {
                    customer.HashedPassword = PasswordHelper.CreateHashedPassword(customerViewModel.InputPassword);
                    await _customerRepository.Update(customer);
                }
                else
                {
                    await _customerRepository.UpdateExcludePassword(customer);
                }                    
                    
                return RedirectToAction(nameof(List));
            }

            return View();
        }

        // GET: AdminCustomerController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var customer = await _customerRepository.GetById(id);

            if (customer is not null)
            {
                return View(new CustomerViewModel(customer));
            }
            else
            {
                return RedirectToAction(nameof(List));
            }
        }

        // POST: AdminCustomerController/Delete/5
        [ActionName(nameof(Delete))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeletePost(int userId)
        {
            try
            {
                if (ModelState.IsValid && userId > 0)
                {
                    await _customerRepository.Delete(userId);
                    return RedirectToAction(nameof(List));
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }

            return View();
        }

        #endregion
    }
}
