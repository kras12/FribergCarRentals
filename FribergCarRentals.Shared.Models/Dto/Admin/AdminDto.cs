using FribergCarRentals.Shared.Models.Dto.User;

namespace FribergCarRentals.Shared.Models.Dto.Admin
{
    /// <summary>
    /// An DTO class for a customer.
    /// </summary>
    public class AdminDto : UserDto
    {
        #region Properties

        /// <summary>
		/// The ID of the admin.
		/// </summary>
        public int AdminId { get; set; }

        #endregion
    }
}
