using FribergCars.Shared.SharedClasses;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FribergCars.Shared.SharedTypes
{
    #region Enums

    public enum VehiclePropulsionType
	{
        [Display(Name = "None", Description = "No propulsion system.")]
        [EnumDatabaseValue("None", DescriptionValue = "No propulsion system.")]
        None = 1,

        [Display(Name = "Other", Description = "Other type of vehicle.")]
        [EnumDatabaseValue("Other", DescriptionValue = "Other type of vehicle.")]
        Other = 2,

        [Display(Name = "BEV", Description = "Battery electric vehicle.")]
        [EnumDatabaseValue("BEV", DescriptionValue = "Battery electric vehicle.")]
        BEV = 3,

        [Display(Name = "Diesel", Description = "Diesel powered vehicle.")]
        [EnumDatabaseValue("Diesel", DescriptionValue = "Diesel powered vehicle.")]
        Diesel = 4,

        [Display(Name = "Gasoline", Description = "Gasoline powered vehicle.")]
        [EnumDatabaseValue("Gasoline", DescriptionValue = "Gasoline powered vehicle.")]
        Gasoline = 5,

        [Display(Name = "HEV", Description = "Hybrid electric vehicle.")]
        [EnumDatabaseValue("HEV", DescriptionValue = "Hybrid electric vehicle.")]
        HEV = 9,

        [Display(Name = "PHEV", Description = "Plugin-in hybrid electric vehicle.")]
        [EnumDatabaseValue("PHEV", DescriptionValue = "Plugin-in hybrid electric vehicle.")]
        PHEV = 7,
    }

    public enum CarRentalStatus
    {
        [Display(Name = "Unavailable", Description = "Not available for renting")]
        [EnumDatabaseValue("Unavailable", DescriptionValue = "Not available for renting")]
        Unavailable = 1,

        [Display(Name = "Available", Description = "Available for renting")]
        [EnumDatabaseValue("Available", DescriptionValue = "Available for renting")]
        Available = 2,

        [Display(Name = "Rented", Description = "Already rented")]
        [EnumDatabaseValue("Rented", DescriptionValue = "Already rented")]
        Rented = 3,

        [Display(Name = "OutOfService", Description = "Taken out of service permanently")]
        [EnumDatabaseValue("OutOfService", DescriptionValue = "Taken out of service permanently")]
        OutOfService = 4
    }

	#endregion
}


