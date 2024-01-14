using FribergCarRentals.Data;
using FribergCarRentals.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentals.DataAccess.Repositories
{
    /// <summary>
    /// A repository class to handle the car entity.
    /// </summary>
    public class CarRepository : GenericRepository<CarEntity>, ICarRepository
    {
        #region Constructors

        public CarRepository(ApplicationDbContext databaseContext) : base(databaseContext)
        {

        }

        #endregion

        #region Methods        

        public async Task Delete(int id)
        {
            var car = new CarEntity() { CarId = id };
            _databaseContext.Cars.Remove(car);
            await _databaseContext.SaveChangesAsync();
        }

        public async override Task<CarEntity> Add(CarEntity entity)
        {
            await _databaseContext.Set<CarEntity>().AddAsync(entity);
            _databaseContext.Entry(entity.PropulsionSystem).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
            _databaseContext.Entry(entity.RentalStatus).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
            await _databaseContext.SaveChangesAsync();
            return entity;
        }

        #endregion

    }
}
