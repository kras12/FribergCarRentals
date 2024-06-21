using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using AppSettings.Shared.Settings;

namespace FribergCarRentals.DataAccess.DatabaseContexts
{
    /// <summary>
    /// A design time database context needed to support scaffolding when the database context class resides in a standalone project.
    /// </summary>
    public class DesignTimeApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        #region Methods

        /// <summary>
        /// Creates a new instance of a derived context.
        /// </summary>
        /// <param name="args">Arguments provided by the design-time service.</param>
        /// <returns>An instance of <typeparamref name="TContext" />.</returns>
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile(AppSettingsHelper.AppSettingsDevelopmentJsonFileName)
            .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString(AppSettingsHelper.ApplicationDbContextConnectionStringKey));

            return new ApplicationDbContext(optionsBuilder.Options);
        }

        #endregion
    }
}
