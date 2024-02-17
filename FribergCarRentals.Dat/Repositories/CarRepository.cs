using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.DataAccess.DatabaseContexts;
using FribergCarRentals.DataAccess.Types;
using static System.Net.Mime.MediaTypeNames;
using System.Threading.Channels;

namespace FribergCarRentals.DataAccess.Repositories
{
    /// <summary>
    /// A repository class that handles the car entity.
    /// </summary>
    /// <remarks>This repository class works on detached entities. All fetched entities will not be tracked by EF Core.</remarks>
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
        /// Adds a car to the database.
        /// </summary>
        /// <param name="entity">The car to add.</param>
        /// <returns></returns>
        public async override Task AddAsync(CarEntity entity)
        {
            await _databaseContext.Set<CarEntity>().AddAsync(entity);
            SetEnumPropertiesTrackingStateUnchanged(entity);
            await _databaseContext.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a car from the database.
        /// </summary>
        /// <param name="id">The ID of the car to delete.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public Task DeleteAsync(int id)
        {
            var car = new CarEntity() { CarId = id };
            _databaseContext.Cars.Remove(car);
            return _databaseContext.SaveChangesAsync();
        }

        /// <summary>
        /// Attempts to fetch all cars with a specific rental status.
        /// </summary>
        /// <remarks>Returned entities will not be tracked by EF Core.</remarks>
        /// <param name="rentalStatus">The rental status of the cars.</param>
        /// <returns>A <see cref="Task{TResult}{T}"/> that contains a collection of matched cars.</returns>
        public async Task<IEnumerable<CarEntity>> GetAllAsync(CarRentalStatusEntity rentalStatus)
        {
            return await _databaseContext.Cars.Where(x => x.RentalStatus == rentalStatus).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Attempts to fetch a car with a specific ID and rental status.
        /// </summary>
        /// <remarks>Returned entities will not be tracked by EF Core.</remarks>
        /// <param name="id">The ID of the car.</param>
        /// <param name="rentalStatus">The rental status of the car.</param>
        /// <returns>A <see cref="Task{TResult}"/> that contains the matched car, or null if not found.</returns>
        public Task<CarEntity?> GetByIdAsync(int id, CarRentalStatusEntity rentalStatus)
        {
            return _databaseContext.Cars.AsNoTracking().SingleOrDefaultAsync(x => x.CarId == id && x.RentalStatus == rentalStatus);
        }

        /// <summary>
        /// Attempts to fetch a car with a specific ID.
        /// </summary>
        /// <remarks>Returned entities will not be tracked by EF Core.</remarks>
        /// <param name="id">The ID of the car.</param>
        /// <remarks>This override is needed to disable tracking since the base class lacks this ability.</remarks>
        /// <returns>a <see cref="Task{TResult}"/> containing the car if found, or null if not found.</returns>
        public override Task<CarEntity?> GetByIdAsync(int id)
        {
            return _databaseContext.Cars.AsNoTracking().SingleOrDefaultAsync(x => x.CarId == id);
        }

        /// <summary>
        /// Attempts to fetch all images for a car with a specific ID. 
        /// </summary>
        /// <remarks>Returned entities will not be tracked by EF Core.</remarks>s>
        /// <param name="id">The ID of the car.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing a collection containing the images found.</returns>
        public async Task<IEnumerable<ImageEntity>> GetCarImagesAsync(int id)
        {
            // EF Core doesn't like the combination of include and AsNoTracking in this case, so we do a work around. 
            var car = await GetByIdAsync(id);
            return car?.Images ?? new List<ImageEntity>();
        }

        /// <summary>
        /// Returns all the cars that are availble to be rented out. 
        /// </summary>
        /// <remarks>Returned entities will not be tracked by EF Core.</remarks>
        /// <returns>A <see cref="Task{TResult}{T}"/> that contains a collection of matched cars.</returns>
        public async Task<IEnumerable<CarEntity>> GetRentableCarsAsync()
        {
            return await _databaseContext.Cars.Where(x => x.RentalStatus!.StatusType == RentalCarStatus.Rentable).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Updates the car entity in the database. 
        /// </summary>
        /// <param name="entity">The car to update.</param>
        /// <returns>A <see cref="Task{TResult}{T}"/> object.</returns>
        public async override Task UpdateAsync(CarEntity entity)
        {
            // We must store the final images outside of the entity for our comparisions,
            // since EF Core will add tracked images to the entity if they don't exist.
            var targetImages = entity.Images.ToList();
            _databaseContext.Update(entity);
            SetEnumPropertiesTrackingStateUnchanged(entity);

            // EF Core will add missing images to the entity here.
            var databaseImages = await _databaseContext.Images.Where(x => x.Car!.CarId == entity.CarId).ToListAsync();
            
            // Modify status for deleted images
            databaseImages.ExceptBy(targetImages.Select(x => x.ImageId), y => y.ImageId).ToList()
                .ForEach(deletedImage =>
                {
                    _databaseContext.Images.Entry(deletedImage).State = EntityState.Deleted;
                });

            await _databaseContext.SaveChangesAsync();
        }

        /// <summary>
        /// Sets the necessary EF Core tracking state for enum properties in the car entity. 
        /// This is needed to instruct the framework that the status entities already exists in the database.
        /// </summary>
        /// <param name="entity">The car to set the tracking states for.</param>
        private void SetEnumPropertiesTrackingStateUnchanged(CarEntity entity)
        {
            // EF Core needs to know that the states already exists in the database
            _databaseContext.Entry(entity.RentalStatus!).State = EntityState.Unchanged;
            _databaseContext.Entry(entity.PropulsionSystem!).State = EntityState.Unchanged;
        }

        #endregion

    }
}
