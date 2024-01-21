using FribergCarRentals.Data.SharedClasses;
using FribergCarRentals.DataAccess.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentals.DataAccess.EntityClasses
{
    /// <summary>
    /// A class that represents an user role.
    /// </summary>
    [Table("UserRoles")]
    public class UserRoleEntity
    {
        #region Constructors

        /// <summary>
        /// A constructor intended for EF Core.
        /// </summary>
        /// <param name="userRoleId">The database ID for the user role.</param>
        /// <param name="userRoleName">The user role name.</param>
        /// <param name="userRoleDescription">The user role description.</param>
        private UserRoleEntity(UserRoleType userRoleId, string userRoleName, string userRoleDescription)
        {
            #region Checks

            if (userRoleName is null)
            {
                throw new ArgumentNullException(nameof(userRoleName), $"The value of parameter '{userRoleName}' can't be null.");
            }

            if (userRoleDescription is null)
            {
                throw new ArgumentNullException(nameof(userRoleDescription), $"The value of parameter '{userRoleDescription}' can't be null.");
            }

            #endregion

            UserRoleId = userRoleId;
            UserRoleName = userRoleName;
            UserRoleDescription = userRoleDescription;
        }

        /// <summary>
        /// A constructor intended for creating the seed data in the database. 
        /// </summary>
        /// <param name="userRoleId">The database ID for the user role.</param>
        /// <exception cref="InvalidOperationException"></exception>
        private UserRoleEntity(UserRoleType userRoleId)
        {
            UserRoleId = userRoleId;
            UserRoleName = userRoleId.GetAttribute<EnumDatabaseValueAttribute>().Value ??
                throw new InvalidOperationException($"The field 'Name' of attribute 'DisplayAttribute' for enum value '{userRoleId}' could not be found.");
            UserRoleDescription = userRoleId.GetAttribute<EnumDatabaseValueAttribute>().DescriptionValue ??
                throw new InvalidOperationException($"The field 'Description' of attribute 'DisplayAttribute' for enum value '{userRoleId}' could not be found.");
        }

        #endregion

        #region Properties

        /// <summary>
        /// The database ID for the user role.
        /// </summary>
        [Key]
        public UserRoleType UserRoleId { get; private set; }

        /// <summary>
        /// The user role type.
        /// </summary>
        public UserRoleType UserRoleType
        {
            get
            {
                return UserRoleId;
            }
        }

        /// <summary>
        /// The user role name.
        /// </summary>
        public string UserRoleName { get; private set; } = "";

        /// <summary>
        /// The user role description.
        /// </summary>
        public string UserRoleDescription { get; private set; } = "";

        #endregion

        #region Methods

        public static UserRoleEntity CreateFromUserRoleName(string userRoleName)
        {
            if (TryCreateFromUserRoleName(userRoleName, out var entity))
            {
                return entity;
            }

            throw new ArgumentException("Invalid user role name.", nameof(userRoleName));
        }

        /// <summary>
        /// Returns a new seed object for inserting into the database.
        /// </summary>
        /// <param name="userRole">The user role.</param>
        /// <returns></returns>
        public static UserRoleEntity CreateSeedObject(UserRoleType userRole)
        {
            return new UserRoleEntity(userRole);
        }

        public static bool TryCreateFromUserRoleName(string userRoleName, [NotNullWhen(true)] out UserRoleEntity? entity)
        {
            #region Checks

            if (string.IsNullOrEmpty(userRoleName))
            {
                throw new ArgumentException($"The value for parameter '{nameof(userRoleName)}' can't be null or empty.", nameof(userRoleName));
            }

            #endregion

            entity = Enum.GetValues(typeof(UserRoleType))
                    .Cast<UserRoleType>()
                    .Where(x => x.GetAttribute<EnumDatabaseValueAttribute>().Value == userRoleName)
                    .Select(x => new UserRoleEntity(x))
                    .SingleOrDefault();

            return entity is not null;
        }

        #endregion
    }
}
