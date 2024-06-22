using FribergCarRentals.Data.EntityClasses;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;

namespace MvcRazorPages.Shared.ModelBinders
{
    /// <summary>
    /// A model binder class that converts a text string to an instance of class <see cref="VehiclePropulsionEntity"/>.
    /// </summary>
    public class VehiclePropulsionModelBinder : SingleValueModelBinderBase<VehiclePropulsionEntity>
    {
        #region Methods

        /// <summary>
        /// Attempts to convert a text string to an instance of class <see cref="VehiclePropulsionEntity"/>.
        /// </summary>
        /// <param name="value">The text string to convert.</param>
        /// <param name="result">The new class instance if the operation was successful.</param>
        /// <returns>True if the operation was successful.</returns>
        protected override bool TryCreateObjectFromString(string value, [NotNullWhen(true)] out VehiclePropulsionEntity? result)
        {
            return VehiclePropulsionEntity.TryCreateFromPropulsionName(value, out result);
        }

        #endregion
    }
}
