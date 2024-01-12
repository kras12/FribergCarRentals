using FribergCarRentals.Data;
using FribergCarRentals.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentals.DataAccess.Repositories
{
    public class CarRepository : GenericRepository<CarEntity>, ICarRepository
    {
        #region Constructors

        public CarRepository(ApplicationDbContext databaseContext) : base(databaseContext)
        {

        }

        #endregion

        #region Methods

        

        #endregion

    }
}
