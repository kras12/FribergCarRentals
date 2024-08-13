using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace FribergCarRentals.Shared.Helpers
{
    /// <summary>
    /// A helper class to manage attributes.
    /// </summary>
    public static class AttributeHelper
    {
        #region Methods

        /// <summary>
        /// Returns the value of a display attribute on a property if such attribute exists.
        /// </summary>
        /// <param name="expression">The expression for the property.</param>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <returns>A <see cref="string"/> that contains the value if the attribute exists, or the name of the property if it doesn't.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static string GetDisplayName<TProperty>(Expression<Func<TProperty>> expression)
        {
            var memberExpression = (MemberExpression)expression.Body;
            var value = memberExpression.Member.GetCustomAttribute(typeof(DisplayNameAttribute)) as DisplayNameAttribute;
            return value?.DisplayName ?? memberExpression.Member.Name ?? throw new ArgumentException("Invalid expression", nameof(expression));
        }

        #endregion
    }
}
