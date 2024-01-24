using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using FribergCarRentals.Data.SharedClasses;

namespace FribergCarRentals.DataAccess.DatabaseContexts
{
    /// <summary>
    /// A design time database context needed to support scaffolding when the database context class resides in a standalone project.
    /// </summary>
    public class DesignTimeApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
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
    }
}
