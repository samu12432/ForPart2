using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForParts.Migrations
{
    /// <inheritdoc />
    public partial class add_UQ_CodeSupply_ProfileColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "UQ_CodeSupply",
                table: "Supply",
                newName: "IX_Supply_codeSupply");

            migrationBuilder.AlterColumn<string>(
                name: "profileColor",
                table: "Profile",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "CodeSupply",
                table: "Profile",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "UQ_CodeSupply_ProfileColor",
                table: "Profile",
                columns: new[] { "CodeSupply", "profileColor" },
                unique: true,
                filter: "[CodeSupply] IS NOT NULL AND [profileColor] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UQ_CodeSupply_ProfileColor",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "CodeSupply",
                table: "Profile");

            migrationBuilder.RenameIndex(
                name: "IX_Supply_codeSupply",
                table: "Supply",
                newName: "UQ_CodeSupply");

            migrationBuilder.AlterColumn<string>(
                name: "profileColor",
                table: "Profile",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
