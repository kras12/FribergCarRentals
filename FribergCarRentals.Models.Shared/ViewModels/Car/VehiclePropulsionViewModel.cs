namespace FribergCarRentals.Shared.ViewModels.Car
{
    /// <summary>
    /// A view model class that handles vehicle propulsion.
    /// </summary>
    public class VehiclePropulsionViewModel
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public VehiclePropulsionViewModel()
        {
            
        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="vehiclePropulsionId">The ID for the propulsion system.</param>
        /// <param name="propulsionName">The name for the propulsion system.</param>
        /// <param name="propulsionDescription">The description for the propulsion system.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public VehiclePropulsionViewModel(int vehiclePropulsionId, string propulsionName, string propulsionDescription)
        {
            #region Checks

            if (vehiclePropulsionId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(vehiclePropulsionId), $"The value of parameter '{vehiclePropulsionId}' can't be negative.");
            }

            if (propulsionName is null)
            {
                throw new ArgumentNullException(nameof(propulsionName), $"The value of parameter '{propulsionName}' can't be null.");
            }

            if (propulsionDescription is null)
            {
                throw new ArgumentNullException(nameof(propulsionName), $"The value of parameter '{propulsionDescription}' can't be null.");
            }

            #endregion

            VehiclePropulsionId = vehiclePropulsionId;
            PropulsionName = propulsionName;
            PropulsionDescription = propulsionDescription;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns true if the status is 'None' (no rental status).
        /// </summary>
        public bool IsStatusNone
        {
            get
            {
                return VehiclePropulsionId == (int)VehiclePropulsionType.None;
            }
        }

        /// <summary>
        /// The description for the propulsion system.
        /// </summary>
        public string PropulsionDescription { get; set; } = "";

        /// <summary>
        /// The name for the propulsion system.
        /// </summary>
        public string PropulsionName { get; set; } = "";

        /// <summary>
        /// The ID for the propulsion system.
        /// </summary>
        public int VehiclePropulsionId { get; private set; }

        #endregion
    }
}
