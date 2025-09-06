using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForParts.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePostMerge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "typeAccesory",
                table: "Accessory",
                newName: "typeAccessory");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "typeAccessory",
                table: "Accessory",
                newName: "typeAccesory");
        }
    }
}
