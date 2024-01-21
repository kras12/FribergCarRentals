using Microsoft.EntityFrameworkCore;
using FribergCarRentals.Data.SharedClasses;
using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Mvc.ModelBinding.Internal;
using FribergCarRentals.DataAccess.EntityClasses;
using FribergCarRentals.DataAccess.Types;

namespace FribergCarRentals.DataAccess.DatabaseContexts
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
        /// The database set for car rental statuses.
        /// </summary>
        public DbSet<CarRentalStatusEntity> CarRentalStatuses { get; set; }

        public DbSet<OrderStatusEntity> OrderStatuses { get; set; }

        public DbSet<AdminEntity> Admins { get; set; }

        public DbSet<CustomerEntity> Customers { get; set; }

        public DbSet<CarOrderEntity> CarOrders { get; set; }

        public DbSet<CarBookingEntity> CarBookings { get; set; }

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

            modelBuilder.Entity<OrderStatusEntity>()
               .HasData(
                   Enum.GetValues(typeof(OrderStatus))
                   .Cast<OrderStatus>()
                   .Select(x => OrderStatusEntity.CreateSeedObject(x)));

            modelBuilder.Entity<OrderStatusEntity>()
               .Property(x => x.OrderStatusId)
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

            modelBuilder.Entity<UserRoleEntity>()
                .HasData(
                    Enum.GetValues(typeof(UserRoleType))
                    .Cast<UserRoleType>()
                    .Select(x => UserRoleEntity.CreateSeedObject(x)));

            modelBuilder.Entity<CarOrderEntity>()
                .Navigation(x => x.Customer)
                .AutoInclude();

            modelBuilder.Entity<CarOrderEntity>()
                .Navigation(x => x.CarBookings)
                .AutoInclude();

            modelBuilder.Entity<CarOrderEntity>()
               .Navigation(x => x.OrderStatus)
               .AutoInclude();

            modelBuilder.Entity<CarBookingEntity>()
                .Navigation(x => x.Car)
                .AutoInclude();

            modelBuilder.Entity<CarBookingEntity>()
                .Navigation(x => x.CarOrder)
                .AutoInclude();
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