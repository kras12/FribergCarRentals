using MvcRazorPages.Shared.Attributes;
using FribergCarRentals.Data.EntityClasses;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MvcRazorPages.Shared.ViewModels.Other;
using FribergCarRentals.Shared.Constants;

namespace MvcRazorPages.Shared.ViewModels.User
{
    /// <summary>
    /// A viewmodel base class that handles data related to users. 
    /// </summary>
    /// <remarks>This class acts like a base class for view models of all types as it supports model binding on its properties.</remarks>
    public abstract class UserViewModelBase : ViewModelBase
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        protected UserViewModelBase()
        {
            
        }

        /// <summary>
        ///  A constructor.
        /// </summary>
        /// <param name="firstName">The first name for the user.</param>
        /// <param name="lastName">The last name for the user.</param>
        /// <param name="email">The email address for the user.</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected UserViewModelBase(string firstName, string lastName, string email)
        {
            #region Checks

            if (firstName is null)
            {
                throw new ArgumentNullException(nameof(firstName), $"The value of parameter '{firstName}' can't be null");
            }

            if (lastName is null)
            {
                throw new ArgumentNullException(nameof(lastName), $"The value of parameter '{lastName}' can't be null");
            }

            if (email is null)
            {
                throw new ArgumentNullException(nameof(email), $"The value of parameter '{email}' can't be null");
            }

            #endregion

            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="user">The admin to model.</param>
        protected UserViewModelBase (AdminEntity admin) : this(admin.User.FirstName, admin.User.LastName, admin.User.Email!)
        {

        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="user">The customer to model.</param>
        protected UserViewModelBase(CustomerEntity customer) : this(customer.User.FirstName, customer.User.LastName, customer.User.Email!)
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// The email address for the user.
        /// </summary>
        [DisplayName("Email")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(maximumLength: ValidationRules.DefaultMaxCharacterInput, ErrorMessage = ValidationMessages.InputTooLongValidationMessage)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [ServerSideRegularExpression(ValidationRules.EmailRegexPattern, ErrorMessage = ValidationMessages.EmailInputValidationMessage)]
        public virtual string Email { get; set; } = "";

        /// <summary>
        /// The first name for the user.
        /// </summary>
        [DisplayName("First Name")]
        [Required]
        [StringLength(maximumLength: ValidationRules.DefaultMaxCharacterInput, ErrorMessage = ValidationMessages.InputTooLongValidationMessage)]
        [ServerSideRegularExpression(ValidationRules.LettersAndSpacesRegexPattern, ErrorMessage = ValidationMessages.OnlyLettersAndSpacesValidationMessage)]
        public virtual string FirstName { get; set; } = "";

        /// <summary>
        /// The full name for the user.
        /// </summary>
        [BindNever]
        [DisplayName("Full Name")]
        public virtual string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        /// <summary>
        /// The last name for the user.
        /// </summary>
        [DisplayName("Last Name")]
        [Required]
        [StringLength(maximumLength: ValidationRules.DefaultMaxCharacterInput, ErrorMessage = ValidationMessages.InputTooLongValidationMessage)]
        [ServerSideRegularExpression(ValidationRules.LettersAndSpacesRegexPattern, ErrorMessage = ValidationMessages.OnlyLettersAndSpacesValidationMessage)]
        public virtual string LastName { get; set; } = "";

        #endregion
    }
}
