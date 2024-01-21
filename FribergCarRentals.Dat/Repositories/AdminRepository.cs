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
    /// A repository class to handle the customer entity.
    /// </summary>
    public class AdminRepository : IAdminRepository
    {
        #region Fields

        protected readonly ApplicationDbContext _databaseContext;

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
        /// Attempts to fetch an admin with matching email and password.
        /// </summary>
        /// <param name="email">The email for the admin.</param>
        /// <param name="password">The password for the admin.</param>
        /// <returns>A <see cref="Task"/> object containing the admin if found or null if not found.</returns>
        public async Task<AdminEntity?> GetMatchingAdmin(string email, string password)
        {
            var admin = await _databaseContext.Admins.Where(x => x.Email == email).SingleOrDefaultAsync();

            if (admin is not null && PasswordHelper.VerifyHashedPassword(admin.Password, password))
            {
                admin.Password = "";
                return admin;
            }

            return null;
        }

        #endregion
    }
}
