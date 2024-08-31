using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.Shared.Mvc.Helpers
{
    /// <summary>
    /// Helper class for controllers.
    /// </summary>
    public static class ControllerHelper
    {
        /// <summary>
        /// Returns the controller name minus the "controller" suffix.
        /// </summary>
        /// <typeparam name="T">The type of the controller class.</typeparam>
        /// <returns>The name as a <see cref="string"/>.</returns>
        public static string GetControllerName<T>() where T : ControllerBase
        {
            return typeof(T).Name.Replace("Controller", "", StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
