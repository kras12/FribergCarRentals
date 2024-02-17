using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.DataAccess.Repositories;
using FribergCarRentals.DataAccess.Types;
using FribergCarRentals.Helpers;
using FribergCarRentals.Models.Other;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Drawing2D;

namespace FribergCarRentalsRazor.Components
{
    /// <summary>
    /// A class that handles image slideshows for cars.
    /// </summary>
    public class CarImageSlideShow : ViewComponent
    {
        #region Fields

        /// <summary>
        /// The injected car repository.
        /// </summary>
        private readonly ICarRepository _carRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="carRepository">Injected car repository.</param>
        public CarImageSlideShow(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The invoke method.
        /// </summary>
        /// <param name="onlyRentableCars">True if only rentable cars should be included.</param>
        /// <param name="maxImagesPerCar">The max number of images per car.</param>
        /// <param name="maxCarCount">The max number of car entities to include.</param>
        /// <param name="prefixImages">An optional collection of images to add at the beginning of the slide show.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing an <see cref="IViewComponentResult"/>.</returns>
        public async Task<IViewComponentResult> InvokeAsync(bool onlyRentableCars, int? maxImagesPerCar = 1, int? maxCarCount = null,
            List<ImageViewModel>? prefixImages = null)
        {
            List<ImageEntity> carEntityImages = new();
            List<CarEntity> carEntities;
            List<ImageViewModel> images = new();

            if (onlyRentableCars)
            {
                carEntities = (await _carRepository.GetAllAsync(CarRentalStatusEntity.CreateFromType(RentalCarStatus.Rentable))).ToList();
            }
            else
            {
                carEntities = (await _carRepository.GetAllAsync()).ToList();
            }

            if (maxCarCount is not null && maxCarCount > 0)
            {
                carEntities = carEntities.Take(maxCarCount.Value).ToList();
            }

            if (maxImagesPerCar > 0)
            {
                carEntityImages = carEntities.SelectMany(x => x.Images.Take(maxImagesPerCar.Value)).ToList();
            }
            else
            {
                carEntityImages = carEntities.SelectMany(x => x.Images).ToList();
            }

            if (prefixImages != null && prefixImages.Count > 0)
            {
                prefixImages.ForEach(x => x.LinksToAction = new FribergCarRentals.Data.RedirectToPageData("Cars"));
                images = prefixImages;
			}

            images.AddRange(carEntityImages.Select(x => new ImageViewModel(
                ImageHelper.GetImageFileUrl(x), x.FileName, x.ImageId, 
                new FribergCarRentals.Data.RedirectToPageData("Cars", urlFragment: x.Car!.CarId.ToString()))));

			return View(new ListViewModel<ImageViewModel>(images));
        }

        #endregion
    }
}
