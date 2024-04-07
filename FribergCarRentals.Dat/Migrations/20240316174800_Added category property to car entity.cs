using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FribergCarRentals.Data.Migrations
{
    /// <inheritdoc />
    public partial class Addedcategorypropertytocarentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryCarCategoryId",
                table: "Cars",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cars_CategoryCarCategoryId",
                table: "Cars",
                column: "CategoryCarCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_CarCategories_CategoryCarCategoryId",
                table: "Cars",
                column: "CategoryCarCategoryId",
                principalTable: "CarCategories",
                principalColumn: "CarCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarCategories_CategoryCarCategoryId",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_Cars_CategoryCarCategoryId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "CategoryCarCategoryId",
                table: "Cars");
        }
    }
}
