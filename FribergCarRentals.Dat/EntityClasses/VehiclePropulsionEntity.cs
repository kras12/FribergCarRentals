using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using FribergCarRentals.DataAccess.Types;
using FribergCarRentals.DataAccess.Attributes;
using FribergCarRentals.DataAccess.Extensions;

namespace FribergCarRentals.DataAccess.EntityClasses
{
    /// <summary>
    /// Represents the propulsion system for a vehicle.
    /// </summary>
    [Table("VehiclePropulsion")]
    public class VehiclePropulsionEntity
    {
        #region Constructors

        /// <summary>
        /// A constructor intended for EF Core.
        /// </summary>
        /// <param name="vehiclePropulsionId">The database ID for the entity.</param>
        /// <param name="propulsionName">The name for the propulsion system. Can't be null.</param>
        /// <param name="propulsionDescription">The description for the propulsion system. Can't be null.</param>
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
        /// The database ID for the entity.
        /// </summary>
        [Key]
        public VehiclePropulsionType VehiclePropulsionId { get; private set; }

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
        /// The name for the propulsion system.
        /// </summary>
        public string PropulsionName { get; private set; }

        /// <summary>
        /// The description for the propulsion system.
        /// </summary>
        public string PropulsionDescription { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a new seed object for inserting into the database.
        /// </summary>
        /// <param name="propulsionType"></param>
        /// <returns></returns>
        public static VehiclePropulsionEntity CreateSeedObject(VehiclePropulsionType propulsionType)
        {
            return new VehiclePropulsionEntity(propulsionType);
        }

        public static bool TryCreateFromPropulsionName(string propulsionName, [NotNullWhen(true)] out VehiclePropulsionEntity? entity)
        {
            #region Checks

            if (string.IsNullOrEmpty(propulsionName))
            {
                throw new ArgumentException($"The value for parameter '{nameof(propulsionName)}' can't be null or empty.", nameof(propulsionName));
            }

            #endregion

            entity = Enum.GetValues(typeof(VehiclePropulsionType))
                    .Cast<VehiclePropulsionType>()
                    .Where(x => x.GetAttribute<EnumDatabaseValueAttribute>().Value == propulsionName)
                    .Select(x => new VehiclePropulsionEntity(x))
                    .SingleOrDefault();

            return entity is not null;
        }

        #endregion
    }
}