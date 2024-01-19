using System.ComponentModel;

namespace FribergCarRentals.Models
{
    public abstract class UserFormViewModel : UserViewModel
    {

        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        protected UserFormViewModel() : base(0, "", "", "")
        {

        }

        protected UserFormViewModel(int userId, string firstName, string lastName, string email) : 
            base(userId, firstName, lastName, email)
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// The input password for the user.
        /// </summary>
        /// <remarks>Is used to input data to form fields and get data from form submissions.</remarks>
        [DisplayName("New Password")]
        public string InputPassword { get; set; } = "";

        #endregion
    }
}
