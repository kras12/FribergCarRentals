using FribergCarRentals.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentals.DataAccess.Repositories
{
    /// <summary>
    /// An interface for a car repository.
    /// </summary>
    public interface ICarRepository : IRepositoryBase<CarEntity>
    {
        // Even if it's empty it still helps promotes the open closed principle in the project. 

        #region Methods

        //public CarEntity? GetCar(int id);

        //public bool TryGetCar(int id, [NotNullWhen(true)] out CarEntity? result);

        //public IEnumerable<CarEntity> GetAllCars();

        //public CarEntity UpdateCar(CarEntity entity);

        //public CarEntity? GetCar(CarEntity entity);

        //public CarEntity AddCar(CarEntity entity);

        //public IEnumerable<CarEntity> FindCar(Expression<Func<CarEntity, bool>> predicate);

        #endregion
    }
}
