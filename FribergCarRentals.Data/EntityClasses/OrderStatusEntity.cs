using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using FribergCarRentals.Shared.Enums;
using FribergCarRentals.Shared.Attributes;
using FribergCarRentals.Shared.Extensions;

namespace FribergCarRentals.Data.EntityClasses
{
    /// <summary>
    /// An entity class that represents an order status.
    /// </summary>
    [Table("OrderStatuses")]
    public class OrderStatusEntity
    {
        #region Constructors

        /// <summary>
        /// A constructor intended for EF Core.
        /// </summary>
        /// <param name="orderStatusId">The database ID for the entity.</param>
        /// <param name="statusName">The status name.</param>
        /// <param name="statusDescription">The status description.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        private OrderStatusEntity(OrderStatus orderStatusId, string statusName, string statusDescription)
        {
            #region Checks

            if (orderStatusId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(orderStatusId), $"The value of parameter '{orderStatusId}' can't be negative.");
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

            OrderStatusId = orderStatusId;
            StatusName = statusName;
            StatusDescription = statusDescription;
        }

        /// <summary>
        /// A constructor intended for creating the seed data in the database. 
        /// </summary>
        /// <param name="orderStatus">The order status.</param>
        /// <exception cref="InvalidOperationException"></exception>
        private OrderStatusEntity(OrderStatus orderStatus)
        {
            OrderStatusId = orderStatus;
            StatusName = orderStatus.GetAttribute<EnumDatabaseValueAttribute>().Value ??
                throw new InvalidOperationException($"The field 'Name' of attribute 'DisplayAttribute' for enum value '{orderStatus}' could not be found.");
            StatusDescription = orderStatus.GetAttribute<EnumDatabaseValueAttribute>().DescriptionValue ??
                throw new InvalidOperationException($"The field 'Description' of attribute 'DisplayAttribute' for enum value '{orderStatus}' could not be found.");
        }

        #endregion

        #region Properties

        /// <summary>
        /// The ID for the status.
        /// </summary>
        [Key]
        public OrderStatus OrderStatusId { get; private set; }

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
        public OrderStatus StatusType
        {
            get
            {
                return OrderStatusId;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new entity that represents an entity stored in the database.
        /// </summary>
        /// <param name="orderStatusType">The order status type for the new object.</param>
        /// <returns>An <see cref="OrderStatusEntity"/> object.</returns>
        public static OrderStatusEntity CreateFromType(OrderStatus orderStatusType)
        {
            return new OrderStatusEntity(orderStatusType);
        }

        /// <summary>
        /// Attempts to create a new entity that represents an entity stored in the database.
        /// </summary>
        /// <param name="statusName">The name of the status to match.</param>
        /// <param name="entity">The resulting <see cref="OrderStatusEntity"/> object if the operation was successful.</param>
        /// <returns>True if the operation was successful.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static bool TryCreateFromStatusName(string statusName, [NotNullWhen(true)] out OrderStatusEntity? entity)
        {
            #region Checks

            if (string.IsNullOrEmpty(statusName))
            {
                throw new ArgumentException($"The value for parameter '{nameof(statusName)}' can't be null or empty.", nameof(statusName));
            }

            #endregion

            entity = Enum.GetValues(typeof(OrderStatus))
                    .Cast<OrderStatus>()
                    .Where(x => x.GetAttribute<EnumDatabaseValueAttribute>().Value == statusName)
                    .Select(x => new OrderStatusEntity(x))
                    .SingleOrDefault();

            return entity is not null;
        }

        #endregion
    }
}
