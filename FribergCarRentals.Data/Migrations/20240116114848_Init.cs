using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FribergCarRentals.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarRentalStatuses",
                columns: table => new
                {
                    CarRentalStatusId = table.Column<int>(type: "int", nullable: false),
                    StatusName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarRentalStatuses", x => x.CarRentalStatusId);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatuses",
                columns: table => new
                {
                    OrderStatusId = table.Column<int>(type: "int", nullable: false),
                    StatusName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatuses", x => x.OrderStatusId);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserRoleId = table.Column<int>(type: "int", nullable: false),
                    UserRoleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserRoleDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.UserRoleId);
                });

            migrationBuilder.CreateTable(
                name: "VehiclePropulsion",
                columns: table => new
                {
                    VehiclePropulsionId = table.Column<int>(type: "int", nullable: false),
                    PropulsionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PropulsionDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehiclePropulsion", x => x.VehiclePropulsionId);
                });

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserRoleId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HashedPassword = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Admins_UserRoles_UserRoleId",
                        column: x => x.UserRoleId,
                        principalTable: "UserRoles",
                        principalColumn: "UserRoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserRoleId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HashedPassword = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Customers_UserRoles_UserRoleId",
                        column: x => x.UserRoleId,
                        principalTable: "UserRoles",
                        principalColumn: "UserRoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    CarId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelYear = table.Column<int>(type: "int", nullable: false),
                    PropulsionSystemVehiclePropulsionId = table.Column<int>(type: "int", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RentalCostPerDay = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    RentalStatusCarRentalStatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.CarId);
                    table.ForeignKey(
                        name: "FK_Cars_CarRentalStatuses_RentalStatusCarRentalStatusId",
                        column: x => x.RentalStatusCarRentalStatusId,
                        principalTable: "CarRentalStatuses",
                        principalColumn: "CarRentalStatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cars_VehiclePropulsion_PropulsionSystemVehiclePropulsionId",
                        column: x => x.PropulsionSystemVehiclePropulsionId,
                        principalTable: "VehiclePropulsion",
                        principalColumn: "VehiclePropulsionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarOrders",
                columns: table => new
                {
                    CarOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerUserId = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderStatusId = table.Column<int>(type: "int", nullable: false),
                    OrderSum = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarOrders", x => x.CarOrderId);
                    table.ForeignKey(
                        name: "FK_CarOrders_Customers_CustomerUserId",
                        column: x => x.CustomerUserId,
                        principalTable: "Customers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarOrders_OrderStatuses_OrderStatusId",
                        column: x => x.OrderStatusId,
                        principalTable: "OrderStatuses",
                        principalColumn: "OrderStatusId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    ImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CarEntityCarId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.ImageId);
                    table.ForeignKey(
                        name: "FK_Images_Cars_CarEntityCarId",
                        column: x => x.CarEntityCarId,
                        principalTable: "Cars",
                        principalColumn: "CarId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarBookings",
                columns: table => new
                {
                    CarBookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarOrderId = table.Column<int>(type: "int", nullable: false),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    PickupDateUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RentalCostPerDay = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ReturnDateUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarBookings", x => x.CarBookingId);
                    table.ForeignKey(
                        name: "FK_CarBookings_CarOrders_CarOrderId",
                        column: x => x.CarOrderId,
                        principalTable: "CarOrders",
                        principalColumn: "CarOrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarBookings_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "CarId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentEntity",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    OrderCarOrderId = table.Column<int>(type: "int", nullable: false),
                    PaymentDetails = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentEntity", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_PaymentEntity_CarOrders_OrderCarOrderId",
                        column: x => x.OrderCarOrderId,
                        principalTable: "CarOrders",
                        principalColumn: "CarOrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CarRentalStatuses",
                columns: new[] { "CarRentalStatusId", "StatusDescription", "StatusName" },
                values: new object[,]
                {
                    { 1, "Not available for renting", "Unavailable" },
                    { 2, "Available for renting", "Available" },
                    { 3, "Already rented", "Rented" },
                    { 4, "Taken out of service permanently", "OutOfService" }
                });

            migrationBuilder.InsertData(
                table: "OrderStatuses",
                columns: new[] { "OrderStatusId", "StatusDescription", "StatusName" },
                values: new object[,]
                {
                    { 1, "No order status.", "None" },
                    { 2, "Order is created.", "Created" },
                    { 3, "Order is completed.", "Completed" },
                    { 4, "Order is canceled.", "Canceled" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "UserRoleId", "UserRoleDescription", "UserRoleName" },
                values: new object[,]
                {
                    { 0, "No role.", "None" },
                    { 1, "Admin role.", "Admin" },
                    { 2, "Customer role.", "Customer" }
                });

            migrationBuilder.InsertData(
                table: "VehiclePropulsion",
                columns: new[] { "VehiclePropulsionId", "PropulsionDescription", "PropulsionName" },
                values: new object[,]
                {
                    { 1, "No propulsion system.", "None" },
                    { 2, "Other type of vehicle.", "Other" },
                    { 3, "Battery electric vehicle.", "BEV" },
                    { 4, "Diesel powered vehicle.", "Diesel" },
                    { 5, "Gasoline powered vehicle.", "Gasoline" },
                    { 7, "Plugin-in hybrid electric vehicle.", "PHEV" },
                    { 9, "Hybrid electric vehicle.", "HEV" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admins_UserRoleId",
                table: "Admins",
                column: "UserRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CarBookings_CarId",
                table: "CarBookings",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_CarBookings_CarOrderId",
                table: "CarBookings",
                column: "CarOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_CarOrders_CustomerUserId",
                table: "CarOrders",
                column: "CustomerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CarOrders_OrderStatusId",
                table: "CarOrders",
                column: "OrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_PropulsionSystemVehiclePropulsionId",
                table: "Cars",
                column: "PropulsionSystemVehiclePropulsionId");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_RentalStatusCarRentalStatusId",
                table: "Cars",
                column: "RentalStatusCarRentalStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UserRoleId",
                table: "Customers",
                column: "UserRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_CarEntityCarId",
                table: "Images",
                column: "CarEntityCarId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentEntity_OrderCarOrderId",
                table: "PaymentEntity",
                column: "OrderCarOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "CarBookings");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "PaymentEntity");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "CarOrders");

            migrationBuilder.DropTable(
                name: "CarRentalStatuses");

            migrationBuilder.DropTable(
                name: "VehiclePropulsion");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "OrderStatuses");

            migrationBuilder.DropTable(
                name: "UserRoles");
        }
    }
}
