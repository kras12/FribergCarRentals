namespace FribergCarRentals.Data
{
    public class RedirectToAction
    {
        #region Constructors

        public RedirectToAction()
        {
        }

        public RedirectToAction(string action, string controller, RouteValueDictionary? routeValues = null)
        {
            Action = action;
            Controller = controller;
            RouteValues = routeValues;
        }

        #endregion

        #region Properties

        public string Action { get; set; } = "";

        public string Controller { get; set; } = "";

        public RouteValueDictionary? RouteValues { get; set; }

        #endregion
    }
}
