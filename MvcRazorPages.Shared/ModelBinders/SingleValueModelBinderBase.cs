using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics.CodeAnalysis;

namespace FribergCarRentals.Shared.Mvc.ModelBinders
{
    /// <summary>
    /// A base class for a model binder. 
    /// </summary>
    /// <typeparam name="TEntity">The type of the objects to create.</typeparam>
    public abstract class SingleValueModelBinderBase<TEntity> : IModelBinder where TEntity : class
    {
        #region Methods

        /// <summary>
        /// Attempts to bind a model.
        /// </summary>
        /// <param name="bindingContext">The <see cref="ModelBindingContext"/> to use.</param>
        /// <returns>A <see cref="Task"/> which will complete when the model binding process completes.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            #region Checks

            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            #endregion

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

        /// <summary>
        /// Attempts to create an instance of type <see cref="TEntity"/> from a text string.
        /// </summary>
        /// <param name="value">The text string to convert from.</param>
        /// <param name="entity">The created object if the operation was successful.</param>
        /// <returns>True if the operation was successful.</returns>
        protected abstract bool TryCreateObjectFromString(string value, [NotNullWhen(returnValue: true)] out TEntity? entity);

        #endregion
    }
}
