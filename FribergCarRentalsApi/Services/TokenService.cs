using FribergFastigheter.Shared.Constants;
using FribergFastigheter.Server.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FribergCarRentals.Data.EntityClasses;
using FribergCarRentals.Shared.Constants;
using Microsoft.IdentityModel.JsonWebTokens;

namespace FribergCarRentalsApi.Services
{
    /// <summary>
    /// Creates JWT tokens for user classes.
    /// </summary>
    public class TokenService : ITokenService
    {
        #region Constants

        /// <summary>
        /// The duration of the token in days.
        /// </summary>
        public const int TokenDurationInDays = 3;

        #endregion

        #region Fields

        /// <summary>
        /// The injected configuration.
        /// </summary>
        private readonly IConfiguration _config;

        /// <summary>
        /// Represents a symmetric security key.
        /// </summary>
        private readonly SymmetricSecurityKey _key;

        /// <summary>
        /// The injected user manager.
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="config">The injected configuration manager.</param>
        /// <param name="userManager">The injected user manager.</param>
        public TokenService(IConfiguration config, UserManager<ApplicationUser> userManager)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]!));
            _userManager = userManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a JWT token for an admin.
        /// </summary>
        /// <param name="admin">The admin to create the token for.</param>
        /// <returns>The created token as a <see cref="string"/>.</returns>
        public async Task<string> CreateToken(AdminEntity admin)
        {
            return CreateToken(await GetClaimsAync(admin.User));
        }

        /// <summary>
        /// Creates a JWT token for a customer.
        /// </summary>
        /// <param name="customer">The customer to create the token for.</param>
        /// <returns>The created token as a <see cref="string"/>.</returns>
        public async Task<string> CreateToken(CustomerEntity customer)
        {            
            return CreateToken(await GetClaimsAync(customer.User));
        }

        /// <summary>
        /// Creates a token from a collection of claims.
        /// </summary>
        /// <param name="claims">The claims to create token for.</param>
        /// <returns>A token in the form of a <see cref="string"/>.</returns>
        private string CreateToken(IEnumerable<Claim> claims)
        {
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(TokenDurationInDays),
                SigningCredentials = creds,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]
            };

            var tokenHandler = new JsonWebTokenHandler();
            tokenHandler.SetDefaultTimesOnTokenCreation = false;

            return tokenHandler.CreateToken(tokenDescriptor);
        }

        /// <summary>
        /// Returns the claims for an <see cref="ApplicationUser"/>.
        /// </summary>
        /// <param name="user">The user to return the claims for.</param>
        /// <returns>A collection of created claims.</returns>
        private async Task<List<Claim>> GetClaimsAync(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ApplicationUserJwtClaims.UserName, user.UserName!),
                new Claim(ApplicationUserJwtClaims.Email, user.Email!),
                new Claim(ApplicationUserJwtClaims.Jti, Guid.NewGuid().ToString()),
                new Claim(ApplicationUserJwtClaims.UserId, user.Id.ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);
            claims.Add(new Claim(ApplicationUserClaims.UserRole, roles.Single()));

            return claims;
        }

        #endregion
    }
}
