using FribergCarRentals.Shared.Models.ViewModels.Message;
using Microsoft.AspNetCore.Components;

namespace FribergCarRentalsBlazor.Components
{
    /// <summary>
    /// A component that shows messages to the user. 
    /// </summary>
    public partial class UserMessages : ComponentBase
    {
        #region Properties

        /// <summary>
        /// The user messages to show.
        /// </summary>
        [Parameter]
        public List<MessageViewModel> Messages { get; set; } = new();

        #endregion

        #region Methods

        /// <summary>
        /// Creates the HTML class for the type of the message. 
        /// </summary>
        /// <param name="type">The type of the message.</param>
        /// <returns>A <see cref="string"/> containing the message class.</returns>
        private string GetMessageClass(MessageType type)
        {
            string result = "";

            switch (type)
            {
                case MessageType.Neutral:
                    result = "message-type__neutral";
                    break;

                case MessageType.Error:
                    result = "message-type__error";
                    break;

                case MessageType.Success:
                    result = "message-type__success";
                    break;

                case MessageType.Warning:
                    result = "message-type__warning";
                    break;

                default:
                    result = "";
                    break;
            }

            return result;
        }

        #endregion
    }
}
