using FribergCarRentals.Shared.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Shared.Models.ViewModels.User
{
    /// <summary>
    /// A view model class that handles data realting to the creation of users. 
    /// </summary>
    public abstract class CreateUserViewModel : UserViewModelBase
    {
        #region Properties

        /// <summary>
        /// The password for the user.
        /// </summary>
        [DisplayName("Password")]
        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [StringLength(maximumLength: ValidationRules.PasswordLengthMaximum, MinimumLength = ValidationRules.PasswordLengthMinium, ErrorMessage = ValidationMessages.PasswordLengthValidationMessage)]
        public string Password { get; set; } = "";

        /// <summary>
        /// The username for the user.
        /// </summary>
        [DisplayName("Username")]
        public string UserName
        {
            get
            {
                return Email;
            }
        }

        #endregion
    }
}
