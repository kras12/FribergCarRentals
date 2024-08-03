using Microsoft.AspNetCore.Mvc.ModelBinding;
using MvcRazorPages.Shared.ViewModels.Message;

namespace MvcRazorPages.Shared.ViewModels.Other
{
    /// <summary>
    /// A base class for view models.
    /// </summary>
    public abstract class ViewModelBase
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="pageTitle">An optional title for the page.</param>
        /// <param name="pageSubTitle">An optional page sub title for the page.</param>
        /// <exception cref="ArgumentException"></exception>
        protected ViewModelBase(string? pageTitle = null, string? pageSubTitle = null)
        {
            PageTitle = pageTitle;
            PageSubTitle = pageSubTitle;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns true if there are messages that can be shown to the user.
        /// </summary>
        public bool HaveMessages
        {
            get
            {
                return Messages.Count > 0;
            }
        }

        /// <summary>
        /// Returns true if there is a page sub title. 
        /// </summary>
        public bool HavePageSubTitle
        {
            get
            {
                return !string.IsNullOrEmpty(PageSubTitle);
            }
        }

        /// <summary>
        /// Returns true if there is a page title. 
        /// </summary>
        public bool HavePageTitle
        {
            get
            {
                return !string.IsNullOrEmpty(PageTitle);
            }
        }

        /// <summary>
        /// Messages that can be shown to the user.
        /// </summary>
        public List<MessageViewModel> Messages { get; } = new();

        /// <summary>
        /// An optional page sub title for the page. 
        /// </summary>
        [BindNever]
        public string? PageSubTitle { get; set; } = null;

        /// <summary>
        /// The title for the page. 
        /// </summary>
        [BindNever]
        public string? PageTitle { get; set; } = null;

        #endregion

        #region Methods

        /// <summary>
        /// Creates a message that can be shown to the user. 
        /// </summary>
        // <param name="type">The type of the message.</param>
        /// <param name="body">The body of the message.</param>
        /// <param name="title">The optional title of the message. </param>
        /// <exception cref="ArgumentException"></exception>
        public void CreateMessage(MessageType type, string body, string? title = null)
        {
            #region Checks

            if (string.IsNullOrEmpty(body))
            {
                throw new ArgumentException($"The value of parameter '{nameof(body)}' can't be empty.", nameof(body));
            }

            #endregion

            Messages.Add(new MessageViewModel(type, body, title));
        }

        #endregion
    }
}
