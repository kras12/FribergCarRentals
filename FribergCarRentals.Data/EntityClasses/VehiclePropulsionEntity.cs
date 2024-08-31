using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using FribergCarRentals.Shared.Enums;
using FribergCarRentals.Shared.Attributes;
using FribergCarRentals.Shared.Extensions;

namespace FribergCarRentals.Data.EntityClasses
{
    /// <summary>
    /// An entity class that represents the propulsion system for a vehicle.
    /// </summary>
    [Table("VehiclePropulsion")]
    public class VehiclePropulsionEntity
    {
        #region Constructors

        /// <summary>
        /// A constructor intended for EF Core.
        /// </summary>
        /// <param name="vehiclePropulsionId">The database ID for the entity.</param>
        /// <param name="propulsionName">The name for the propulsion system.</param>
        /// <param name="propulsionDescription">The description for the propulsion system.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        private VehiclePropulsionEntity(VehiclePropulsionType vehiclePropulsionId, string propulsionName, string propulsionDescription)
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

        /// <summary>
        /// A constructor intended for creating the seed data in the database. 
        /// </summary>
        /// <param name="propulsionType">The type of propulsion system.</param>
        /// <exception cref="InvalidOperationException"></exception>
        private VehiclePropulsionEntity(VehiclePropulsionType propulsionType)
        {
            VehiclePropulsionId = propulsionType;
            PropulsionName = propulsionType.GetAttribute<EnumDatabaseValueAttribute>().Value ??
                throw new InvalidOperationException($"The field 'Name' of attribute 'DisplayAttribute' for enum value '{propulsionType}' could not be found.");
            PropulsionDescription = propulsionType.GetAttribute<EnumDatabaseValueAttribute>().DescriptionValue ??
                throw new InvalidOperationException($"The field 'Description' of attribute 'DisplayAttribute' for enum value '{propulsionType}' could not be found.");
        }

        #endregion

        #region Properties

        /// <summary>
        /// The description for the propulsion system.
        /// </summary>
        public string PropulsionDescription { get; private set; }

        /// <summary>
        /// The name for the propulsion system.
        /// </summary>
        public string PropulsionName { get; private set; }

        /// <summary>
        /// The propulsion type.
        /// </summary>
        public VehiclePropulsionType PropulsionType
        {
            get
            {
                return VehiclePropulsionId;
            }
        }

        /// <summary>
        /// The ID for the entity.
        /// </summary>
        [Key]
        public VehiclePropulsionType VehiclePropulsionId { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new entity that represents an entity stored in the database.
        /// </summary>
        /// <param name="propulsionId">The ID of the propulsion type.</param>
        /// <returns>A <see cref="VehiclePropulsionEntity"/> object.</returns>
        public static VehiclePropulsionEntity CreateFromId(int propulsionId)
        {
            return new VehiclePropulsionEntity((VehiclePropulsionType)propulsionId);
        }

        /// <summary>
        /// Creates a new entity that represents an entity stored in the database.
        /// </summary>
        /// <param name="propulsionType">The propulsion type for the new object.</param>
        /// <returns>A <see cref="VehiclePropulsionEntity"/> object.</returns>
        public static VehiclePropulsionEntity CreateFromType(VehiclePropulsionType propulsionType)
        {
            return new VehiclePropulsionEntity(propulsionType);
        }

        /// <summary>
        /// Attempts to create a new entity that represents an entity stored in the database.
        /// </summary>
        /// <param name="propulsionSystemName">The name of the propulsion system to match.</param>
        /// <param name="entity">The resulting <see cref="VehiclePropulsionEntity"/> object if the operation was successful.</param>
        /// <returns>True if the operation was successful.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static bool TryCreateFromPropulsionName(string propulsionSystemName, [NotNullWhen(true)] out VehiclePropulsionEntity? entity)
        {
            #region Checks

            if (string.IsNullOrEmpty(propulsionSystemName))
            {
                throw new ArgumentException($"The value for parameter '{nameof(propulsionSystemName)}' can't be null or empty.", nameof(propulsionSystemName));
            }

            #endregion

            entity = Enum.GetValues(typeof(VehiclePropulsionType))
                    .Cast<VehiclePropulsionType>()
                    .Where(x => x.GetAttribute<EnumDatabaseValueAttribute>().Value == propulsionSystemName)
                    .Select(x => new VehiclePropulsionEntity(x))
                    .SingleOrDefault();

            return entity is not null;
        }

        #endregion
    }
}