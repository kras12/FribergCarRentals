using FribergCarRentals.Shared.Dto.Order;
using FribergCarRentals.Shared.Dto.User;

namespace FribergCarRentals.Shared.Dto.Customer
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
