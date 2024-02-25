using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FribergCarRentals.Models.Other
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
        [BindNever]
        public string? RequestId { get; set; }

        /// <summary>
        /// Returns true if there is a request ID that can be shown. 
        /// </summary>
        [BindNever]
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        #endregion
    }
}
