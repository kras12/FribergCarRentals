namespace FribergCarRentals.Models
{
    public class CustomerFormViewModel : UserFormViewModel
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public CustomerFormViewModel() : base(0, "", "", "")
        {

        }

        public CustomerFormViewModel(CustomerEntity customer) :
            base(customer.UserId, customer.FirstName, customer.LastName, customer.Email)
        {

        }

        #endregion
    }
}
