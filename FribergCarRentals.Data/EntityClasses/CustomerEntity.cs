using FribergFastigheter.Server.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Data.EntityClasses
{
    /// <summary>
    /// An entity class that represents a customer.
    /// </summary>
    public class CustomerEntity
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public CustomerEntity()
        {

        }

        /// <summary>
        ///  A constructor.
        /// </summary>
        /// <param name="user"><summary>
		/// The user associated with the admin.</param>
        public CustomerEntity(ApplicationUser user)
        {
            User = user;
        }

        #endregion

        #region Properties

        /// <summary>
		/// The ID of the customer.
		/// </summary>
		[Key]
        public int CustomerId { get; set; }

        /// <summary>
        /// A collection of orders for the customer. 
        /// </summary>
        public List<CarOrderEntity> Orders { get; } = new();

        /// <summary>
		/// The user associated with the admin.
		/// </summary>
		[Required]
        public ApplicationUser User { get; set; }

        #endregion
    }
}
