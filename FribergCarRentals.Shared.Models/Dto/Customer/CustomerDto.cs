using FribergCarRentals.Shared.Models.Dto.User;

namespace FribergCarRentals.Shared.Models.Dto.Customer
{
    /// <summary>
    /// An DTO class for a customer.
    /// </summary>
    public class CustomerDto : UserDto
    {
        #region Properties

        /// <summary>
		/// The ID of the customer.
		/// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// The number of orders the customer have.
        /// </summary>
        public int OrderCount { get; set; }

        #endregion
    }
}
