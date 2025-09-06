using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForParts.Migrations
{
    /// <inheritdoc />
    public partial class updateAccessory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "color",
                table: "Accessory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "serie",
                table: "Accessory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "typeAccesory",
                table: "Accessory",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "color",
                table: "Accessory");

            migrationBuilder.DropColumn(
                name: "serie",
                table: "Accessory");

            migrationBuilder.DropColumn(
                name: "typeAccesory",
                table: "Accessory");
        }
    }
}
