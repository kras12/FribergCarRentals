using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using FribergCarRentals.Shared.Types.Attributes;
using FribergCarRentals.Shared.Types.Enums;
using FribergCarRentals.Shared.Types.Extensions;

namespace FribergCarRentals.Data.EntityClasses
{
	/// <summary>
	/// An entity class that represents rental statuses for a car.
	/// </summary>
	[Table("CarRentalStatuses")]
    public class CarRentalStatusEntity
    {
        #region Constructors

        /// <summary>
        /// A constructor intended for EF Core.
        /// </summary>
        /// <param name="carRentalStatusId">The database ID for the entity.</param>
        /// <param name="statusName">The status name.</param>
        /// <param name="statusDescription">The status description.</param>
        /// <exception cref="ArgumentNullException"></exception>
        private CarRentalStatusEntity(RentalCarStatus carRentalStatusId, string statusName, string statusDescription)
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
        /// A constructor intended for creating the seed data in the database. 
        /// </summary>
        /// <param name="rentalStatus">The rental status for the car.</param>
        /// <exception cref="InvalidOperationException"></exception>
        private CarRentalStatusEntity(RentalCarStatus rentalStatus)
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
        [Key]
        public RentalCarStatus CarRentalStatusId { get; private set; }

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

        #region Methods

        /// <summary>
        /// Creates a new entity that represents an entity stored in the database.
        /// </summary>
        /// <param name="rentalStatusId">The ID of the rental status type.</param>
        /// <returns>A <see cref="VehiclePropulsionEntity"/> object.</returns>
        public static CarRentalStatusEntity CreateFromId(int rentalStatusId)
        {
            return new CarRentalStatusEntity((RentalCarStatus)rentalStatusId);
        }

        /// <summary>
        /// Creates a new entity that represents an entity stored in the database.
        /// </summary>
        /// <param name="rentalStatusType">The rental status type for the new object.</param>
        /// <returns>A <see cref="CarRentalStatusEntity"/> object.</returns>
        public static CarRentalStatusEntity CreateFromType(RentalCarStatus rentalStatusType)
        {
            return new CarRentalStatusEntity(rentalStatusType);
        }

        /// <summary>
        /// Attempts to create a new entity that represents an entity stored in the database.
        /// </summary>
        /// <param name="statusName">The name of the status to match.</param>
        /// <param name="entity">The resulting <see cref="CarRentalStatusEntity"/> object if the operation was successful.</param>
        /// <returns>True if the operation was successful.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static bool TryCreateFromStatusName(string statusName, [NotNullWhen(true)] out CarRentalStatusEntity? entity)
        {
            #region Checks

            if (string.IsNullOrEmpty(statusName))
            {
                throw new ArgumentException($"The value for parameter '{nameof(statusName)}' can't be null or empty.", nameof(statusName));
            }

            #endregion

            entity = Enum.GetValues(typeof(RentalCarStatus))
                    .Cast<RentalCarStatus>()
                    .Where(x => x.GetAttribute<EnumDatabaseValueAttribute>().Value == statusName)
                    .Select(x => new CarRentalStatusEntity(x))
                    .SingleOrDefault();

            return entity is not null;
        }

        #endregion
    }
}
