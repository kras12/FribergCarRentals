namespace FribergCarRentals.Data
{
    public class GoBackButtonData
    {
        #region Constructors
        
        public GoBackButtonData(string action, string controller)
        {
            Action = action;
            Controller = controller;
        }

        #endregion

        #region Properties

        public string Action { get; } = "";

        public string Controller { get; } = "";

        #endregion
    }
}
