using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TotemPWA.Migrations
{
    /// <inheritdoc />
    public partial class IgredientesEAdicionaisEIgredientesProdutoEupdateProduto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensPedidos_Produtos_ProdutoId",
                table: "ItensPedidos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItensPedidos",
                table: "ItensPedidos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Produtos",
                table: "Produtos");

            migrationBuilder.RenameTable(
                name: "Produtos",
                newName: "Produto");

            migrationBuilder.AddColumn<int>(
                name: "AdministradorId",
                table: "Produto",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItensPedidos",
                table: "ItensPedidos",
                columns: new[] { "ItensPedidoId", "ProdutoId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Produto",
                table: "Produto",
                column: "ProdutoId");

            migrationBuilder.CreateTable(
                name: "Igredientes",
                columns: table => new
                {
                    IgredienteId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Imagem = table.Column<byte[]>(type: "BLOB", nullable: false),
                    Valor = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Igredientes", x => x.IgredienteId);
                });

            migrationBuilder.CreateTable(
                name: "Adicionais",
                columns: table => new
                {
                    AdicionalId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Quantidade = table.Column<int>(type: "INTEGER", nullable: false),
                    IgredienteId = table.Column<int>(type: "INTEGER", nullable: false),
                    ItensPedidoId = table.Column<int>(type: "INTEGER", nullable: false),
                    ItensPedidoId1 = table.Column<int>(type: "INTEGER", nullable: false),
                    ItensPedidoProdutoId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adicionais", x => x.AdicionalId);
                    table.ForeignKey(
                        name: "FK_Adicionais_Igredientes_IgredienteId",
                        column: x => x.IgredienteId,
                        principalTable: "Igredientes",
                        principalColumn: "IgredienteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Adicionais_ItensPedidos_ItensPedidoId1_ItensPedidoProdutoId",
                        columns: x => new { x.ItensPedidoId1, x.ItensPedidoProdutoId },
                        principalTable: "ItensPedidos",
                        principalColumns: new[] { "ItensPedidoId", "ProdutoId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IgredienteProdutos",
                columns: table => new
                {
                    IgredienteId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProdutoId = table.Column<int>(type: "INTEGER", nullable: false),
                    IgredienteProdutoId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantidade = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IgredienteProdutos", x => new { x.ProdutoId, x.IgredienteId });
                    table.ForeignKey(
                        name: "FK_IgredienteProdutos_Igredientes_IgredienteId",
                        column: x => x.IgredienteId,
                        principalTable: "Igredientes",
                        principalColumn: "IgredienteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IgredienteProdutos_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produto",
                        principalColumn: "ProdutoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItensPedidos_PedidoId",
                table: "ItensPedidos",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_Produto_AdministradorId",
                table: "Produto",
                column: "AdministradorId");

            migrationBuilder.CreateIndex(
                name: "IX_Adicionais_IgredienteId",
                table: "Adicionais",
                column: "IgredienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Adicionais_ItensPedidoId1_ItensPedidoProdutoId",
                table: "Adicionais",
                columns: new[] { "ItensPedidoId1", "ItensPedidoProdutoId" });

            migrationBuilder.CreateIndex(
                name: "IX_IgredienteProdutos_IgredienteId",
                table: "IgredienteProdutos",
                column: "IgredienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensPedidos_Produto_ProdutoId",
                table: "ItensPedidos",
                column: "ProdutoId",
                principalTable: "Produto",
                principalColumn: "ProdutoId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Produto_Administradores_AdministradorId",
                table: "Produto",
                column: "AdministradorId",
                principalTable: "Administradores",
                principalColumn: "AdministradorId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensPedidos_Produto_ProdutoId",
                table: "ItensPedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_Produto_Administradores_AdministradorId",
                table: "Produto");

            migrationBuilder.DropTable(
                name: "Adicionais");

            migrationBuilder.DropTable(
                name: "IgredienteProdutos");

            migrationBuilder.DropTable(
                name: "Igredientes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItensPedidos",
                table: "ItensPedidos");

            migrationBuilder.DropIndex(
                name: "IX_ItensPedidos_PedidoId",
                table: "ItensPedidos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Produto",
                table: "Produto");

            migrationBuilder.DropIndex(
                name: "IX_Produto_AdministradorId",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "AdministradorId",
                table: "Produto");

            migrationBuilder.RenameTable(
                name: "Produto",
                newName: "Produtos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItensPedidos",
                table: "ItensPedidos",
                columns: new[] { "PedidoId", "ProdutoId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Produtos",
                table: "Produtos",
                column: "ProdutoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensPedidos_Produtos_ProdutoId",
                table: "ItensPedidos",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "ProdutoId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
