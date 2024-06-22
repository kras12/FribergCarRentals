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
