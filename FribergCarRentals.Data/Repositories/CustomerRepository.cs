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
    /// A repository class that handles the customer entity.
    /// </summary>
    /// <remarks>This repository class works on detached entities. All fetched entities will not be tracked by EF Core.</remarks>
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

        /// <summary>
        /// Adds a customer to the database.
        /// </summary>
        /// <param name="entity">The customer to add.</param>
        /// <returns>A <see cref="Task"/> object.</returns>
        public async override Task AddAsync(CustomerEntity entity)
        {
            entity.Password = PasswordHelper.HashPassword(entity.Password);
            await _databaseContext.Set<CustomerEntity>().AddAsync(entity);
            _databaseContext.Entry(entity.UserRole).State = EntityState.Unchanged;
            await _databaseContext.SaveChangesAsync();
        }

        /// <summary>
        /// Checks whether a customer with the specified email exists. 
        /// </summary>
        /// <param name="email">The email for the customer.</param>
        /// <returns>A <see cref="Task"/> object containing true if there was a matching customer.</returns>
        public Task<bool> CustomerExists(string email)
        {
            return _databaseContext.Customers.AnyAsync(x => x.Email == email);
        }

        /// <summary>
        /// Deletes a customer from the database.
        /// </summary>
        /// <param name="id">The ID of the customer to delete.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public Task DeleteAsync(int id)
        {
            var entity = new CustomerEntity() { UserId = id };
            _databaseContext.Customers.Remove(entity);
            return _databaseContext.SaveChangesAsync();
        }


        /// <summary>
        /// Performs a search in the database.
        /// </summary>
        /// <remarks>The resulting entities is not tracked by EF Core.</remarks>
        /// <param name="predicate">The predicate to use.</param>
        /// <returns>A <see cref="Task"/> object containing a collection of the resulting customers.</returns>
        public override Task<IEnumerable<CustomerEntity>> FindAsync(Expression<Func<CustomerEntity, bool>> predicate)
        {
            return base.FindAsync(predicate);
        }

        /// <summary>
        /// Gets all customers from the database.
        /// </summary>
        /// <remarks>The resulting entities is not tracked by EF Core.</remarks>
        /// <returns>A <see cref="Task"/> object containg a collection of all customers found.</returns>
        public async override Task<IEnumerable<CustomerEntity>> GetAllAsync()
        {
            var customers = (await base.GetAllAsync()).ToList();

            foreach (var customer in customers)
            {
                customer.Password = "";
            }

            return customers;
        }

        /// <summary>
        /// Gets a customer by ID.
        /// </summary>
        /// <remarks>Returned entities will not be tracked by EF Core.</remarks>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>A <see cref="Task"/> object containg the customer.</returns>
        public async override Task<CustomerEntity?> GetByIdAsync(int id)
        {
            var customer = await _databaseContext.Customers.AsNoTracking().SingleOrDefaultAsync(x => x.UserId == id);

            if (customer is not null)
            {
                customer.Password = "";
            }

            return customer;
        }

        /// <summary>
        /// Attempts to fetch a customer with matching email and password.
        /// </summary>
        /// <remarks>Returned entities will not be tracked by EF Core.</remarks>
        /// <param name="email">The email for the customer.</param>
        /// <param name="password">The password for the customer.</param>
        /// <returns>A <see cref="Task"/> object containing the customer if found or null if not found.</returns>
        public async Task<CustomerEntity?> GetMatchingCustomerAsync(string email, string password)
        {
            var customer = await _databaseContext.Customers.AsNoTracking().Where(x => x.Email == email).SingleOrDefaultAsync();

            if (customer is not null && PasswordHelper.VerifyAgainstHashedPassword(customer.Password, password))
            {
                customer.Password = "";
                return customer;
            }

            return null;
        }

        /// <summary>
        /// Updates a customer in the database.
        /// </summary>
        /// <param name="entity">The customer to update.</param>
        /// <returns>A <see cref="Task"/> object.</returns>
        public override Task UpdateAsync(CustomerEntity entity)
        {
            if (!string.IsNullOrEmpty(entity.Password))
            {
                entity.Password = PasswordHelper.HashPassword(entity.Password);
            }

            return base.UpdateAsync(entity);
        }

        /// <summary>
        /// Updates a customer in the database without updating the password.
        /// </summary>
        /// <param name="entity">The customer to update.</param>
        /// <returns>A <see cref="Task"/> object.</returns>
        public async Task UpdateExcludePasswordAsync(CustomerEntity entity)
        {
            entity.Password = await _databaseContext.Customers.Where(x => x.UserId == entity.UserId).Select(x => x.Password).SingleAsync();
            await base.UpdateAsync(entity);
        }

        #endregion
    }
}
