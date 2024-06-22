using FribergCarRentals.Data.EntityClasses;

namespace FribergCarRentals.Data.Repositories
{
    /// <summary>
    /// An interface for an admin repository.
    /// </summary>
    public interface IAdminRepository
    {
        #region Methods

        /// <summary>
        /// Adds a new admin.
        /// </summary>
        /// <param name="entity">The admin to add.</param>
        /// <returns>A <see cref="Task"/> object containing the added admin.</returns>
        public Task<AdminEntity> AddAsync(AdminEntity entity);

        /// <summary>
        /// Returns true if there is any admins in the database.
        /// </summary>
        /// <returns>A <see cref="Task"/> object containing true if there is any admins in the database.</returns>
        public Task<bool> AnyAsync();

        /// <summary>
        /// Attempts to fetch an admin by ID.
        /// </summary>
        /// <remarks>Returned entities will not be tracked by EF Core.</remarks>
        /// <param name="id">The ID of the admin.</param>
        /// <returns>A <see cref="Task"/> object containing the admin if found or null if not found.</returns>
        public Task<AdminEntity?> GetByIdAsync(int id);

        /// <summary>
        /// Attempts to fetch an admin by user ID.
        /// </summary>
        /// <remarks>Returned entities will not be tracked by EF Core.</remarks>
        /// <param name="id">The ID of the admin.</param>
        /// <returns>A <see cref="Task"/> object containing the admin if found or null if not found.</returns>
        public Task<AdminEntity?> GetByUserIdAsync(string userId);

        #endregion
    }
}
