using FribergCarRentals.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;

namespace FribergCarRentals.Data.ModelBinder
{
    public class VehiclePropulsionModelBinder : SingleValueModelBinderBase<VehiclePropulsionEntity>
    {
        protected override bool TryCreateObjectFromString(string value, [NotNullWhen(true)] out VehiclePropulsionEntity? result)
        {
            return VehiclePropulsionEntity.TryCreateFromPropulsionName(value, out result);
        }
    }
}
