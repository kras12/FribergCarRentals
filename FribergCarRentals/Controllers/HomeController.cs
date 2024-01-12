using FribergCarRentals.DataAccess.Repositories;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FribergCarRentals.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ICarRepository _carRepository;

        public HomeController(ILogger<HomeController> logger, ICarRepository carRepository)
        {
            _logger = logger;
            _carRepository = carRepository;
        }

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
            //var testCar = new CarEntity("Bmw", "Blue", "523", 2008, "ABC123", VehiclePropulsionEntity.CreateSeedObject(FribergCars.Shared.SharedTypes.VehiclePropulsionType.Gasoline), 
            //    CarRentalStatusEntity.CreateSeedObject(FribergCars.Shared.SharedTypes.CarRentalStatus.Available));
            //testCar.Images.Add(new ImageEntity(@"C:\test-image.jpg"));

            return View(await _carRepository.GetAll());
        }

        public IActionResult CarDetails(int i)
        {
            var testCar = new CarEntity("Bmw", "Blue", "523", 2008, "ABC123", VehiclePropulsionEntity.CreateSeedObject(FribergCars.Shared.SharedTypes.VehiclePropulsionType.Gasoline),
                CarRentalStatusEntity.CreateSeedObject(FribergCars.Shared.SharedTypes.CarRentalStatus.Available));
            testCar.Images.Add(new ImageEntity(@"C:\test-image.jpg"));

            return View(testCar);
        }
    }
}
