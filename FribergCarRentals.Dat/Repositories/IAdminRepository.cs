using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FribergCarRentals.DataAccess.EntityClasses;

namespace FribergCarRentals.DataAccess.Repositories
{
    public interface IAdminRepository
    {
        #region Methods

        /// <summary>
        /// Attempts to fetch an admin by ID.
        /// </summary>
        /// <param name="id">The ID of the admin.</param>
        /// <returns>A <see cref="Task"/> object containing the admin if found or null if not found.</returns>
        public Task<AdminEntity?> GetById(int id);

        /// <summary>
        /// Attempts to fetch an admin with matching email and password.
        /// </summary>
        /// <param name="email">The email for the admin.</param>
        /// <param name="password">The password for the admin.</param>
        /// <returns>A <see cref="Task"/> object containing the admin if found or null if not found.</returns>
        public Task<AdminEntity?> GetMatchingAdmin(string email, string password);

        #endregion
    }
}
