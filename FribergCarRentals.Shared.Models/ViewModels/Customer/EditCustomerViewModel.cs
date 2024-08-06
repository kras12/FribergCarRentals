using FribergCarRentals.Shared.Models.ViewModels.User;

namespace FribergCarRentals.Shared.Models.ViewModels.Customer
{
    /// <summary>
    /// A view model class that handles data related to editing a customer.
    /// </summary>
    public class EditCustomerViewModel : UserEditViewModel
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public EditCustomerViewModel() : this(0, "", "", "")
        {

        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="userId">The ID for the customer.</param>
        /// <param name="firstName">The first name for the customer.</param>
        /// <param name="lastName">The last name for the customer.</param>
        /// <param name="email">The email for the customer.</param>
        public EditCustomerViewModel(int userId, string firstName, string lastName, string email) :
            base(userId, firstName, lastName, email)
        {
            PageSubTitle = $"#{AccountId} - {FullName}";
        }

        #endregion
    }
}
