using System.Security.Claims;

namespace FribergFastigheter.Shared.Constants
{
    /// <summary>
    /// A class that contains user claims.
    /// </summary>
    public class ApplicationUserClaims
    {
        #region Constants

        public const string CustomerId = "customerid";

        /// <summary>
        /// The user email claim. 
        /// </summary>
        public const string Email = ClaimTypes.Email;

        /// <summary>
        /// The user ID claim.
        /// </summary>
        public const string UserId = ClaimTypes.NameIdentifier;

        /// <summary>
        /// The user name claim. 
        /// </summary>
        public const string UserName = ClaimTypes.Name;

        /// <summary>
        /// The user role claim. 
        /// </summary>
        public const string UserRole = ClaimTypes.Role;

        #endregion
    }
}
