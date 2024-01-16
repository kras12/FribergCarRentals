using FribergCars.Shared.SharedClasses;
using FribergCars.Shared.SharedTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentals.DataAccess.EntityClasses
{
    /// <summary>
    /// A class that represents car booking statuses for a car.
    /// </summary>
    [Table("CarBookingStatuses")]
    public class CarBookingStatusEntity
    {
        #region Constructors

        /// <summary>
        /// A constructor intended for EF Core.
        /// </summary>
        /// <param name="carBookingStatusId">The database ID for the entity. Can't be negative.</param>
        /// <param name="statusName">The status name.</param>
        /// <param name="statusDescription">The status description.</param>
        private CarBookingStatusEntity(CarBookingStatus carBookingStatusId, string statusName, string statusDescription)
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

            CarBookingStatusId = carBookingStatusId;
            StatusName = statusName;
            StatusDescription = statusDescription;
        }

        /// <summary>
        /// A constructor intended for creating the seed data in the database. 
        /// </summary>
        /// <param name="bookingStatus">The booking status for the car.</param>
        /// <exception cref="InvalidOperationException"></exception>
        private CarBookingStatusEntity(CarBookingStatus bookingStatus)
        {
            CarBookingStatusId = bookingStatus;
            StatusName = bookingStatus.GetAttribute<EnumDatabaseValueAttribute>().Value ??
                throw new InvalidOperationException($"The field 'Name' of attribute 'DisplayAttribute' for enum value '{bookingStatus}' could not be found.");
            StatusDescription = bookingStatus.GetAttribute<EnumDatabaseValueAttribute>().DescriptionValue ??
                throw new InvalidOperationException($"The field 'Description' of attribute 'DisplayAttribute' for enum value '{bookingStatus}' could not be found.");
        }

        #endregion

        #region Properties

        /// <summary>
        /// The database ID for the entity.
        /// </summary>
        [Key]
        public CarBookingStatus CarBookingStatusId { get; private set; }

        /// <summary>
        /// The status description.
        /// </summary>
        public string StatusDescription { get; private set; } = "";

        /// <summary>
        /// The status name.
        /// </summary>
        public string StatusName { get; private set; } = "";

        /// <summary>
        /// The status type.
        /// </summary>
        public CarBookingStatus StatusType
        {
            get
            {
                return CarBookingStatusId;
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Returns a new seed object for inserting into the database.
        /// </summary>
        /// <param name="bookingStatus"></param>
        /// <returns><see cref="CarBookingStatusEntity"/></returns>
        public static CarBookingStatusEntity CreateSeedObject(CarBookingStatus bookingStatus)
        {
            return new CarBookingStatusEntity(bookingStatus);
        }

        public static bool TryCreateFromStatusName(string statusName, [NotNullWhen(true)] out CarBookingStatusEntity? entity)
        {
            #region Checks

            if (string.IsNullOrEmpty(statusName))
            {
                throw new ArgumentException($"The value for parameter '{nameof(statusName)}' can't be null or empty.", nameof(statusName));
            }

            #endregion

            entity = Enum.GetValues(typeof(CarBookingStatus))
                    .Cast<CarBookingStatus>()
                    .Where(x => x.GetAttribute<EnumDatabaseValueAttribute>().Value == statusName)
                    .Select(x => new CarBookingStatusEntity(x))
                    .SingleOrDefault();

            return entity is not null;
        }

        #endregion
    }
}
