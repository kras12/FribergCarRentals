using FribergCarRentals.Data.SharedClasses;
using Microsoft.Extensions.Configuration;

namespace FribergCarRentals.Data
{
    public class ConnectionStringManager
    {
        #region Methods

        public string GetConnectionString(ConfigurationManager? configurationManager = null)
        {
            if (configurationManager is null)
            {
                configurationManager = WebApplication.CreateBuilder().Configuration;
            }

            return configurationManager.GetConnectionString(AppSettingsHelper.ApplicationDbContextConnectionStringName) ?? 
                throw new InvalidOperationException("Connection string 'FribergCarRentalsDev' was not found.");
        }

        #endregion
    }
}
