using FribergCarRentals.Data;
using FribergCarRentals.Models;
using FribergCarRentals.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentals.Data.Repositories
{
    /// <summary>
    /// A repository class to handle the car entity.
    /// </summary>
    public class CarRepository : GenericRepository<CarEntity>, ICarRepository
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="databaseContext">The database context to use.</param>
        public CarRepository(ApplicationDbContext databaseContext) : base(databaseContext)
        {

        }

        #endregion

        #region Methods        

        /// <summary>
        /// Attempts to fetch all cars with a specific rental status.
        /// </summary>
        /// <param name="rentalStatus">The rental status of the cars.</param>
        /// <returns>A <see cref="IEnumerable{T}"/> that contains matched cars.</returns>
        public async Task<IEnumerable<CarEntity>> GetAll(CarRentalStatusEntity rentalStatus)
        {
            return await _databaseContext.Cars.Where(x => x.RentalStatus == rentalStatus).ToListAsync();
        }

        /// <summary>
        /// Attempts to fetch a car with a specific ID and rental status.
        /// </summary>
        /// <param name="id">The ID of the car..</param>
        /// <param name="rentalStatus">The rental status of the car.</param>
        /// <returns>a <see cref="CarEntity"/> if the car was found. Null if not.</returns>
        public Task<CarEntity?> GetById(int id, CarRentalStatusEntity rentalStatus)
        {
            return _databaseContext.Cars.SingleOrDefaultAsync(x => x.CarId == id && x.RentalStatus == rentalStatus);
        }

        /// <summary>
        /// Deletes a car from the database.
        /// </summary>
        /// <param name="id">The ID of the car to delete.</param>
        /// <returns>A <see cref="Task{TResult}"/> object containing true if the car was deleted. False if not.</returns>
        public async Task<bool> Delete(int id)
        {
            var car = new CarEntity() { CarId = id };
            _databaseContext.Cars.Remove(car);
            return await _databaseContext.SaveChangesAsync() > 0;
        }

        public async override Task<CarEntity> Add(CarEntity entity)
        {
            await _databaseContext.Set<CarEntity>().AddAsync(entity);
            _databaseContext.Entry(entity.PropulsionSystem).State = EntityState.Unchanged;
            _databaseContext.Entry(entity.RentalStatus).State = EntityState.Unchanged;
            await _databaseContext.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Returns all the cars that are availble to be rented out. 
        /// </summary>
        /// <returns>A collection of cars.</returns>
        public Task<List<CarEntity>> GetRentableCars()
        {
            return _databaseContext.Cars.Where(x => x.RentalStatus!.StatusType == CarRentalStatus.Available).ToListAsync();
        }

        public async override Task<CarEntity> Update(CarEntity entity)
        {
            var oldCar = await GetById(entity.CarId);

            if (oldCar is not null)
            {
                // Copy car data
                _databaseContext.Entry(oldCar).CurrentValues.SetValues(entity);

                // Existing images
                var existingImages = oldCar.Images.IntersectBy(entity.Images.Select(x => x.FilePath), y => y.FilePath).ToList();

                foreach (var existingImage in existingImages)
                {
                    var sourceImage = entity.Images.Single(y => y.FilePath == existingImage.FilePath);
                    sourceImage.ImageId = existingImage.ImageId;
                    _databaseContext.Entry(existingImage).CurrentValues.SetValues(sourceImage);
                }

                // New images
                var newImages = entity.Images.ExceptBy(oldCar.Images.Select(x => x.FilePath), y => y.FilePath).ToList();
                oldCar.Images.AddRange(newImages);

                // Deleted images
                var deletedImages = oldCar.Images.ExceptBy(entity.Images.Select(x => x.FilePath), y => y.FilePath).ToList();
                deletedImages.ForEach(x => oldCar.Images.Remove(x));

                // Save
                await _databaseContext.SaveChangesAsync();
                return oldCar;
            }

            throw new Exception("The car could not be found.");
        }

        #endregion

    }
}
