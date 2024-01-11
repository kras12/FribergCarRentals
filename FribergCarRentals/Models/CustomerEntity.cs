namespace FribergCarRentals.Models
{
    public class CustomerEntity
    {
        #region Constructors

        #endregion

        #region Properties



        public List<CarOrderEntity> Orders { get; } = new List<CarOrderEntity>();

        #endregion
    }
}
