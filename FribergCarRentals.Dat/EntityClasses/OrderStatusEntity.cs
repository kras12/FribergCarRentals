using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FribergCarRentals.DataAccess.Types;
using FribergCarRentals.DataAccess.Attributes;
using FribergCarRentals.DataAccess.Extensions;

namespace FribergCarRentals.DataAccess.EntityClasses
{
    /// <summary>
    /// A class that represents an order status.
    /// </summary>
    [Table("OrderStatuses")]
    public class OrderStatusEntity
    {
        #region Constructors

        /// <summary>
        /// A constructor intended for EF Core.
        /// </summary>
        /// <param name="orderStatusId">The database ID for the entity. Can't be negative.</param>
        /// <param name="statusName">The status name. Can't be null.</param>
        /// <param name="statusDescription">The status description. Can't be null.</param>
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
        /// The database ID for the entity.
        /// </summary>
        [Key]
        public OrderStatus OrderStatusId { get; private set; }

        /// <summary>
        /// The order status type.
        /// </summary>
        public OrderStatus StatusType
        {
            get
            {
                return OrderStatusId;
            }
        }

        /// <summary>
        /// The status name.
        /// </summary>
        public string StatusName { get; private set; } = "";

        /// <summary>
        /// The status description.
        /// </summary>
        public string StatusDescription { get; private set; } = "";

        #endregion

        #region Methods

        /// <summary>
        /// Returns a new seed object for inserting into the database.
        /// </summary>
        /// <param name="orderStatus">The order status.</param>
        /// <returns>A <see cref="OrderStatusEntity"/>.</returns>
        public static OrderStatusEntity CreateSeedObject(OrderStatus orderStatus)
        {
            return new OrderStatusEntity(orderStatus);
        }

        /// <summary>
        /// Attempts to create a new order status entity from an order status.
        /// </summary>
        /// <param name="statusName">The name of the status.</param>
        /// <param name="entity">The resulting <see cref="OrderStatusEntity"/> object if the operation was successful. Null if not.</param>
        /// <returns></returns>
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
