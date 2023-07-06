using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sklepix.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Catgories_CategoryID",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Catgories",
                table: "Catgories");

            migrationBuilder.RenameTable(
                name: "Catgories",
                newName: "Categories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryID",
                table: "Products",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryID",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Catgories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Catgories",
                table: "Catgories",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Catgories_CategoryID",
                table: "Products",
                column: "CategoryID",
                principalTable: "Catgories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
