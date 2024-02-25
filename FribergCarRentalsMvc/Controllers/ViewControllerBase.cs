using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.Controllers
{
    public class ViewControllerBase : Controller
    {
        #region Methods

        protected bool IsRequestFromAnotherController(string currentControllerRoutePart)
        {
            var refererUri = Request.GetTypedHeaders().Referer ?? throw new InvalidOperationException("Failed to retrieve the referer URI");
            return !refererUri.AbsolutePath.Contains(currentControllerRoutePart);
        }

        #endregion
    }
}
