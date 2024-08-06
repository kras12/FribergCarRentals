namespace FribergCarRentals.Shared.ViewModels.Car
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
        public CarRentalStatusViewModel(RentalCarStatus carRentalStatusId, string statusName, string statusDescription)
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

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="rentalStatus">The rental status for the car.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public CarRentalStatusViewModel(RentalCarStatus rentalStatus)
        {
            CarRentalStatusId = rentalStatus;
            StatusName = rentalStatus.GetAttribute<EnumDatabaseValueAttribute>().Value ??
                throw new InvalidOperationException($"The field 'Name' of attribute 'DisplayAttribute' for enum value '{rentalStatus}' could not be found.");
            StatusDescription = rentalStatus.GetAttribute<EnumDatabaseValueAttribute>().DescriptionValue ??
                throw new InvalidOperationException($"The field 'Description' of attribute 'DisplayAttribute' for enum value '{rentalStatus}' could not be found.");
        }

        #endregion

        #region Properties

        /// <summary>
        /// The ID for the status.
        /// </summary>
        public int CarRentalStatusId { get; private set; }

        /// <summary>
        /// Returns true if the status is 'None' (no rental status).
        /// </summary>
        public bool IsStatusNone
        {
            get
            {
                return CarRentalStatusId == (int)RentalCarStatusType.None;
            }
        }

        /// <summary>
        /// The description for the status.
        /// </summary>
        public string StatusDescription { get; private set; } = "";

        /// <summary>
        /// The name for the status.
        /// </summary>
        public string StatusName { get; private set; } = "";

        /// <summary>
        /// The type for the status.
        /// </summary>
        public RentalCarStatus StatusType
        {
            get
            {
                return CarRentalStatusId;
            }
        }

        #endregion
    }
}
