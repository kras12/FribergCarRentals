namespace FribergCarRentals.Shared.Constants
{
    /// <summary>
    /// Contains validation rules for data sent through forms and DTOs.
    /// </summary>
    public class ValidationRules
    {
        #region RegexPatterns

        /// <summary>
        /// Regular expression pattern for email addresses. 
        /// </summary>
        /// <remarks>
        /// <para>Letters, numeric characters, dots, underscores and dashes are allowed before the @ character.</para>
        /// <para>Letters, numeric characters, dots, and dashes are allowed after the @ character and before the last dot.</para>
        /// <para>Only letters are allowed after the last dot.</para>
        /// </remarks>
        public const string EmailRegexPattern = @"^[\p{L}\p{N}\._\-]+\@[\p{L}\p{N}\.\-]+\.\p{L}+$";

        /// <summary>
        /// Regular expression pattern to only allow letters, numbers and spaces. 
        /// </summary>
        public const string LettersAndSpacesRegexPattern = @"^[\p{L} ]+$";

        /// <summary>
        /// Regular expression pattern to only allow letters, numbers and spaces. 
        /// </summary>
        public const string LettersNumbersAndSpacesRegexPattern = @"^[\p{L}\p{N} ]+$";

        /// <summary>
        /// Regular expression pattern for registration numbers (formats abc123 and acb12d). 
        /// </summary>
        public const string RegistrationNumberRegexPattern = @"^[a-zA-Z]{3}[0-9]{2}[a-zA-Z0-9]{1}$";

        #endregion

        #region OtherConstants

        /// <summary>
        /// A format string for showing date only (format yyyy-MM-dd).
        /// </summary>
        public const string DateFormatString = "{0:yyyy-MM-dd}";

        /// <summary>
        /// The default format string for displaying dates. 
        /// </summary>
        public const string DefaultDateFormatString = "{0:g}";

        /// <summary>
        /// The default float number format string with 2 decimals.
        /// </summary>
        public const string DefaultFloatNumberInputFormatString = "{0:0.00}";

        /// <summary>
        /// The default integer number format string with thousands separator.
        /// </summary>
        public const string DefaultIntegerNumberOutputFormatString = "{0:N0}";

        /// <summary>
        /// The default value for max number of characters allowed as input.
        /// </summary>
        public const int DefaultMaxCharacterInput = 50;

        /// <summary>
        /// The default format string for displaying price with 2 decimals, thousands separator, and currency.
        /// </summary>
        public const string DefaultPriceOutputFormatString = "{0:N2} kr";

        /// <summary>
        /// The maximum allowed password length.
        /// </summary>
        public const int MaxPasswordLength = 50;

        /// <summary>
        /// The minimum allowed password length.
        /// </summary>
        public const int MinPasswordLength = 6;

        #endregion
    }
}
