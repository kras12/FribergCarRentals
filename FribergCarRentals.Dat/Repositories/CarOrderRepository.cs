using FribergCarRentals.Data;
using FribergCarRentals.Models;
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
    public class CarOrderRepository : GenericRepository<CarOrderEntity>, ICarOrderRepository
    {
        #region Constructors

        public CarOrderRepository(ApplicationDbContext databaseContext) : base(databaseContext)
        {

        }

        #endregion

        #region Methods        

        public async Task Delete(int id)
        {
            var carOrder = new CarOrderEntity() { CarOrderId = id };
            _databaseContext.CarOrders.Remove(carOrder);
            await _databaseContext.SaveChangesAsync();
        }

        #endregion
    }
}
