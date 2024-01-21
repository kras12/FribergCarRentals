using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.DataAccess.DatabaseContexts;
using FribergCarRentals.DataAccess.Crypto;

namespace FribergCarRentals.DataAccess.Repositories
{
    /// <summary>
    /// A repository class to handle the customer entity.
    /// </summary>
    public class CustomerRepository : GenericRepository<CustomerEntity>, ICustomerRepository
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="databaseContext">The database context to use.</param>
        public CustomerRepository(ApplicationDbContext databaseContext) : base(databaseContext)
        {

        }

        #endregion

        #region Methods

        public async override Task<CustomerEntity> Add(CustomerEntity entity)
        {
            entity.Password = PasswordHelper.HashPassword(entity.Password);
            await _databaseContext.Set<CustomerEntity>().AddAsync(entity);
            _databaseContext.Entry(entity.UserRole).State = EntityState.Unchanged;
            await _databaseContext.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Deletes an entity from the database.
        /// </summary>
        /// <param name="id">The ID of the entity to delete.</param>
        /// <returns>A <see cref="Task"/> object containing true if the customer was deleted.</returns>
        public async Task<bool> Delete(int id)
        {
            var entity = new CustomerEntity() { UserId = id };
            _databaseContext.Customers.Remove(entity);
            return await _databaseContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Attempts to fetch a customer with matching email and password.
        /// </summary>
        /// <param name="email">The email for the customer.</param>
        /// <param name="password">The password for the customer.</param>
        /// <returns>A <see cref="Task"/> object containing the customer if found or null if not found.</returns>
        public async Task<CustomerEntity?> GetMatchingCustomer(string email, string password)
        {
            var customer = await _databaseContext.Customers.Where(x => x.Email == email).SingleOrDefaultAsync();

            if (customer is not null && PasswordHelper.VerifyHashedPassword(customer.Password, password))
            {
                customer.Password = "";
                return customer;
            }

            return null;
        }

        public async override Task<IEnumerable<CustomerEntity>> GetAll()
        {
            var customers = (await base.GetAll()).ToList();

            foreach (var customer in customers)
            {
                customer.Password = "";
            }

            return customers;
        }

        public async override ValueTask<CustomerEntity?> GetById(int id)
        {
            var customer = await base.GetById(id);

            if (customer is not null)
            {
                customer.Password = "";
            }

            return customer;
        }

        public async override Task<IEnumerable<CustomerEntity>> Find(Expression<Func<CustomerEntity, bool>> predicate)
        {
            var customers = (await base.Find(predicate)).ToList();

            foreach (var customer in customers)
            {
                customer.Password = "";
            }

            return customers;
        }

        public Task<CustomerEntity> UpdateExcludePassword(CustomerEntity entity)
        {
            _databaseContext.Entry(entity).State = EntityState.Unchanged;
            return base.Update(entity);
        }

        public override Task<CustomerEntity> Update(CustomerEntity entity)
        {
            if (string.IsNullOrEmpty(entity.Password))
            {
                entity.Password = PasswordHelper.HashPassword(entity.Password);
            }

            return base.Update(entity);
        }

        #endregion
    }
}
