using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FribergCarRentals.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Renameduserpasswordfield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HashedPassword",
                table: "Customers",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "HashedPassword",
                table: "Admins",
                newName: "Password");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Customers",
                newName: "HashedPassword");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Admins",
                newName: "HashedPassword");
        }
    }
}
