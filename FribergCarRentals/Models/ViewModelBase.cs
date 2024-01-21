namespace FribergCarRentals.Data
{
    public class ViewModelBase
    {
        #region Properties

        public bool IsRequestFromAnotherController { get; set; } = false;

        #endregion
    }
}
