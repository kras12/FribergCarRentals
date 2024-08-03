using FribergCarRentals.Shared.Dto.User;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcRazorPages.Shared.ViewModels.User
{
    /// <summary>
    /// A DTO class that handles data related to editing an user. 
    /// </summary>
    public abstract class EditUserDto : UserDto
    {
        #region Properties

        /// <summary>
        /// The password for the user.
        /// </summary>
        public string? NewPassword { get; set; }

        #endregion
    }
}
