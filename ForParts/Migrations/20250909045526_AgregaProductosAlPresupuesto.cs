using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForParts.Migrations
{
    /// <inheritdoc />
    public partial class AgregaProductosAlPresupuesto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_BudgetedProduct_ProductId",
                table: "Budgets");

            migrationBuilder.DropIndex(
                name: "IX_Budgets_ProductId",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Budgets");

            migrationBuilder.AddColumn<int>(
                name: "BudgetId",
                table: "BudgetedProduct",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetedProduct_BudgetId",
                table: "BudgetedProduct",
                column: "BudgetId");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetedProduct_Budgets_BudgetId",
                table: "BudgetedProduct",
                column: "BudgetId",
                principalTable: "Budgets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetedProduct_Budgets_BudgetId",
                table: "BudgetedProduct");

            migrationBuilder.DropIndex(
                name: "IX_BudgetedProduct_BudgetId",
                table: "BudgetedProduct");

            migrationBuilder.DropColumn(
                name: "BudgetId",
                table: "BudgetedProduct");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Budgets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_ProductId",
                table: "Budgets",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_BudgetedProduct_ProductId",
                table: "Budgets",
                column: "ProductId",
                principalTable: "BudgetedProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
