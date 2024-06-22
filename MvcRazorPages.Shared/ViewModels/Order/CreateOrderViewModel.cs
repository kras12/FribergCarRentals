using FribergCarRentals.Data.EntityClasses;
using MvcRazorPages.Shared.ViewModels.Other;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcRazorPages.Shared.ViewModels.Order
{
    /// <summary>
    /// A view model class that handles data related to order creation. 
    /// </summary>
    public class CreateOrderViewModel : ViewModelBase
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public CreateOrderViewModel()
        {

        }

        /// <summary>
        /// A constructor. 
        /// </summary>
        /// <param name="car">The car for the order.</param>
        /// <param name="pickupDate">The pickup date for the car booking.</param>
        /// <param name="returnDate">The return date for the car booking.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public CreateOrderViewModel(CarEntity car, DateTime pickupDate, DateTime returnDate)
        {
            #region Checks

            if (car is null)
            {
                throw new ArgumentNullException(nameof(car), $"The value of parameter '{car}' can't be null.");
            }

            #endregion

            CarId = car.CarId;
            CarDescription = $"{car.Brand} {car.Model} {car.ModelYear}";
            PickupDateLocalTime = pickupDate;
            ReturnDateLocalTime = returnDate;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Description of the car.
        /// </summary>
        [DisplayName("Car")]
        [Required]
        public string CarDescription { get; set;  } = "";

        /// <summary>
        /// The ID of the car for the order.
        /// </summary>
        [DisplayName("Car ID")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The value must be a positive number larger than 1.")]
        public int CarId { get; set; }

        /// <summary>
        /// The pickup date in local time.
        /// </summary>
        [DisplayName("Pickup Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = DateFormatString)]
        [Required]
        public DateTime PickupDateLocalTime { get; set; }

        /// <summary>
        /// The return date in local time.
        /// </summary>
        [DisplayName("Return Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = DateFormatString)]
        [Required]
        public DateTime ReturnDateLocalTime { get; set; }

        #endregion
    }
}
