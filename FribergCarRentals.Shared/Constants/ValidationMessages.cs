namespace FribergCarRentals.Shared.Constants
{
    /// <summary>
    /// Contains validation messages for data being sent through forms and DTOs. 
    /// </summary>
    public class ValidationMessages
    {
        #region Messages

        /// <summary>
        /// The message to show for invalid email addresses. 
        /// </summary>
        public const string EmailInputValidationMessage = "The text you entered is not a valid email.";

        /// <summary>
        /// The message to show when the input text is too long. 
        /// </summary>
        public const string InputTooLongValidationMessage = "The text you entered is longer than 50 characters.";

        /// <summary>
        /// The message to show when an input field has not been filled out. 
        /// </summary>
        public const string MandatoryFieldValidationMessage = "This field is mandatory.";

        /// <summary>
        /// A message to inform the user that only letters and spaces are allowed as input. 
        /// </summary>
        public const string OnlyLettersAndSpacesValidationMessage = "Only letters and spaces are allowed as input.";

        /// <summary>
        /// A message to inform the user that only letters, numbers and spaces are allowed as input. 
        /// </summary>
        public const string OnlyLettersNumbersAndSpacesValidationMessage = "Only letters, numbers and spaces are allowed as input.";

        /// <summary>
        /// The validation message to show when the length of the password doesn't fit the character count interval. 
        /// </summary>
        public const string PasswordLengthValidationMessage = "The password must be between 6 and 50 characters long.";

        /// <summary>
        /// Error message for when the pickup date is not in the future. 
        /// </summary>
        public const string PickupDateMustBeInFutureErrorMessage = "The pickup date must be at least one day into the future.";

        /// <summary>
        /// A message to inform the user about the valid input format for a registration number.
        /// </summary>
        public const string RegistrationNumberValidationMessage = "Registration numbers must be entered in the format abc123 or acb12d.";

        /// <summary>
        /// Error message for when the return date occurrs before the pickup date.
        /// </summary>
        public const string ReturnDateOccursBeforePickupDateErrorMessage = "The return date can't occur before the pickup date.";

        #endregion
    }
}
