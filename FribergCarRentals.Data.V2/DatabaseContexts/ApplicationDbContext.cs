using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;
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
        /// The database set for admins.
        /// </summary>
        public DbSet<AdminEntity> Admins { get; set; }

        /// <summary>
        /// The database set for car bookings.
        /// </summary>
        public DbSet<CarBookingEntity> CarBookings { get; set; }

        /// <summary>
        /// The database set for car categories.
        /// </summary>
        public DbSet<CarCategoryEntity> CarCategories { get; set; }

        /// <summary>
        /// The database set for car orders.
        /// </summary>
        public DbSet<CarOrderEntity> CarOrders { get; set; }

        /// <summary>
        /// The database set for car rental statuses.
        /// </summary>
        public DbSet<CarRentalStatusEntity> CarRentalStatuses { get; set; }

        /// <summary>
        /// The database set for cars.
        /// </summary>
        public DbSet<CarEntity> Cars { get; set; }

        /// <summary>
        /// The database set for customers.
        /// </summary>
        public DbSet<CustomerEntity> Customers { get; set; }

        /// <summary>
        /// The database set for car images.
        /// </summary>
        public DbSet<ImageEntity> Images { get; set; }

        /// <summary>
        /// The database set for order statuses.
        /// </summary>
        public DbSet<OrderStatusEntity> OrderStatuses { get; set; }

        /// <summary>
        /// The database set for propulsion systems.
        /// </summary>
        public DbSet<VehiclePropulsionEntity> PropulsionSystems { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Configures the conventions.
        /// </summary>
        /// <param name="configurationBuilder">The configuration builder.</param>
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            // Expliclity set the precision. The default is 18,2 but we choose to give a little more precision on the decimal side. 
            // We will still be able to handle huge numbers. 
            configurationBuilder.Properties<decimal>()
                .HavePrecision(18, 4);
        }

        /// <summary>
        /// Configures the options.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                optionsBuilder.EnableSensitiveDataLogging(sensitiveDataLoggingEnabled: true);
            }
        }

        /// <summary>
        /// Configures the database model.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =====================================
            // AdminEntity
            // =====================================
            modelBuilder.Entity<AdminEntity>()
                .HasIndex(x => x.Email)
                .IsUnique();

            // =====================================
            // CarBookingEntity
            // =====================================

            modelBuilder.Entity<CarBookingEntity>()
                .Navigation(x => x.Car)
                .AutoInclude();

            modelBuilder.Entity<CarBookingEntity>()
                .Navigation(x => x.CarOrder)
                .AutoInclude();

            // =====================================
            // CarEntity
            // =====================================

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
                .Navigation(x => x.Category)
                .AutoInclude();

            modelBuilder.Entity<CarEntity>()
                .HasMany(x => x.Images)
                .WithOne(x => x.Car)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CarEntity>()
                .HasOne(x => x.Category)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            // =====================================
            // CarOrderEntity
            // =====================================

            modelBuilder.Entity<CarOrderEntity>()
                .Navigation(x => x.Customer)
                .AutoInclude();

            modelBuilder.Entity<CarOrderEntity>()
                .Navigation(x => x.CarBookings)
                .AutoInclude();

            modelBuilder.Entity<CarOrderEntity>()
               .Navigation(x => x.OrderStatus)
               .AutoInclude();

            // =====================================
            // CarRentalStatusEntity
            // =====================================

            modelBuilder.Entity<CarRentalStatusEntity>()
                .HasData(
                    Enum.GetValues(typeof(RentalCarStatus))
                    .Cast<RentalCarStatus>()
                    .Select(x => CarRentalStatusEntity.CreateFromType(x)));

            modelBuilder.Entity<CarRentalStatusEntity>()
               .Property(x => x.CarRentalStatusId)
               .HasConversion<int>();

            // =====================================
            // CustomerEntity
            // =====================================

            modelBuilder.Entity<CustomerEntity>()
                .HasIndex(x => x.Email)
                .IsUnique();

            modelBuilder.Entity<CustomerEntity>()
                .Navigation(x => x.Orders)
                .AutoInclude();

            // =====================================
            // OrderStatusEntity
            // =====================================

            modelBuilder.Entity<OrderStatusEntity>()
               .HasData(
                   Enum.GetValues(typeof(OrderStatus))
                   .Cast<OrderStatus>()
                   .Select(x => OrderStatusEntity.CreateFromType(x)));

            modelBuilder.Entity<OrderStatusEntity>()
               .Property(x => x.OrderStatusId)
               .HasConversion<int>();

            // =====================================
            // UserRoleEntity
            // =====================================
            modelBuilder.Entity<UserRoleEntity>()
                .HasData(
                    Enum.GetValues(typeof(UserRoleType))
                    .Cast<UserRoleType>()
                    .Select(x => UserRoleEntity.CreateFromType(x)));

            // =====================================
            // VehiclePropulsionEntity
            // =====================================

            modelBuilder.Entity<VehiclePropulsionEntity>()
                .HasData(
                    Enum.GetValues(typeof(VehiclePropulsionType))
                    .Cast<VehiclePropulsionType>()
                    .Select(x => VehiclePropulsionEntity.CreateFromType(x)));

            modelBuilder.Entity<VehiclePropulsionEntity>()
                .Property(x => x.VehiclePropulsionId)
                .HasConversion<int>();            
        }

        #endregion
    }
}