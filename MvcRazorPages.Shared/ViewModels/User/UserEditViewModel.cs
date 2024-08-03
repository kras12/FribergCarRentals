using FribergCarRentals.Shared.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcRazorPages.Shared.ViewModels.User
{
    /// <summary>
    /// A view model class that handles data related to editing an user. 
    /// </summary>
    public abstract class UserEditViewModel : UserViewModelBase
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="firstName">The first name of the user.</param>
        /// <param name="lastName">The last name of the user.</param>
        /// <param name="email">The email of the user.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        protected UserEditViewModel(int userId, string firstName, string lastName, string email) :
            base(firstName, lastName, email)
        {
            #region Checks

            if (userId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId), $"The value of parameter '{userId}' can't be negative.");
            }

            #endregion

            AccountId = userId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The password for the user.
        /// </summary>
        [DisplayName("New Password")]
        [DataType(DataType.Password)]
        [StringLength(maximumLength: ValidationRules.PasswordLengthMaximum, MinimumLength = ValidationRules.PasswordLengthMinium, ErrorMessage = ValidationMessages.PasswordLengthValidationMessage)]
        public string? NewPassword { get; set; }

        // TODO - Give better name?
        /// <summary>
        /// The ID for the admin/customer account.
        /// </summary>
        [DisplayName("ID")]
        [Range(1, int.MaxValue, ErrorMessage = "The value must be a positive number larger than 1.")]
        public int AccountId { get; set; }

        #endregion
    }
}
