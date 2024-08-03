using System.IdentityModel.Tokens.Jwt;

namespace FribergCarRentals.Shared.Constants
{
    /// <summary>
    /// A class that contains user claims for JWT tokens.
    /// </summary>
    public class ApplicationUserJwtClaims
    {
        #region Constants

        /// <summary>
        /// The user email claim. 
        /// </summary>
        public const string Email = JwtRegisteredClaimNames.Email;

        /// <summary>
        /// The user jti claim. 
        /// </summary>
        public const string Jti = "jti";

        /// <summary>
        /// The user ID claim.
        /// </summary>
        public const string UserId = JwtRegisteredClaimNames.Sub;

        /// <summary>
        /// The user name claim. 
        /// </summary>
        public const string UserName = JwtRegisteredClaimNames.PreferredUsername;

        /// <summary>
        /// The user role claim. 
        /// </summary>
        public const string UserRole = "user_role";

        #endregion
    }
}
