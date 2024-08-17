using FribergCarRentals.Shared.Models.Dto.Order;
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
        /// A collection of orders for the customer. 
        /// </summary>
        public List<CarOrderDto> Orders { get; } = new();

        #endregion
    }
}
