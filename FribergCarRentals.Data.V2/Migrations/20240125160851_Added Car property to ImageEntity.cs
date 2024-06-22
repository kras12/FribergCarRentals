using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FribergCarRentals.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedCarpropertytoImageEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Cars_CarEntityCarId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_CarEntityCarId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "CarEntityCarId",
                table: "Images");

            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "Images",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "CarRentalStatuses",
                keyColumn: "CarRentalStatusId",
                keyValue: 1,
                column: "StatusDescription",
                value: "No status");

            migrationBuilder.UpdateData(
                table: "CarRentalStatuses",
                keyColumn: "CarRentalStatusId",
                keyValue: 2,
                column: "StatusDescription",
                value: "Available for renting");

            migrationBuilder.UpdateData(
                table: "CarRentalStatuses",
                keyColumn: "CarRentalStatusId",
                keyValue: 3,
                column: "StatusDescription",
                value: "Pending pickup");

            migrationBuilder.UpdateData(
                table: "CarRentalStatuses",
                keyColumn: "CarRentalStatusId",
                keyValue: 4,
                column: "StatusDescription",
                value: "Has been picked up");

            migrationBuilder.UpdateData(
                table: "CarRentalStatuses",
                keyColumn: "CarRentalStatusId",
                keyValue: 5,
                column: "StatusDescription",
                value: "Has been returned");

            migrationBuilder.UpdateData(
                table: "CarRentalStatuses",
                keyColumn: "CarRentalStatusId",
                keyValue: 6,
                column: "StatusDescription",
                value: "Temporarily out of service");

            migrationBuilder.UpdateData(
                table: "CarRentalStatuses",
                keyColumn: "CarRentalStatusId",
                keyValue: 7,
                column: "StatusDescription",
                value: "Permanently out of service");

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "OrderStatusId",
                keyValue: 1,
                column: "StatusDescription",
                value: "No order status");

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "OrderStatusId",
                keyValue: 2,
                column: "StatusDescription",
                value: "Order is created");

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "OrderStatusId",
                keyValue: 3,
                column: "StatusDescription",
                value: "Order is completed");

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "OrderStatusId",
                keyValue: 4,
                column: "StatusDescription",
                value: "Order is canceled");

            migrationBuilder.UpdateData(
                table: "VehiclePropulsion",
                keyColumn: "VehiclePropulsionId",
                keyValue: 1,
                column: "PropulsionDescription",
                value: "No propulsion system");

            migrationBuilder.UpdateData(
                table: "VehiclePropulsion",
                keyColumn: "VehiclePropulsionId",
                keyValue: 2,
                column: "PropulsionDescription",
                value: "Other type of vehicle");

            migrationBuilder.UpdateData(
                table: "VehiclePropulsion",
                keyColumn: "VehiclePropulsionId",
                keyValue: 3,
                column: "PropulsionDescription",
                value: "Battery electric vehicle");

            migrationBuilder.UpdateData(
                table: "VehiclePropulsion",
                keyColumn: "VehiclePropulsionId",
                keyValue: 4,
                column: "PropulsionDescription",
                value: "Diesel powered vehicle");

            migrationBuilder.UpdateData(
                table: "VehiclePropulsion",
                keyColumn: "VehiclePropulsionId",
                keyValue: 5,
                column: "PropulsionDescription",
                value: "Gasoline powered vehicle");

            migrationBuilder.UpdateData(
                table: "VehiclePropulsion",
                keyColumn: "VehiclePropulsionId",
                keyValue: 7,
                column: "PropulsionDescription",
                value: "Plugin-in hybrid electric vehicle");

            migrationBuilder.UpdateData(
                table: "VehiclePropulsion",
                keyColumn: "VehiclePropulsionId",
                keyValue: 9,
                column: "PropulsionDescription",
                value: "Hybrid electric vehicle");

            migrationBuilder.CreateIndex(
                name: "IX_Images_CarId",
                table: "Images",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Cars_CarId",
                table: "Images",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "CarId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Cars_CarId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_CarId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Images");

            migrationBuilder.AddColumn<int>(
                name: "CarEntityCarId",
                table: "Images",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CarRentalStatuses",
                keyColumn: "CarRentalStatusId",
                keyValue: 1,
                column: "StatusDescription",
                value: "No status.");

            migrationBuilder.UpdateData(
                table: "CarRentalStatuses",
                keyColumn: "CarRentalStatusId",
                keyValue: 2,
                column: "StatusDescription",
                value: "Available for renting.");

            migrationBuilder.UpdateData(
                table: "CarRentalStatuses",
                keyColumn: "CarRentalStatusId",
                keyValue: 3,
                column: "StatusDescription",
                value: "Pending pickup.");

            migrationBuilder.UpdateData(
                table: "CarRentalStatuses",
                keyColumn: "CarRentalStatusId",
                keyValue: 4,
                column: "StatusDescription",
                value: "Has been picked up.");

            migrationBuilder.UpdateData(
                table: "CarRentalStatuses",
                keyColumn: "CarRentalStatusId",
                keyValue: 5,
                column: "StatusDescription",
                value: "Has been returned.");

            migrationBuilder.UpdateData(
                table: "CarRentalStatuses",
                keyColumn: "CarRentalStatusId",
                keyValue: 6,
                column: "StatusDescription",
                value: "Temporarily out of service.");

            migrationBuilder.UpdateData(
                table: "CarRentalStatuses",
                keyColumn: "CarRentalStatusId",
                keyValue: 7,
                column: "StatusDescription",
                value: "Permanently out of service.");

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "OrderStatusId",
                keyValue: 1,
                column: "StatusDescription",
                value: "No order status.");

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "OrderStatusId",
                keyValue: 2,
                column: "StatusDescription",
                value: "Order is created.");

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "OrderStatusId",
                keyValue: 3,
                column: "StatusDescription",
                value: "Order is completed.");

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "OrderStatusId",
                keyValue: 4,
                column: "StatusDescription",
                value: "Order is canceled.");

            migrationBuilder.UpdateData(
                table: "VehiclePropulsion",
                keyColumn: "VehiclePropulsionId",
                keyValue: 1,
                column: "PropulsionDescription",
                value: "No propulsion system.");

            migrationBuilder.UpdateData(
                table: "VehiclePropulsion",
                keyColumn: "VehiclePropulsionId",
                keyValue: 2,
                column: "PropulsionDescription",
                value: "Other type of vehicle.");

            migrationBuilder.UpdateData(
                table: "VehiclePropulsion",
                keyColumn: "VehiclePropulsionId",
                keyValue: 3,
                column: "PropulsionDescription",
                value: "Battery electric vehicle.");

            migrationBuilder.UpdateData(
                table: "VehiclePropulsion",
                keyColumn: "VehiclePropulsionId",
                keyValue: 4,
                column: "PropulsionDescription",
                value: "Diesel powered vehicle.");

            migrationBuilder.UpdateData(
                table: "VehiclePropulsion",
                keyColumn: "VehiclePropulsionId",
                keyValue: 5,
                column: "PropulsionDescription",
                value: "Gasoline powered vehicle.");

            migrationBuilder.UpdateData(
                table: "VehiclePropulsion",
                keyColumn: "VehiclePropulsionId",
                keyValue: 7,
                column: "PropulsionDescription",
                value: "Plugin-in hybrid electric vehicle.");

            migrationBuilder.UpdateData(
                table: "VehiclePropulsion",
                keyColumn: "VehiclePropulsionId",
                keyValue: 9,
                column: "PropulsionDescription",
                value: "Hybrid electric vehicle.");

            migrationBuilder.CreateIndex(
                name: "IX_Images_CarEntityCarId",
                table: "Images",
                column: "CarEntityCarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Cars_CarEntityCarId",
                table: "Images",
                column: "CarEntityCarId",
                principalTable: "Cars",
                principalColumn: "CarId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
