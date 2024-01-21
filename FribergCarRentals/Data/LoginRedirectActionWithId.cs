namespace FribergCarRentals.Data
{
    public class LoginRedirectActionWithId
    {
        #region Constructors

        public LoginRedirectActionWithId()
        {
        }

        public LoginRedirectActionWithId(string action, string controller, RouteValueDictionary? routeValues = null)
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
