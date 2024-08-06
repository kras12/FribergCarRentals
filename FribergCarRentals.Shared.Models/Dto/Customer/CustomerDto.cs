using FribergCarRentals.Shared.Models.Dto.Order;
using FribergCarRentals.Shared.Models.Dto.User;

namespace FribergCarRentals.Shared.Models.Dto.Customer
{
    /// <summary>
    /// An DTO class for a customer.
    /// </summary>
    public class CustomerDto
    {
        #region Properties

        /// <summary>
		/// The ID of the customer.
		/// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// A collection of orders for the customer. 
        /// </summary>
        public List<CarOrderDto> Orders { get; } = new();

        /// <summary>
		/// The user associated with the customer.
		/// </summary>
        public required UserDto User { get; set; }

        #endregion
    }
}
