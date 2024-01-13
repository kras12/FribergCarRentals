using FribergCars.Shared.SharedClasses;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FribergCars.Shared.SharedTypes
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

    public enum CarRentalStatus
    {
        [EnumDatabaseValue("Unavailable", DescriptionValue = "Not available for renting")]
        Unavailable = 1,

        [EnumDatabaseValue("Available", DescriptionValue = "Available for renting")]
        Available = 2,

        [EnumDatabaseValue("Rented", DescriptionValue = "Already rented")]
        Rented = 3,

        [EnumDatabaseValue("OutOfService", DescriptionValue = "Taken out of service permanently")]
        OutOfService = 4
    }

	#endregion
}


