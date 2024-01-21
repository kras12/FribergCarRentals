using FribergCarRentals.DataAccess.Repositories;
using FribergCarRentals.Models;
using FribergCarRentals.Models.Car;
using FribergCarRentals.Models.Customer;
using FribergCars.Shared.SharedTypes;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FribergCarRentals.Controllers
{
    [Route("/[action]")]
    public class HomeController : ViewControllerBase
    {
        #region Fields

        private readonly ICarRepository _carRepository;

        #endregion

        #region Constructor

        public HomeController(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        #endregion

        #region Actions        

        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Cars()
        {
            return View((await _carRepository.GetAll(CarRentalStatusEntity.CreateSeedObject(CarRentalStatus.Available)))
                .Select(x => new CarViewModel(x)).ToList());
        }

        public async Task<IActionResult> Car(int id)
        {
            var car = await _carRepository.GetById(id);

            if (car is not null)
            {
                return View(car);
            }
            else
            {
                return RedirectToAction(nameof(Cars));
            }
        }

        #endregion
    }
}
