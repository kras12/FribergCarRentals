using System.ComponentModel;

namespace FribergCarRentals.Models.User
{
    public abstract class UserEditViewModel : UserViewModel
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        protected UserEditViewModel() : this(0, "", "", "")
        {

        }

        protected UserEditViewModel(int userId, string firstName, string lastName, string email) :
            base(userId, firstName, lastName, email)
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// The password for the user.
        /// </summary>
        [DisplayName("New Password")]
        public string Password { get; set; } = "";

        #endregion
    }
}
