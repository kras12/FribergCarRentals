using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FribergCarRentals.Data.User;
using FribergCarRentals.DataAccess.EntityClasses;

namespace FribergCarRentals.Data.Admin
{
    public class AdminViewModel : UserViewModel
    {
        #region Constructors

        /// <summary>
        ///  A constructor.
        /// </summary>
        /// <param name="admin">The admin to copy data from.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public AdminViewModel(AdminEntity admin) :
            base(admin.UserId, admin.FirstName, admin.LastName, admin.Email)
        {

        }

        #endregion
    }
}
