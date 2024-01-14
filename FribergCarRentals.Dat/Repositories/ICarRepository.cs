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

        public Task Delete(int id);

        #endregion
    }
}
