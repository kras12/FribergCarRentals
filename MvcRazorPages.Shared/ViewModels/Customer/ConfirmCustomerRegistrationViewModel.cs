namespace MvcRazorPages.Shared.ViewModels.Customer
{
    /// <summary>
    /// View model class for customer registration confirmation.
    /// </summary>
    public class ConfirmCustomerRegistrationViewModel
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="confirmEmailLink">An optional confirm email link for debug builds.</param>
        public ConfirmCustomerRegistrationViewModel(string? confirmEmailLink = null)
        {
            ConfirmEmailLink = confirmEmailLink;
        }

        #endregion

        #region Properties

        /// <summary>
        /// An optional confirm email link for debug builds.
        /// </summary>
        public string? ConfirmEmailLink { get; private set; }

        /// <summary>
        /// Returns true if there is an confirm email link.
        /// </summary>
        public bool HaveConfirmEmailLink
        {
            get
            {
                return !string.IsNullOrEmpty(ConfirmEmailLink);
            }
        }

        #endregion
    }
}
