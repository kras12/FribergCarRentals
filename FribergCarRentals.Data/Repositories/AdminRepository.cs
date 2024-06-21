using FribergCarRentals.DataAccess.Crypto;
using FribergCarRentals.DataAccess.DatabaseContexts;
using FribergCarRentals.DataAccess.EntityClasses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentals.DataAccess.Repositories
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
            return _databaseContext.Admins.Where(x => x.UserId == id).AsNoTracking().SingleOrDefaultAsync();
        }

        /// <summary>
        /// Attempts to fetch an admin with matching email and password.
        /// </summary>
        /// <remarks>Returned entities will not be tracked by EF Core.</remarks>
        /// <param name="email">The email for the admin.</param>
        /// <param name="password">The password for the admin.</param>
        /// <returns>A <see cref="Task"/> object containing the admin if found or null if not found.</returns>
        public async Task<AdminEntity?> GetMatchingAdminAsync(string email, string password)
        {
            var admin = await _databaseContext.Admins.AsNoTracking().SingleOrDefaultAsync(x => x.Email == email);

            if (admin is not null && PasswordHelper.VerifyAgainstHashedPassword(admin.Password, password))
            {
                admin.Password = "";
                return admin;
            }

            return null;
        }

        #endregion
    }
}
