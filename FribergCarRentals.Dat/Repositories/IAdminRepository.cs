using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FribergCarRentals.DataAccess.EntityClasses;

namespace FribergCarRentals.DataAccess.Repositories
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

        /// <summary>
        /// Attempts to fetch an admin with matching email and password.
        /// </summary>
        /// <remarks>Returned entities will not be tracked by EF Core.</remarks>
        /// <param name="email">The email for the admin.</param>
        /// <param name="password">The password for the admin.</param>
        /// <returns>A <see cref="Task"/> object containing the admin if found or null if not found.</returns>
        public Task<AdminEntity?> GetMatchingAdminAsync(string email, string password);

        #endregion
    }
}
