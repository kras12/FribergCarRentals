namespace FribergCarRentals.Shared.Attributes
{
    /// <summary>
    /// A custom attribute class designed to specify values for insertion into value and description database fields. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumDatabaseValueAttribute : Attribute
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="databaseValue">The value intended for the value database field.</param>
        /// <exception cref="ArgumentException"></exception>
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

        /// <summary>
        /// The value intended for the description database field.
        /// </summary>
        public string DescriptionValue { get; set; } = "";

        /// <summary>
        /// The value intended for the value database field.
        /// </summary>
        public string Value { get; } = "";

        #endregion
    }
}
