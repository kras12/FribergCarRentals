using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FribergCarRentals.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Renamedorderdatecolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderDate",
                table: "CarOrders",
                newName: "OrderDateUtc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderDateUtc",
                table: "CarOrders",
                newName: "OrderDate");
        }
    }
}
