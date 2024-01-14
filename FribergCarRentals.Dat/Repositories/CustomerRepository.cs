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
    /// A repository class to handle the customer entity.
    /// </summary>
    public class CustomerRepository : GenericRepository<CustomerEntity>, ICustomerRepository
    {
        #region Constructors

        public CustomerRepository(ApplicationDbContext databaseContext) : base(databaseContext)
        {

        }

        #endregion

        #region Methods        

        public async override Task<CustomerEntity> Add(CustomerEntity entity)
        {
            await _databaseContext.Set<CustomerEntity>().AddAsync(entity);
            _databaseContext.Entry(entity.UserRole).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
            await _databaseContext.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(int id)
        {
            var entity = new CustomerEntity() { UserId = id };
            _databaseContext.Customers.Remove(entity);
            await _databaseContext.SaveChangesAsync();
        }

        #endregion
    }
}
