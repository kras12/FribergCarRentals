using FribergCarRentals.DataAccess.Repositories;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FribergCarRentals.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly ICarRepository _carRepository;

        public HomeController(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [Route("Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("Cars")]
        public async Task<IActionResult> Cars()
        {
            //var testCar = new CarEntity("Bmw", "Blue", "523", 2008, "ABC123", VehiclePropulsionEntity.CreateSeedObject(FribergCars.Shared.SharedTypes.VehiclePropulsionType.Gasoline), 
            //    CarRentalStatusEntity.CreateSeedObject(FribergCars.Shared.SharedTypes.CarRentalStatus.Available));
            //testCar.Images.Add(new ImageEntity(@"C:\test-image.jpg"));

            return View(await _carRepository.GetAll());
        }

        [Route("Car/{id}")]
        public async Task<IActionResult> Car(int id)
        {
            //var testCar = new CarEntity("Bmw", "Blue", "523", 2008, "ABC123", VehiclePropulsionEntity.CreateSeedObject(FribergCars.Shared.SharedTypes.VehiclePropulsionType.Gasoline),
            //    CarRentalStatusEntity.CreateSeedObject(FribergCars.Shared.SharedTypes.CarRentalStatus.Available));
            //testCar.Images.Add(new ImageEntity(@"C:\test-image.jpg"));

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
    }
}
