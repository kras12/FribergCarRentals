using FribergCarRentals.Shared.Dto.User;

namespace FribergCarRentals.Shared.Dto.Admin
{
    /// <summary>
    /// An DTO class for a customer.
    /// </summary>
    public class AdminDto
    {
        #region Properties

        /// <summary>
		/// The ID of the admin.
		/// </summary>
        public int AdminId { get; set; }

        /// <summary>
		/// The user associated with the admin.
		/// </summary>
        public required ApplicationUserDto User { get; set; }

        #endregion
    }
}
