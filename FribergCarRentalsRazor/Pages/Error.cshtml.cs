using MvcRazorPages.Shared.ViewModels.Other;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace FribergCarRentals.Pages
{
    /// <summary>
    /// A page model for an error page.
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public ErrorModel()
        {

        }

        #endregion

        #region Properties

        public ErrorViewModel ErrorViewModel { get; private set; } = new ErrorViewModel();

        #endregion

        #region Methods

        /// <summary>
        /// Handler for GET requests.
        /// </summary>
        public void OnGet()
        {
            ErrorViewModel.RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }

        #endregion
    }
}
