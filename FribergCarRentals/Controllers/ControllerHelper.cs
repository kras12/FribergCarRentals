using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.Controllers
{
    internal static class ControllerHelper
    {
        public static string GetControllerNameUnsuffixed<T>() where T : Controller
        {
            return typeof(T).Name.Replace("Controller", "", StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
