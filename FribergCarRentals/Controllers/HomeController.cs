using FribergCarRentals.Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.DataAccess.Repositories;
using FribergCarRentals.DataAccess.Types;
using FribergCarRentals.Models.Other;
using FribergCarRentals.Models.Car;

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Cars()
        {
            ListViewModel<CarViewModel> viewModel = new ListViewModel<CarViewModel>((await _carRepository.GetAllAsync(CarRentalStatusEntity.CreateFromType(RentalCarStatus.Rentable)))
                .Select(x => new CarViewModel(x))
                .ToList());

            return View(viewModel);
        }

        #endregion
    }
}
