using Microsoft.Extensions.Configuration;

namespace FribergCarRentals.Data
{
    public class ConnectionStringManager
    {
        #region Methods

        public string GetConnectionString()
        {
            return new ConfigurationManager().GetConnectionString("FribergCarRentalsDev") ?? 
                throw new InvalidOperationException("Connection string 'FribergCarRentalsDev' was not found.");
        }

        #endregion
    }
}
