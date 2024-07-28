using FribergCarRentals.Shared.Dto.Car;

namespace FribergCarRentals.Shared.Dto.Order
{
    /// <summary>
    /// A DTO class that contains the results of a car rental search.
    /// </summary>
    public class CarRentalSearchResultDto
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public CarRentalSearchResultDto()
        {
            
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="availableCars">A collection of cars available for renting.</param>
        public CarRentalSearchResultDto(List<CarDto> availableCars)
        {
            AvailableCars = availableCars;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Cars available to rent 
        /// </summary>
        public List<CarDto> AvailableCars { get; } = new();        

        #endregion
    }
}
