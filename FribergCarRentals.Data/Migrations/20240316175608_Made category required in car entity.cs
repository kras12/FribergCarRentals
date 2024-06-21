using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FribergCarRentals.Data.Migrations
{
    /// <inheritdoc />
    public partial class Madecategoryrequiredincarentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarCategories_CategoryCarCategoryId",
                table: "Cars");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryCarCategoryId",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_CarCategories_CategoryCarCategoryId",
                table: "Cars",
                column: "CategoryCarCategoryId",
                principalTable: "CarCategories",
                principalColumn: "CarCategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarCategories_CategoryCarCategoryId",
                table: "Cars");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryCarCategoryId",
                table: "Cars",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_CarCategories_CategoryCarCategoryId",
                table: "Cars",
                column: "CategoryCarCategoryId",
                principalTable: "CarCategories",
                principalColumn: "CarCategoryId");
        }
    }
}
