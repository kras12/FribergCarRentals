using FribergCars.Shared.SharedTypes;
using System.ComponentModel.DataAnnotations;
using FribergCars.Shared.SharedClasses;
using System.ComponentModel.DataAnnotations.Schema;

namespace FribergCarRentals.Models
{
    /// <summary>
    /// A class that represents rental statuses for a car.
    /// </summary>
    [Table("CarRentalStatuses")]
    public class CarRentalStatusEntity
    {
        #region Constructors

        /// <summary>
        /// A constructor intended for EF Core.
        /// </summary>
        /// <param name="carRentalStatusId">The database ID for the entity. Can't be negative.</param>
        /// <param name="statusName">The status name.</param>
        /// <param name="statusDescription">The status description.</param>
        private CarRentalStatusEntity(int carRentalStatusId, string statusName, string statusDescription)
        {
            #region Checks


            if (carRentalStatusId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(carRentalStatusId), $"The value of parameter '{carRentalStatusId}' can't be negative.");
            }

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
        private CarRentalStatusEntity(CarRentalStatus rentalStatus)
        {
            CarRentalStatusId = (int)rentalStatus;
            StatusName = rentalStatus.GetAttribute<DisplayAttribute>().Name ??
                throw new InvalidOperationException($"The field 'Name' of attribute 'DisplayAttribute' for enum value '{rentalStatus}' could not be found.");
            StatusDescription = rentalStatus.GetAttribute<DisplayAttribute>().Description ??
                throw new InvalidOperationException($"The field 'Description' of attribute 'DisplayAttribute' for enum value '{rentalStatus}' could not be found.");
        }

        #endregion

        #region Properties

        /// <summary>
        /// The database ID for the entity.
        /// </summary>
        [Key]
        public int CarRentalStatusId { get; set; }

        /// <summary>
        /// The status name.
        /// </summary>
        public string StatusName { get; set; } = "";

        /// <summary>
        /// The status description.
        /// </summary>
        public string StatusDescription { get; set; } = "";

        #endregion

        #region Methods

        /// <summary>
        /// Returns a new seed object for inserting into the database.
        /// </summary>
        /// <param name="rentalStatus"></param>
        /// <returns></returns>
        public static CarRentalStatusEntity CreateSeedObject(CarRentalStatus rentalStatus)
        {
            return new CarRentalStatusEntity(rentalStatus);
        }

        #endregion
    }
}
