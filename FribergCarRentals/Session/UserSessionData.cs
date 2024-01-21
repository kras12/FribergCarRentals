using FribergCarRentals.Data.EntityClasses;

namespace FribergCarRentals.Session
{
    public class UserSessionData
    {
        #region Constructors

        public UserSessionData(int userId, string email, UserRoleEntity role)
        {
            UserId = userId;
            UserEmail = email;
            UserRole = role;
        }

        #endregion

        #region Properties

        public int UserId { get; }

        public string UserEmail { get; } = "";

        public UserRoleEntity UserRole { get; }

        #endregion
    }
}
