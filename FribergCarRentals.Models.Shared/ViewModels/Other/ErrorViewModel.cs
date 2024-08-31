namespace FribergCarRentals.Shared.ViewModels.Other
{
    /// <summary>
    /// A view model used for presenting error information.
    /// </summary>
    public class ErrorViewModel : ViewModelBase
    {
        #region Properties        

        /// <summary>
        /// The request ID.
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Returns true if there is a request ID that can be shown. 
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        #endregion
    }
}
