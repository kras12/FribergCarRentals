using FribergCarRentals.Shared.Enums;

namespace FribergCarRentals.Shared.Models.ViewModels.Car
{
    /// <summary>
    /// A view model class that handles a car rental status.
    /// </summary>
    public class CarRentalStatusViewModel
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public CarRentalStatusViewModel()
        {

        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="carRentalStatusId">The database ID for the entity.</param>
        /// <param name="statusName">The status name.</param>
        /// <param name="statusDescription">The status description.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public CarRentalStatusViewModel(int carRentalStatusId, string statusName, string statusDescription)
        {
            #region Checks

            if (statusName is null)
            {
                throw new ArgumentNullException(nameof(statusName), $"The value of parameter '{statusName}' can't be null.");
            }

            if (statusDescription is null)
            {
                throw new ArgumentNullException(nameof(statusDescription), $"The value of parameter '{statusDescription}' can't be null.");
            }

            #endregion

            CarRentalStatusId = carRentalStatusId;
            StatusName = statusName;
            StatusDescription = statusDescription;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The ID for the status.
        /// </summary>
        public int CarRentalStatusId { get; set; }

        /// <summary>
        /// Returns true if the status is 'None' (no rental status).
        /// </summary>
        public bool IsStatusNone
        {
            get
            {
                return CarRentalStatusId == (int)RentalCarStatus.None;
            }
        }

        /// <summary>
        /// The description for the status.
        /// </summary>
        public string StatusDescription { get; set; } = "";

        /// <summary>
        /// The name for the status.
        /// </summary>
        public string StatusName { get; set; } = "";

        #endregion
    }
}
