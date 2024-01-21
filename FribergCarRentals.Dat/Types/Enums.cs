using FribergCarRentals.Data.SharedClasses;
using FribergCarRentals.DataAccess.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FribergCarRentals.DataAccess.Types
{
    #region Enums

    public enum VehiclePropulsionType
    {
        [EnumDatabaseValue("None", DescriptionValue = "No propulsion system.")]
        None = 1,

        [EnumDatabaseValue("Other", DescriptionValue = "Other type of vehicle.")]
        Other = 2,

        [EnumDatabaseValue("BEV", DescriptionValue = "Battery electric vehicle.")]
        BEV = 3,

        [EnumDatabaseValue("Diesel", DescriptionValue = "Diesel powered vehicle.")]
        Diesel = 4,

        [EnumDatabaseValue("Gasoline", DescriptionValue = "Gasoline powered vehicle.")]
        Gasoline = 5,

        [EnumDatabaseValue("HEV", DescriptionValue = "Hybrid electric vehicle.")]
        HEV = 9,

        [EnumDatabaseValue("PHEV", DescriptionValue = "Plugin-in hybrid electric vehicle.")]
        PHEV = 7,
    }

    public enum RentalCarStatus
    {
        [EnumDatabaseValue("None", DescriptionValue = "No status.")]
        None = 1,

        [EnumDatabaseValue("Rentable", DescriptionValue = "Available for renting.")]
        Rentable = 2,

        [EnumDatabaseValue("PendingPickup", DescriptionValue = "Pending pickup.")]
        PendingPickup = 3,

        [EnumDatabaseValue("PickedUp", DescriptionValue = "Has been picked up.")]
        PickedUp = 4,

        [EnumDatabaseValue("Returned", DescriptionValue = "Has been returned.")]
        Returned = 5,

        [EnumDatabaseValue("TemporaryOutOfService", DescriptionValue = "Temporarily out of service.")]
        TemporaryOutOfService = 6,

        [EnumDatabaseValue("PermanentlyOutOfService", DescriptionValue = "Permanently out of service.")]
        PermanentlyOutOfService = 7
    }

    public enum OrderStatus
    {
        [EnumDatabaseValue("None", DescriptionValue = "No order status.")]
        None = 1,

        [EnumDatabaseValue("Created", DescriptionValue = "Order is created.")]
        Created = 2,

        [EnumDatabaseValue("Completed", DescriptionValue = "Order is completed.")]
        Completed = 3,

        [EnumDatabaseValue("Canceled", DescriptionValue = "Order is canceled.")]
        Canceled = 4
    }

    #endregion
}


