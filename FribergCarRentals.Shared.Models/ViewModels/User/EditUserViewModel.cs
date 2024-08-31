using FribergCarRentals.Shared.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Shared.Models.ViewModels.User
{
    /// <summary>
    /// A view model class that handles data related to editing an user. 
    /// </summary>
    public abstract class EditUserViewModel : UserViewModelBase
    {
        #region Properties

        // TODO - Give better name?
        /// <summary>
        /// The ID for the admin/customer account.
        /// </summary>
        [DisplayName("ID")]
        [Range(1, int.MaxValue, ErrorMessage = "The value must be a positive number larger than 1.")]
        public int AccountId { get; set; }

        /// <summary>
        /// The password for the user.
        /// </summary>
        [DisplayName("New Password")]
        [DataType(DataType.Password)]
        [StringLength(maximumLength: ValidationRules.PasswordLengthMaximum, MinimumLength = ValidationRules.PasswordLengthMinium, ErrorMessage = ValidationMessages.PasswordLengthValidationMessage)]
        public string? Password { get; set; }

        #endregion
    }
}
