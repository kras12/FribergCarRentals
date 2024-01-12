using Microsoft.EntityFrameworkCore;
using FribergCarRentals.Models;
using FribergCars.Shared.SharedTypes;
using FribergCars.Shared.SharedClasses;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Data
{
    /// <summary>
    /// The database context for the application.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="options">The options to use.</param>
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// The database set for cars.
        /// </summary>
        public DbSet<CarEntity> Cars { get; set; }

        #endregion

        #region Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VehiclePropulsionEntity>()
                .HasData(
                    Enum.GetValues(typeof(VehiclePropulsionType))
                    .Cast<VehiclePropulsionType>()
                    .Select(x => VehiclePropulsionEntity.CreateSeedObject(x)));

            modelBuilder.Entity<CarRentalStatusEntity>()
                .HasData(
                    Enum.GetValues(typeof(CarRentalStatus))
                    .Cast<CarRentalStatus>()
                    .Select(x => CarRentalStatusEntity.CreateSeedObject(x)));
    }

        #endregion
    }
}