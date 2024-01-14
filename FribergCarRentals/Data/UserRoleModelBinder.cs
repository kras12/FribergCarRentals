using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace FribergCarRentals.Data
{
    public class UserRoleModelBinder : SingleValueModelBinderBase<UserRoleEntity>
    {
        protected override bool TryCreateObjectFromString(string value, [NotNullWhen(true)] out UserRoleEntity? result)
        {
            return UserRoleEntity.TryCreateFromUserRoleName(value, out result);
        }
    }
}
