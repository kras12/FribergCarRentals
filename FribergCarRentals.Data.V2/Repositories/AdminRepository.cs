using FribergCarRentals.Data.DatabaseContexts;
using FribergCarRentals.Data.EntityClasses;
using Microsoft.EntityFrameworkCore;

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

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="databaseContext">The database context to use.</param>
        public AdminRepository(ApplicationDbContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        #endregion

        #region Methods

        // <summary>
        /// Adds a new admin.
        /// </summary>
        /// <param name="entity">The admin to add.</param>
        /// <returns>A <see cref="Task"/> object containing the added admin.</returns>
        public async Task<AdminEntity> AddAsync(AdminEntity entity)
        {
            await _databaseContext.Admins.AddAsync(entity);
            await _databaseContext.SaveChangesAsync();
            return entity;
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

        #endregion
    }
}
