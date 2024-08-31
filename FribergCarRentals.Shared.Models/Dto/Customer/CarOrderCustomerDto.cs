using FribergCarRentals.Shared.Models.Dto.User;

namespace FribergCarRentals.Shared.Models.Dto.Customer
{
    /// <summary>
    /// An DTO class that contains customer data for an order. 
    /// </summary>
    public class CarOrderCustomerDto : UserDto
    {
        #region Properties

        /// <summary>
		/// The ID of the customer.
		/// </summary>
        public int CustomerId { get; set; }

        #endregion
    }
}
