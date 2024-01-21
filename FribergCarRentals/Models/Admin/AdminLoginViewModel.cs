using System.ComponentModel;
using FribergCarRentals.Models.User;

namespace FribergCarRentals.Models.Admin
{
    public class AdminLoginViewModel : UserLoginViewModel
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public AdminLoginViewModel() : base()
        {

        }

        public AdminLoginViewModel(string email, string password) : base(email, password)
        {

        }

        #endregion
    }
}
