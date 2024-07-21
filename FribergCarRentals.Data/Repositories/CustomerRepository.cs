using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Data.DatabaseContexts;
using FribergFastigheter.Server.Data.Entities;
using Microsoft.AspNetCore.Identity;
using FribergFastigheter.Shared.Constants;
using FribergCarRentals.Data.Exceptions;

namespace FribergCarRentals.Data.Repositories
{
    /// <summary>
    /// A repository class that handles the customer entity.
    /// </summary>
    /// <remarks>This repository class works on detached entities. All fetched entities will not be tracked by EF Core.</remarks>
    public class CustomerRepository : GenericRepository<CustomerEntity>, ICustomerRepository
    {
        #region Fields

        // The injected user manager.
        private readonly UserManager<ApplicationUser> _userManager;

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="databaseContext">The database context to use.</param>
        /// <param name="userManager">The injected user manager.</param>
        public CustomerRepository(ApplicationDbContext databaseContext, UserManager<ApplicationUser> userManager) : base(databaseContext)
        {
            _userManager = userManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a customer to the database.
        /// </summary>
        /// <param name="customer">The customer to add.</param>
        /// <returns>A <see cref="Task"/> object.</returns>
        /// <exception cref="CreateUserException"></exception>
        public async override Task AddAsync(CustomerEntity customer)
        {
            #region Checks

            if (customer.User == null)
            {
                throw new ArgumentNullException(nameof(customer.User), "The identity user can't be null.");
            }

            #endregion

            // Create user
            IdentityResult? createUserResult = await _userManager.CreateAsync(customer.User, customer.User.Password!);
            IdentityResult? addRoleResult = null;

            if (!createUserResult.Succeeded)
            {
                List<string> creationErrors = new(createUserResult.Errors.Select(x => x.Description));
                string creationErrorString = string.Join(Environment.NewLine, creationErrors);
                throw new CreateUserException(creationErrorString);
            }

            // Add user role
            addRoleResult = await _userManager.AddToRoleAsync(customer.User, ApplicationUserRoles.Customer);

            if (!addRoleResult.Succeeded)
            {
                await _userManager.DeleteAsync(customer.User);

                List<string> addRoleErrors = new(addRoleResult.Errors.Select(x => x.Description));
                string addRoleErrorString = string.Join(Environment.NewLine, addRoleErrors);
                throw new CreateUserException(addRoleErrorString);
            }

            // Create customer
            _databaseContext.Entry(customer.User).State = EntityState.Unchanged;
            _databaseContext.Customers.Add(customer);
            await _databaseContext.SaveChangesAsync();
        }

        /// <summary>
        /// Checks whether a customer with the specified email exists. 
        /// </summary>
        /// <param name="email">The email for the customer.</param>
        /// <returns>A <see cref="Task"/> object containing true if there was a matching customer.</returns>
        public Task<bool> CustomerExists(string email)
        {
            return _databaseContext.Customers.AnyAsync(x => x.User.Email == email);
        }

        /// <summary>
        /// Deletes a customer from the database.
        /// </summary>
        /// <param name="id">The ID of the customer to delete.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public Task DeleteAsync(int id)
        {
            var entity = new CustomerEntity() { CustomerId = id };
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
        /// Gets a customer by ID.
        /// </summary>
        /// <remarks>Returned entities will not be tracked by EF Core.</remarks>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>A <see cref="Task"/> object containg the customer.</returns>
        public async override Task<CustomerEntity?> GetByIdAsync(int id)
        {
            return await _databaseContext.Customers.AsNoTracking().SingleOrDefaultAsync(x => x.CustomerId == id);
        }

        /// <summary>
        /// Attempts to fetch a customer by user ID.
        /// </summary>
        /// <remarks>Returned entities will not be tracked by EF Core.</remarks>
        /// <param name="id">The ID of the admin.</param>
        /// <returns>A <see cref="Task"/> object containing the admin if found or null if not found.</returns>
        public Task<CustomerEntity?> GetByUserIdAsync(string userId)
        {
            return _databaseContext.Customers.AsNoTracking().SingleOrDefaultAsync(x => x.User.Id == userId);
        }

        /// <summary>
        /// Attempts to fetch the user ID for the customer.
        /// </summary>
        /// <param name="id">The ID of the customer.</param>
        /// <returns>A <see cref="Task"/> object containing the user ID if found or null if not found.</returns>
        public Task<string?> GetUserId(int id)
        {
            return _databaseContext.Customers.AsNoTracking().Where(x => x.CustomerId == id).Select(x => x.User.Id).SingleOrDefaultAsync();
        }

        #endregion
    }
}
