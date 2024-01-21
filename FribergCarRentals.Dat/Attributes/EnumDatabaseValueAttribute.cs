using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentals.DataAccess.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumDatabaseValueAttribute : Attribute
    {
        #region Constructors

        public EnumDatabaseValueAttribute(string databaseValue)
        {
            #region Checks

            if (string.IsNullOrEmpty(databaseValue))
            {
                throw new ArgumentException($"The value of parameter '{nameof(databaseValue)}' can't be null or empty.", nameof(databaseValue));
            }

            #endregion

            Value = databaseValue;
        }

        #endregion

        #region Properties

        public string Value { get; } = "";

        public string DescriptionValue { get; set; } = "";

        #endregion
    }
}
