using System.Text.Json.Serialization;

namespace FribergCarRentals.Shared.Models.Dto.User
{
    /// <summary>
    /// A DTO class for email confirmation.
    /// </summary>
    public class ConfirmEmailDto
    {
        #region Constructors

        /// <summary>
        /// A constructor intended for JSON serializers.
        /// </summary>
        [JsonConstructor]
        private ConfirmEmailDto()
        {
            
        }

        /// <summary>
        /// A constructor. 
        /// </summary>
        /// <param name="code">The confirmation code sent to the client.</param>
        /// <param name="email">The email of the user.</param>
        public ConfirmEmailDto(string code, string email)
        {
            Code = code;
            Email = email;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The confirmation code sent to the client.
        /// </summary>
        public string Code { get; set; } = "";

        /// <summary>
        /// The email of the user.
        /// </summary>
        public string Email { get; set; } = "";

        #endregion
    }
}
