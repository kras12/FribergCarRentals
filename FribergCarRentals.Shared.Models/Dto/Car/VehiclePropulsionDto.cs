namespace FribergCarRentals.Shared.Models.Dto.Car
{
    /// <summary>
    /// A DTO class that represents the propulsion system for a vehicle.
    /// </summary>
    public class VehiclePropulsionDto
    {
        #region Properties

        /// <summary>
        /// The description for the propulsion system.
        /// </summary>
        public string PropulsionDescription { get; set; } = "";

        /// <summary>
        /// The name for the propulsion system.
        /// </summary>
        public string PropulsionName { get; set; } = "";

        /// <summary>
        /// The ID for the entity.
        /// </summary>
        public int VehiclePropulsionId { get; set; }

        #endregion
    }
}