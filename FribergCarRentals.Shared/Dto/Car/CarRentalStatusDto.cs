namespace FribergCarRentals.Shared.Dto.Car
{
    /// <summary>
    /// A DTO class that represents rental statuses for a car.
    /// </summary>
    public class CarRentalStatusDto
    {
        #region Properties

        /// <summary>
        /// The ID for the status.
        /// </summary>
        public int CarRentalStatusId { get; private set; }

        /// <summary>
        /// The description for the status.
        /// </summary>
        public string StatusDescription { get; private set; } = "";

        /// <summary>
        /// The name for the status.
        /// </summary>
        public string StatusName { get; private set; } = "";

        #endregion
    }
}
