using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace FribergCarRentals.Data.ModelBinder
{
    public class CustomModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(VehiclePropulsionEntity))
            {
                return new BinderTypeModelBinder(typeof(VehiclePropulsionModelBinder));
            }
            else if (context.Metadata.ModelType == typeof(CarRentalStatusEntity))
            {
                return new BinderTypeModelBinder(typeof(CarRentalStatusModelBinder));
            }
            else if (context.Metadata.ModelType == typeof(UserRoleEntity))
            {
                return new BinderTypeModelBinder(typeof(UserRoleModelBinder));
            }
            else if (context.Metadata.ModelType == typeof(List<ImageEntity>))
            {
                return new BinderTypeModelBinder(typeof(ImageListModelBinder));
            }

            return null;
        }
    }
}
