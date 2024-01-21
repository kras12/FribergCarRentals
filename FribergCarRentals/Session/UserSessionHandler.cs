using FribergCarRentals.DataAccess.EntityClasses;
using FribergCars.Shared.SharedClasses;

namespace FribergCarRentals.Session
{
    public static class UserSessionHandler
    {
        #region Methods

        public static UserSessionData GetUserData(ISession session)
        {
            return new UserSessionData(
                session.GetInt32(nameof(UserSessionData.UserId)) ?? throw new InvalidOperationException("The user ID was not found in the session variable."),
                session.GetString(nameof(UserSessionData.UserEmail)) ?? throw new InvalidOperationException("The user email was not found in the session variable."),
                UserRoleEntity.CreateFromUserRoleName(session.GetString(nameof(UserSessionData.UserRole))!) ?? throw new InvalidOperationException("The user role was not found in the session variable."));
        }

        public static void SetUserData(ISession session, UserSessionData data)
        {
            session.SetInt32(nameof(UserSessionData.UserId), data.UserId);
            session.SetString(nameof(UserSessionData.UserEmail), data.UserEmail);
            session.SetString(nameof(UserSessionData.UserRole), data.UserRole.UserRoleName);
        }

        public static void RemoveUserData(ISession session)
        {
            session.Remove(nameof(UserSessionData.UserId));
            session.Remove(nameof(UserSessionData.UserEmail));
            session.Remove(nameof(UserSessionData.UserRole));
        }

        private static bool IsUserLoggedIn(ISession session, UserRoleType userRole)
        {
            List<string> keys = new()
            {
                nameof(UserSessionData.UserId),
                nameof(UserSessionData.UserEmail),
                nameof(UserSessionData.UserRole)
            };

            bool result = false;

            if (session.Keys.Intersect(keys).Count() == keys.Count)
            {
                result = UserRoleEntity.CreateFromUserRoleName(
                session.GetString(nameof(UserSessionData.UserRole))!).UserRoleType == userRole;
            }

            return result;
        }

        public static bool IsCustomerLoggedIn(ISession session)
        {
            return IsUserLoggedIn(session, UserRoleType.Customer);
        }

        public static bool IsAdminLoggedIn(ISession session)
        {
            return IsUserLoggedIn(session, UserRoleType.Admin); 
        }

        #endregion
    }
}
