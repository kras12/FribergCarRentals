using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Models
{
    /// <summary>
    /// A class that represents a car order. 
    /// </summary>
    public class CarOrderEntity
    {
        #region Constructors



        #endregion

        #region Properties

        /// <summary>
        /// The order ID.
        /// </summary>
        [Key]
        public int CarOrderId { get; set; }

        /// <summary>
        /// The order date.
        /// </summary>
        public DateTime OrderDate { get; set; }

        public DateTime PickupDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public CarEntity Car {  get; set; } 

        public Decimal RentalCostPerDay { get; set; }

        public CustomerEntity Customer { get; set; }

        public Decimal OrderSum { get; set; }

        public List<PaymentEntity> Payments { get; } = new List<PaymentEntity>();

        public string OrderDetails { get; set; } = "";

        #endregion
    }
}
