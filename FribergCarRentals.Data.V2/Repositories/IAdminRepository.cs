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
        /// Attempts to fetch an admin by ID.
        /// </summary>
        /// <remarks>Returned entities will not be tracked by EF Core.</remarks>
        /// <param name="id">The ID of the admin.</param>
        /// <returns>A <see cref="Task"/> object containing the admin if found or null if not found.</returns>
        public Task<AdminEntity?> GetByIdAsync(int id);

        #endregion
    }
}
