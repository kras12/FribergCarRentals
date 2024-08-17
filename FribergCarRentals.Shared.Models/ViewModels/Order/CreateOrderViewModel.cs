using FribergCarRentals.Shared.Constants;
using FribergCarRentals.Shared.Models.ViewModels.Car;
using FribergCarRentals.Shared.Models.ViewModels.Other;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Shared.Models.ViewModels.Order
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
        public CreateOrderViewModel(CarViewModel car, DateTime pickupDate, DateTime returnDate)
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
        public virtual string CarDescription { get; set; } = "";

        /// <summary>
        /// The ID of the car for the order.
        /// </summary>
        [DisplayName("Car ID")]
        [Required(ErrorMessage = ValidationMessages.MandatoryFieldValidationMessage)]
        [Range(1, int.MaxValue, ErrorMessage = "The value must be a positive number larger than 1.")]
        public int CarId { get; set; }

        /// <summary>
        /// The pickup date in local time.
        /// </summary>
        [DisplayName("Pickup Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = ValidationRules.DateFormatString)]
        [Required(ErrorMessage = ValidationMessages.MandatoryFieldValidationMessage)]
        public DateTime PickupDateLocalTime { get; set; }

        /// <summary>
        /// The return date in local time.
        /// </summary>
        [DisplayName("Return Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = ValidationRules.DateFormatString)]
        [Required(ErrorMessage = ValidationMessages.MandatoryFieldValidationMessage)]
        public DateTime ReturnDateLocalTime { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the car description.
        /// </summary>
        /// <param name="carDescription">The new description.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void SetCarDescription(string carDescription)
        {
            #region Checks

            if (string.IsNullOrEmpty(carDescription))
            {
                throw new ArgumentNullException(nameof(carDescription));
            }

            #endregion

            CarDescription = carDescription;
        }

        #endregion
    }
}
