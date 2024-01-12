using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCars.Shared.SharedClasses
{
    public static class AppSettingsHelper
    {
        #region Properties

        public static string ApplicationDbContextConnectionStringName => "ApplicationDbContext";

        public static string AppSettingsDevelopmentJsonFileName => "appsettings.Development.json";

        #endregion
    }
}
