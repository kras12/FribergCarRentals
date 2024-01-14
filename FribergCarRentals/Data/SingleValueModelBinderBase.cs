using FribergCarRentals.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics.CodeAnalysis;

namespace FribergCarRentals.Data
{
    public abstract class SingleValueModelBinderBase<TEntity> : IModelBinder where TEntity : class
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

            var firstValue = valueProviderResult.FirstValue;

            if (string.IsNullOrEmpty(firstValue))
            {
                return Task.CompletedTask;
            }

            if (TryCreateObjectFromString(firstValue, out var result))
            {
                bindingContext.Result = ModelBindingResult.Success(result);
            }
            else
            {
                bindingContext.Result = ModelBindingResult.Failed();
            }

            return Task.CompletedTask;
        }

        protected abstract bool TryCreateObjectFromString(string value, [NotNullWhen(returnValue: true)] out TEntity? entity);
    }
}
