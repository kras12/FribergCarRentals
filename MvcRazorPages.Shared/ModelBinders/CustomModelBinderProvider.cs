using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using FribergCarRentals.Data.EntityClasses;

namespace MvcRazorPages.Shared.ModelBinders
{
    /// <summary>
    /// A model binder provider class that provides model binders for the Friberg Car Rentals project. 
    /// </summary>
    public class CustomModelBinderProvider : IModelBinderProvider
    {
        #region Methods

        /// <summary>
        /// Creates a <see cref="IModelBinder"/> based on the provided <see cref="ModelBinderProviderContext"/>.
        /// </summary>
        /// <param name="context">The <see cref="ModelBinderProviderContext"/> to use.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>An <see cref="IModelBinder"/>.</returns>
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            #region Checks
            
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            #endregion

            if (context.Metadata.ModelType == typeof(VehiclePropulsionEntity))
            {
                return new BinderTypeModelBinder(typeof(VehiclePropulsionModelBinder));
            }
            else if (context.Metadata.ModelType == typeof(CarRentalStatusEntity))
            {
                return new BinderTypeModelBinder(typeof(CarRentalStatusModelBinder));
            }
            else if (context.Metadata.ModelType == typeof(List<ImageEntity>))
            {
                return new BinderTypeModelBinder(typeof(ImageListModelBinder));
            }

            return null;
        }

        #endregion
    }
}
