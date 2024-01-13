using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FribergCarRentals.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
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
                name: "Cars",
                columns: table => new
                {
                    CarId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelYear = table.Column<int>(type: "int", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RentalStatusCarRentalStatusId = table.Column<int>(type: "int", nullable: false),
                    PropulsionSystemVehiclePropulsionId = table.Column<int>(type: "int", nullable: false)
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
                        principalColumn: "CarId");
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
                name: "IX_Cars_PropulsionSystemVehiclePropulsionId",
                table: "Cars",
                column: "PropulsionSystemVehiclePropulsionId");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_RentalStatusCarRentalStatusId",
                table: "Cars",
                column: "RentalStatusCarRentalStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_CarEntityCarId",
                table: "Images",
                column: "CarEntityCarId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "CarRentalStatuses");

            migrationBuilder.DropTable(
                name: "VehiclePropulsion");
        }
    }
}
