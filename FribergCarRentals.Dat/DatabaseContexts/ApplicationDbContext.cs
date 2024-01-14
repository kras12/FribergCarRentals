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

        public DbSet<VehiclePropulsionEntity> PropulsionSystems { get; set; }  

        public DbSet<CarRentalStatusEntity> CarRentalStatuses { get; set; }

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

            modelBuilder.Entity<VehiclePropulsionEntity>()
                .Property(x => x.VehiclePropulsionId)
                .HasConversion<int>();

            modelBuilder.Entity<CarRentalStatusEntity>()
                .HasData(
                    Enum.GetValues(typeof(CarRentalStatus))
                    .Cast<CarRentalStatus>()
                    .Select(x => CarRentalStatusEntity.CreateSeedObject(x)));

            modelBuilder.Entity<CarRentalStatusEntity>()
               .Property(x => x.CarRentalStatusId)
               .HasConversion<int>();

            modelBuilder.Entity<CarEntity>()
                .Navigation(x => x.PropulsionSystem)
                .AutoInclude();

            modelBuilder.Entity<CarEntity>()
                .Navigation(x => x.RentalStatus)
                .AutoInclude();

            modelBuilder.Entity<CarEntity>()
                .Navigation(x => x.Images)
                .AutoInclude();

            modelBuilder.Entity<CarEntity>()
                .HasMany(x => x.Images)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }

        #endregion
    }
}