using FribergCarRentals.Data.EntityClasses;
using System.Diagnostics.CodeAnalysis;

namespace MvcRazorPages.Shared.ModelBinders
{
    /// <summary>
    /// A model binder class that converts a text string to an instance of class <see cref="CarRentalStatusEntity"/>.
    /// </summary>
    public class CarRentalStatusModelBinder : SingleValueModelBinderBase<CarRentalStatusEntity>
    {
        #region Methods

        /// <summary>
        /// Attempts to convert a text string to an instance of class <see cref="CarRentalStatusEntity"/>.
        /// </summary>
        /// <param name="value">The text string to convert.</param>
        /// <param name="result">The new class instance if the operation was successful.</param>
        /// <returns>True if the operation was successful.</returns>
        protected override bool TryCreateObjectFromString(string value, [NotNullWhen(true)] out CarRentalStatusEntity? result)
        {
            return CarRentalStatusEntity.TryCreateFromStatusName(value, out result);
        }

        #endregion
    }
}
