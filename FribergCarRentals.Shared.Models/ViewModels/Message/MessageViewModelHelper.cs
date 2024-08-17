namespace FribergCarRentals.Shared.Models.ViewModels.Message
{
    /// <summary>
    /// A class that cretes user messages in the form of <see cref="MessageViewModel"/> objects. 
    /// </summary>
    public static class MessageViewModelHelper
    {
        #region CarCategories

        /// <summary>
        /// Creates a car category creation success message.
        /// </summary>
        /// <param name="categoryId">The ID of the car category that was created.</param>
        /// <returns>A <see cref="MessageViewModel"/>.</returns>
        public static MessageViewModel CreateCarCategoryCreationSuccessMessage(int categoryId)
        {
            #region Checks

            if (categoryId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(categoryId), $"The value of parameter '{nameof(categoryId)}' can't be negative.");
            }

            #endregion

            return new MessageViewModel(MessageType.Success, $"Category #{categoryId} was created successfully", "Created");
        }

        /// <summary>
        /// Creates a car category deletion success message.
        /// </summary>
        /// <param name="categoryId">The ID of the car category that was deleted.</param>
        /// <returns>A <see cref="MessageViewModel"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static MessageViewModel CreateCarCategoryDeletionSuccessMessage(int categoryId)
        {
            #region Checks

            if (categoryId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(categoryId), $"The value of parameter '{nameof(categoryId)}' can't be negative.");
            }

            #endregion

            return new MessageViewModel(MessageType.Success, $"Category #{categoryId} was deleted successfully.", "Deleted");
        }

        /// <summary>
        /// Creates a car category update success message.
        /// </summary>
        /// <param name="categoryId">The ID of the car category that was updated.</param>
        /// <returns>A <see cref="MessageViewModel"/>.</returns>
        public static MessageViewModel CreateCarCategoryUpdateSuccessMessage(int categoryId)
        {
            return new MessageViewModel(MessageType.Success, $"Category #{categoryId} was updated successfully.", "Saved");
        }

        #endregion

        #region Cars

        /// <summary>
        /// Creates a car creation success message.
        /// </summary>
        /// <param name="carId">The ID of the car that was created.</param>
        /// <returns>A <see cref="MessageViewModel"/>.</returns>
        public static MessageViewModel CreateCarCreationSuccessMessage(int carId)
        {
            return new MessageViewModel(MessageType.Success, $"Car #{carId} was created successfully", "Created");
        }

        /// <summary>
        /// Creates a car deletion success message.
        /// </summary>
        /// <param name="carId">The ID of the car that was deleted.</param>
        /// <returns>A <see cref="MessageViewModel"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static MessageViewModel CreateCarDeletionSuccessMessage(int carId)
        {
            #region Checks

            if (carId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(carId), $"The value of parameter '{nameof(carId)}' can't be negative.");
            }

            #endregion

            return new MessageViewModel(MessageType.Success, $"Car #{carId} was deleted successfully.", "Deleted");
        }

        /// <summary>
        /// Creates a car update success message.
        /// </summary>
        /// <param name="categoryId">The ID of the car that was updated.</param>
        /// <returns>A <see cref="MessageViewModel"/>.</returns>
        public static MessageViewModel CreateCarUpdateSuccessMessage(int carId)
        {
            return new MessageViewModel(MessageType.Success, $"Car #{carId} was updated successfully.", "Saved");
        }

        #endregion

        #region Customers

        /// <summary>
        /// Creates a customer creation success message.
        /// </summary>
        /// <param name="customerId">The ID of the customer that was created.</param>
        /// <returns>A <see cref="MessageViewModel"/>.</returns>
        public static MessageViewModel CreateCustomerCreationSuccessMessage(int customerId)
        {
            return new MessageViewModel(MessageType.Success, $"Customer #{customerId} was created successfully.", "Created");
        }

        /// <summary>
        /// Creates a customer deletion success message.
        /// </summary>
        /// <param name="customerId">The ID of the customer that was deleted.</param>
        /// <returns>A <see cref="MessageViewModel"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static MessageViewModel CreateCustomerDeletionSuccessMessage(int customerId)
        {
            #region Checks

            if (customerId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(customerId), $"The value of parameter '{nameof(customerId)}' can't be negative.");
            }

            #endregion

            return new MessageViewModel(MessageType.Success, $"Customer #{customerId} was deleted successfully.", "Deleted");
        }

        /// <summary>
        /// Creates a customer update success message.
        /// </summary>
        /// <param name="customerId">The ID of the customer that was updated.</param>
        /// <returns>A <see cref="MessageViewModel"/>.</returns>
        public static MessageViewModel CreateCustomerUpdateSuccessMessage(int customerId)
        {
            return new MessageViewModel(MessageType.Success, $"Customer #{customerId} was updated successfully.", "Saved");
        }

        /// <summary>
        /// Creates a resent confirm email link to customer success message.
        /// </summary>
        /// <param name="customerId">The ID of the customer that was created.</param>
        /// <returns>A <see cref="MessageViewModel"/>.</returns>
        public static MessageViewModel CreateResentConfirmEmailLinkToCustomerMessage(int customerId)
        {
            return new MessageViewModel(MessageType.Success, $"An email should have been sent to customer #{customerId}. But since this app does not currently have a real email sender registered, the email address was automatically confirmed. ", "Email Sent");
        }

        #endregion

        #region Orders

        /// <summary>
        /// Creates an order cancellation success message.
        /// </summary>
        /// <param name="orderId">The ID of the order that was canceled.</param>
        /// <returns>A <see cref="MessageViewModel"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static MessageViewModel CreateOrderCancellationSuccessMessage(int orderId)
        {
            #region Checks

            if (orderId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(orderId), $"The value of parameter '{nameof(orderId)}' can't be negative.");
            }

            #endregion

            return new MessageViewModel(MessageType.Success, $"Order #{orderId} was cancelled successfully.", "Cancelled");
        }

        /// <summary>
        /// Creates an order completion success message.
        /// </summary>
        /// <param name="orderId">The ID of the order that was completed.</param>
        /// <returns>A <see cref="MessageViewModel"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static MessageViewModel CreateOrderCompletionSuccessMessage(int orderId)
        {
            #region Checks

            if (orderId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(orderId), $"The value of parameter '{nameof(orderId)}' can't be negative.");
            }

            #endregion

            return new MessageViewModel(MessageType.Success, $"Order #{orderId} was completed successfully.", "Completed");
        }

        /// <summary>
        /// Creates an order deletion success message.
        /// </summary>
        /// <param name="orderId">The ID of the order that was deleted.</param>
        /// <returns>A <see cref="MessageViewModel"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static MessageViewModel CreateOrderDeletionSuccessMessage(int orderId)
        {
            #region Checks

            if (orderId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(orderId), $"The value of parameter '{nameof(orderId)}' can't be negative.");
            }

            #endregion

            return new MessageViewModel(MessageType.Success, $"Order #{orderId} was deleted successfully.", "Deleted");
        }

        #endregion
    }
}
