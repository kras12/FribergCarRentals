using Microsoft.EntityFrameworkCore;

namespace FribergCarRentals.Data
{
    /// <summary>
    /// The database context for the application.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="options">The options to use.</param>
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        #endregion
    }
}