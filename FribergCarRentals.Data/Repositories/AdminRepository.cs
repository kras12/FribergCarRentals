using FribergCarRentals.Data.DatabaseContexts;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Data.Exceptions;
using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FribergCarRentals.Shared.Constants;

namespace FribergCarRentals.Data.Repositories
{
    /// <summary>
    /// A repository class to handle the admin entity.
    /// </summary>
    /// <remarks>This repository class works on detached entities. All fetched entities will not be tracked by EF Core.</remarks>
    public class AdminRepository : IAdminRepository
    {
        #region Fields

        /// <summary>
        /// The database context.
        /// </summary>
        private readonly ApplicationDbContext _databaseContext;

        // The injected user manager.
        private readonly UserManager<ApplicationUser> _userManager;

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="databaseContext">The database context to use.</param>
        /// <param name="userManager">The injected user manager.</param>
        public AdminRepository(ApplicationDbContext databaseContext, UserManager<ApplicationUser> userManager)
        {
            _databaseContext = databaseContext;
            _userManager = userManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a new admin.
        /// </summary>
        /// <param name="admin">The admin to add.</param>
        /// <returns>A <see cref="Task"/> object containing the added admin.</returns>
        public async Task<AdminEntity> AddAsync(AdminEntity admin)
        {
            #region Checks

            if (admin.User == null)
            {
                throw new ArgumentNullException(nameof(admin.User), "The identity user can't be null.");
            }

            #endregion

            // Create user
            IdentityResult? createUserResult = await _userManager.CreateAsync(admin.User, admin.User.Password!);
            IdentityResult? addRoleResult = null;

            if (!createUserResult.Succeeded)
            {
                List<string> creationErrors = new(createUserResult.Errors.Select(x => x.Description));
                string creationErrorString = string.Join(Environment.NewLine, creationErrors);
                throw new CreateUserException(creationErrorString);
            }

            // Add user role
            addRoleResult = await _userManager.AddToRoleAsync(admin.User, ApplicationUserRoles.Admin);

            if (!addRoleResult.Succeeded)
            {
                await _userManager.DeleteAsync(admin.User);

                List<string> addRoleErrors = new(addRoleResult.Errors.Select(x => x.Description));
                string addRoleErrorString = string.Join(Environment.NewLine, addRoleErrors);
                throw new CreateUserException(addRoleErrorString);
            }

            // Create admin
            _databaseContext.Entry(admin.User).State = EntityState.Unchanged;
            _databaseContext.Admins.Add(admin);
            await _databaseContext.SaveChangesAsync();
            return admin;
        }

        /// <summary>
        /// Attempts to find an admin with a matching email address.
        /// </summary>
        /// <param name="email">The email of the admin.</param>
        /// <returns>A <see cref="Task"/> object containing true if a matching admin was found.</returns>
        public Task<bool> AdminExistsAsync(string email)
        {
            return _databaseContext.Admins.AnyAsync(x => x.User.Email == email.ToLower());
        }

        /// <summary>
        /// Returns true if there is any admins in the database.
        /// </summary>
        /// <returns>A <see cref="Task"/> object containing true if there is any admins in the database.</returns>
        public Task<bool> AnyAsync()
        {
            return _databaseContext.Admins.AnyAsync();
        }

        /// <summary>
        /// Gets an admin by email.
        /// </summary>
        /// <param name="email">The email of the admin.</param>
        /// <returns>A <see cref="Task"/> object containg the admin.</returns>
        public async Task<AdminEntity?> GetByEmailAsync(string email)
        {
            return await _databaseContext.Admins.Where(x => x.User.Email == email).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Attempts to fetch an admin by ID.
        /// </summary>
        /// <remarks>Returned entities will not be tracked by EF Core.</remarks>
        /// <param name="id">The ID of the admin.</param>
        /// <returns>A <see cref="Task"/> object containing the admin if found or null if not found.</returns>
        public Task<AdminEntity?> GetByIdAsync(int id)
        {
            return _databaseContext.Admins.Where(x => x.AdminId == id).AsNoTracking().SingleOrDefaultAsync();
        }

        /// <summary>
        /// Attempts to fetch an admin by user ID.
        /// </summary>
        /// <remarks>Returned entities will not be tracked by EF Core.</remarks>
        /// <param name="id">The ID of the admin.</param>
        /// <returns>A <see cref="Task"/> object containing the admin if found or null if not found.</returns>
        public Task<AdminEntity?> GetByUserIdAsync(string userId)
        {
            return _databaseContext.Admins.Where(x => x.User.Id == userId).AsNoTracking().SingleOrDefaultAsync();
        }

        /// <summary>
        /// Checks whether the admin's email address is confirmed.
        /// </summary>
        /// <param name="admin">The admin.</param>
        /// <returns>A <see cref="Task"/> object containing true if the email is confirmed.</returns>
        public Task<bool> IsEmailConfirmedAsync(AdminEntity admin)
        {
            return _userManager.IsEmailConfirmedAsync(admin.User);
        }

        #endregion
    }
}
