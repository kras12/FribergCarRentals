using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FribergCarRentals.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Removedorderdetailsfromthecarordertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderDetails",
                table: "CarOrders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderDetails",
                table: "CarOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
