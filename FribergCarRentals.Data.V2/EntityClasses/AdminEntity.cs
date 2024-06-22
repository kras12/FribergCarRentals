using FribergFastigheter.Server.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.DataAccess.EntityClasses
{
    /// <summary>
    /// An entity class that represents an admin. 
    /// </summary>
    public class AdminEntity
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public AdminEntity()
        {

        }

        /// <summary>
        ///  A constructor.
        /// </summary>
        /// <param name="user"><summary>
		/// The user associated with the admin.</param>
        public AdminEntity(ApplicationUser user)
        {
            User = user;
        }

        #endregion

        #region Properties

        /// <summary>
		/// The ID of the admin.
		/// </summary>
		[Key]
        public int AdminId { get; set; }

        /// <summary>
		/// The user associated with the admin.
		/// </summary>
		[Required]
        public ApplicationUser User { get; set; }

        #endregion
    }
}