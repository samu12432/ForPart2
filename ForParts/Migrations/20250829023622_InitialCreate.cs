using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForParts.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Identificador = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoDocumento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DireccionFiscal_Calle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DireccionFiscal_Numero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DireccionFiscal_Ciudad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DireccionFiscal_Departamento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DireccionFiscal_CodigoPostal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DireccionFiscal_Pais = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "ProductMovements",
                columns: table => new
                {
                    MovementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeProduct = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovementType = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovementDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductMovements", x => x.MovementId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    productId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    codeProduct = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    productName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    productDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    productCategory = table.Column<int>(type: "int", nullable: false),
                    productPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    imageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StockActual = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.productId);
                });

            migrationBuilder.CreateTable(
                name: "StockMovements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeSupply = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuantityChange = table.Column<int>(type: "int", nullable: false),
                    MovementDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MovementType = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockMovements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Supply",
                columns: table => new
                {
                    idSupply = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    codeSupply = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    nameSupply = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    descriptionSupply = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    imageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nameSupplier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    priceSupply = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    isEnabledSupply = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supply", x => x.idSupply);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userEmail = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    phoneNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    userName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    passwordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    passwordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    IsEmailConfirmed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    InvoiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceDateCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InvoiceCompany = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvoiceBranchOfCompanyId = table.Column<int>(type: "int", nullable: false),
                    InvoiceCurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoCambio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InvoiceDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvoiceExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InvoiceState = table.Column<int>(type: "int", nullable: false),
                    FechaEmision = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ZureoRespuesta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.InvoiceId);
                    table.ForeignKey(
                        name: "FK_Invoices_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Accessory",
                columns: table => new
                {
                    idSupply = table.Column<int>(type: "int", nullable: false),
                    descriptionAccessory = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accessory", x => x.idSupply);
                    table.ForeignKey(
                        name: "FK_Accessory_Supply_idSupply",
                        column: x => x.idSupply,
                        principalTable: "Supply",
                        principalColumn: "idSupply",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Glass",
                columns: table => new
                {
                    idSupply = table.Column<int>(type: "int", nullable: false),
                    glassThickness = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    glassLength = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    glassWidth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    glassType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Glass", x => x.idSupply);
                    table.ForeignKey(
                        name: "FK_Glass_Supply_idSupply",
                        column: x => x.idSupply,
                        principalTable: "Supply",
                        principalColumn: "idSupply",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Profile",
                columns: table => new
                {
                    idSupply = table.Column<int>(type: "int", nullable: false),
                    profileWeigth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    profileHeigth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    weigthMetro = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    profileColor = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profile", x => x.idSupply);
                    table.ForeignKey(
                        name: "FK_Profile_Supply_idSupply",
                        column: x => x.idSupply,
                        principalTable: "Supply",
                        principalColumn: "idSupply",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stock",
                columns: table => new
                {
                    idStock = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    codeSupply = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SupplyId = table.Column<int>(type: "int", nullable: false),
                    stockQuantity = table.Column<int>(type: "int", nullable: false),
                    stockCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    stockUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.idStock);
                    table.ForeignKey(
                        name: "FK_Stock_Supply_SupplyId",
                        column: x => x.SupplyId,
                        principalTable: "Supply",
                        principalColumn: "idSupply",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupplyNecessary",
                columns: table => new
                {
                    idSupplyNecessary = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    supplyId = table.Column<int>(type: "int", nullable: false),
                    productId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    SupplyidSupply = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplyNecessary", x => x.idSupplyNecessary);
                    table.ForeignKey(
                        name: "FK_SupplyNecessary_Products_productId",
                        column: x => x.productId,
                        principalTable: "Products",
                        principalColumn: "productId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupplyNecessary_Supply_SupplyidSupply",
                        column: x => x.SupplyidSupply,
                        principalTable: "Supply",
                        principalColumn: "idSupply");
                    table.ForeignKey(
                        name: "FK_SupplyNecessary_Supply_supplyId",
                        column: x => x.supplyId,
                        principalTable: "Supply",
                        principalColumn: "idSupply",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    ProductCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    productId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceItems_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "InvoiceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceItems_Products_productId",
                        column: x => x.productId,
                        principalTable: "Products",
                        principalColumn: "productId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_InvoiceId",
                table: "InvoiceItems",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_productId",
                table: "InvoiceItems",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CustomerId",
                table: "Invoices",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_SupplyId",
                table: "Stock",
                column: "SupplyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_CodeSupply",
                table: "Stock",
                column: "codeSupply",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_CodeSupply",
                table: "Supply",
                column: "codeSupply",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupplyNecessary_productId",
                table: "SupplyNecessary",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyNecessary_supplyId",
                table: "SupplyNecessary",
                column: "supplyId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyNecessary_SupplyidSupply",
                table: "SupplyNecessary",
                column: "SupplyidSupply");

            migrationBuilder.CreateIndex(
                name: "IX_Users_phoneNumber",
                table: "Users",
                column: "phoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_UserName",
                table: "Users",
                column: "userName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Users_Active_Email",
                table: "Users",
                column: "userEmail",
                unique: true,
                filter: "[IsEmailConfirmed] = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accessory");

            migrationBuilder.DropTable(
                name: "Glass");

            migrationBuilder.DropTable(
                name: "InvoiceItems");

            migrationBuilder.DropTable(
                name: "ProductMovements");

            migrationBuilder.DropTable(
                name: "Profile");

            migrationBuilder.DropTable(
                name: "Stock");

            migrationBuilder.DropTable(
                name: "StockMovements");

            migrationBuilder.DropTable(
                name: "SupplyNecessary");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Supply");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
