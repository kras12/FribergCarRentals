using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FribergCarRentals.Data.Migrations
{
    /// <inheritdoc />
    public partial class Addedstatusfieldtocarbookings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookingStatusCarBookingStatusId",
                table: "CarBookings",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "CarBookingStatuses",
                columns: table => new
                {
                    CarBookingStatusId = table.Column<int>(type: "int", nullable: false),
                    StatusDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarBookingStatuses", x => x.CarBookingStatusId);
                });

            migrationBuilder.InsertData(
                table: "CarBookingStatuses",
                columns: new[] { "CarBookingStatusId", "StatusDescription", "StatusName" },
                values: new object[,]
                {
                    { 1, "No status.", "None" },
                    { 2, "Pending status.", "Pending" },
                    { 3, "Car is picked up.", "PickedUpCar" },
                    { 4, "Car is returned.", "ReturnedCar" },
                    { 5, "Booking is canceled.", "Canceled" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarBookings_BookingStatusCarBookingStatusId",
                table: "CarBookings",
                column: "BookingStatusCarBookingStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarBookings_CarBookingStatuses_BookingStatusCarBookingStatusId",
                table: "CarBookings",
                column: "BookingStatusCarBookingStatusId",
                principalTable: "CarBookingStatuses",
                principalColumn: "CarBookingStatusId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarBookings_CarBookingStatuses_BookingStatusCarBookingStatusId",
                table: "CarBookings");

            migrationBuilder.DropTable(
                name: "CarBookingStatuses");

            migrationBuilder.DropIndex(
                name: "IX_CarBookings_BookingStatusCarBookingStatusId",
                table: "CarBookings");

            migrationBuilder.DropColumn(
                name: "BookingStatusCarBookingStatusId",
                table: "CarBookings");
        }
    }
}
