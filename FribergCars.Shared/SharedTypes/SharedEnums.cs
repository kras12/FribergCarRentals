using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace FribergCars.Shared.SharedTypes
{
	#region Enums

	public enum CarPropulsionSystem
	{
        None,
        BEV, // Battery electric vehicles
        Diesel,
        Gasoline,
        HEV, // Hybrid electric vehicles		
        Other,
        PHEV, // Plugin-in hybrid electric vehicles      
	}

    public enum CarRentalStatus
    {
        Active,
    }

	#endregion
}



//If you use EF Core 6, you can do such trick. For instance, you have an enum CurrencyId like this:

//public enum CurrencyId
//{
//    USD = -1,
//    AMD = -2,
//    EUR = -3,
//    RUB = -4,
//    CAD = -5,
//    AUD = -6,
//}

//And you want to use it in your domain EF class that will be mapped on table. Like this:

//public sealed class Currency
//{
//    [Key]
//    public CurrencyId CurrencyId { get; set; }
//    public string Name { get; set; }
//}

//In your DatabaseContext you can override your OnModelCreating method and write there this data seeding declaration:

//modelBuilder
//        .Entity<Currency>().HasData(
//            Enum.GetValues(typeof(CurrencyId))
//                .Cast<CurrencyId>()
//                .Select(e => new Currency
//                {
//                    CurrencyId = e,
//                    Name = e.ToString()
//                })
//        );

//Then your data in database will look like this:

//CurrencyId Name
//-6	   AUD
//-5	   CAD
//-4	   RUB
//-3	   EUR
//-2	   AMD
//-1	   USD
