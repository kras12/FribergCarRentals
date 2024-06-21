using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FribergCarRentals.Data.Migrations
{
    /// <inheritdoc />
    public partial class Modifiedcarrentalstatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CarRentalStatuses",
                keyColumn: "CarRentalStatusId",
                keyValue: 1,
                columns: new[] { "StatusDescription", "StatusName" },
                values: new object[] { "No status.", "None" });

            migrationBuilder.UpdateData(
                table: "CarRentalStatuses",
                keyColumn: "CarRentalStatusId",
                keyValue: 2,
                columns: new[] { "StatusDescription", "StatusName" },
                values: new object[] { "Available for renting.", "Rentable" });

            migrationBuilder.UpdateData(
                table: "CarRentalStatuses",
                keyColumn: "CarRentalStatusId",
                keyValue: 3,
                columns: new[] { "StatusDescription", "StatusName" },
                values: new object[] { "Pending pickup.", "PendingPickup" });

            migrationBuilder.UpdateData(
                table: "CarRentalStatuses",
                keyColumn: "CarRentalStatusId",
                keyValue: 4,
                columns: new[] { "StatusDescription", "StatusName" },
                values: new object[] { "Has been picked up.", "PickedUp" });

            migrationBuilder.InsertData(
                table: "CarRentalStatuses",
                columns: new[] { "CarRentalStatusId", "StatusDescription", "StatusName" },
                values: new object[,]
                {
                    { 5, "Has been returned.", "Returned" },
                    { 6, "Temporarily out of service.", "TemporaryOutOfService" },
                    { 7, "Permanently out of service.", "PermanentlyOutOfService" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CarRentalStatuses",
                keyColumn: "CarRentalStatusId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "CarRentalStatuses",
                keyColumn: "CarRentalStatusId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "CarRentalStatuses",
                keyColumn: "CarRentalStatusId",
                keyValue: 7);

            migrationBuilder.UpdateData(
                table: "CarRentalStatuses",
                keyColumn: "CarRentalStatusId",
                keyValue: 1,
                columns: new[] { "StatusDescription", "StatusName" },
                values: new object[] { "Not available for renting", "Unavailable" });

            migrationBuilder.UpdateData(
                table: "CarRentalStatuses",
                keyColumn: "CarRentalStatusId",
                keyValue: 2,
                columns: new[] { "StatusDescription", "StatusName" },
                values: new object[] { "Available for renting", "Available" });

            migrationBuilder.UpdateData(
                table: "CarRentalStatuses",
                keyColumn: "CarRentalStatusId",
                keyValue: 3,
                columns: new[] { "StatusDescription", "StatusName" },
                values: new object[] { "Already rented", "Rented" });

            migrationBuilder.UpdateData(
                table: "CarRentalStatuses",
                keyColumn: "CarRentalStatusId",
                keyValue: 4,
                columns: new[] { "StatusDescription", "StatusName" },
                values: new object[] { "Taken out of service permanently", "OutOfService" });
        }
    }
}
