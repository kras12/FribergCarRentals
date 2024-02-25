using FribergCarRentals.Data;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;
using FribergCarRentals.DataAccess.EntityClasses;

namespace FribergCarRentals.Data.ModelBinders
{
    /// <summary>
    /// A model binder class that converts a text string to an instance of class <see cref="UserRoleEntity"/>.
    /// </summary>
    public class UserRoleModelBinder : SingleValueModelBinderBase<UserRoleEntity>
    {
        #region Methods

        /// <summary>
        /// Attempts to convert a text string to an instance of class <see cref="UserRoleEntity"/>.
        /// </summary>
        /// <param name="value">The text string to convert.</param>
        /// <param name="result">The new class instance if the operation was successful.</param>
        /// <returns>True if the operation was successful.</returns>
        protected override bool TryCreateObjectFromString(string value, [NotNullWhen(true)] out UserRoleEntity? result)
        {
            return UserRoleEntity.TryCreateFromUserRoleName(value, out result);
        }

        #endregion
    }
}
