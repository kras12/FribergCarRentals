using FribergCarRentals.Data;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;
using FribergCarRentals.DataAccess.EntityClasses;

namespace FribergCarRentals.Data.ModelBinder
{
    public class UserRoleModelBinder : SingleValueModelBinderBase<UserRoleEntity>
    {
        protected override bool TryCreateObjectFromString(string value, [NotNullWhen(true)] out UserRoleEntity? result)
        {
            return UserRoleEntity.TryCreateFromUserRoleName(value, out result);
        }
    }
}
