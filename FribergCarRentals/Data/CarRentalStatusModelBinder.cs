using FribergCarRentals.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics.CodeAnalysis;

namespace FribergCarRentals.Data
{
    public class CarRentalStatusModelBinder : SingleValueModelBinderBase<CarRentalStatusEntity>
    {
        protected override bool TryCreateObjectFromString(string value, [NotNullWhen(true)] out CarRentalStatusEntity? entity)
        {
            return CarRentalStatusEntity.TryCreateFromStatusName(value, out entity);
        }
    }
}
