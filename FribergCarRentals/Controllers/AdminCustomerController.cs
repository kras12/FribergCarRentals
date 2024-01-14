using FribergCarRentals.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Models;
using FribergCarRentals.Data;

namespace FribergCarRentals.Controllers
{
    public class AdminCustomerController : Controller
    {
        #region Fields

        private readonly ICustomerRepository _customerRepository;

        #endregion

        #region Constructors

        public AdminCustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        #endregion

        // GET: AdminCustomerController
        public async Task<ActionResult> List()
        {
            return View((await _customerRepository.GetAll()).Select(x => new CustomerViewModel(x)).ToList());
        }

        // GET: AdminCustomerController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: AdminCustomerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminCustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CustomerViewModel customerViewModel)
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
                return View(new CustomerViewModel(customer));
            }
            else
            {
                return RedirectToAction(nameof(List));
            }
        }

        // POST: AdminCustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int userId, IFormCollection collection, CustomerViewModel customerViewModel)
        {
            try
            {
                if (ModelState.IsValid && userId > 0 && userId == customerViewModel.UserId &&
                    DataTransferHelper.TryTransferData(customerViewModel, out CustomerEntity customer))
                {
                    customer.HashedPassword = PasswordHelper.CreateHashedPassword(customerViewModel.InputPassword);
                    await _customerRepository.Update(customer);
                    return RedirectToAction(nameof(List));
                }
            }
            catch (Exception)
            {

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
    }
}
