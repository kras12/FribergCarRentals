using Microsoft.EntityFrameworkCore;
using FribergCarRentals.Models;
using FribergCars.Shared.SharedTypes;
using FribergCars.Shared.SharedClasses;
using System.ComponentModel.DataAnnotations;
using FribergCarRentals.DataAccess.EntityClasses;
using Microsoft.Identity.Client;

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

        /// <summary>
        /// The database set for propulsion systems.
        /// </summary>
        public DbSet<VehiclePropulsionEntity> PropulsionSystems { get; set; }  

        /// <summary>
        /// The database set for car rental status.
        /// </summary>
        public DbSet<CarRentalStatusEntity> CarRentalStatuses { get; set; }

        public DbSet<AdminEntity> Admins { get; set; }

        public DbSet<CustomerEntity> Customers { get; set; }

        public DbSet<CarOrderEntity> CarOrders { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Configures the database model.
        /// </summary>
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

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            // Expliclity set the precision. The default is 18,2 but we choose to give a little more precision on the decimal side. 
            // We will still be able to handle huge numbers. 
            configurationBuilder.Properties<decimal>()
                .HavePrecision(18, 4);
        }

        #endregion
    }
}