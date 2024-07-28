using System.ComponentModel.DataAnnotations;

namespace MvcRazorPages.Shared.ViewModels.Order
{
    /// <summary>
    /// A DTO class for creating car orders.
    /// </summary>
    public class CreateCarOrderDto
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public CreateCarOrderDto()
        {

        }

        /// <summary>
        /// A constructor. 
        /// </summary>
        /// <param name="carId">The ID of the car for the order.</param>
        /// <param name="pickupDate">The pickup date for the car booking.</param>
        /// <param name="returnDate">The return date for the car booking.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public CreateCarOrderDto(int carId, DateTime pickupDate, DateTime returnDate)
        {
            #region Checks

            if (carId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(carId), $"The value of parameter '{carId}' must be larger than zero.");
            }

            #endregion

            CarId = carId;
            PickupDateLocalTime = pickupDate;
            ReturnDateLocalTime = returnDate;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The ID of the car for the order.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The value must be a positive number larger than 1.")]
        public int CarId { get; set; }

        /// <summary>
        /// The pickup date in local time.
        /// </summary>
        [Required]
        public DateTime PickupDateLocalTime { get; set; }

        /// <summary>
        /// The return date in local time.
        /// </summary>
        [Required]
        public DateTime ReturnDateLocalTime { get; set; }

        #endregion
    }
}
