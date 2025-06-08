using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TotemPWA.Migrations
{
    /// <inheritdoc />
    public partial class IgredientesEAdicionaisEIgredientesProdutoEupdateProduto_bugfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adicionais_ItensPedidos_ItensPedidoId1_ItensPedidoProdutoId",
                table: "Adicionais");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItensPedidos",
                table: "ItensPedidos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IgredienteProdutos",
                table: "IgredienteProdutos");

            migrationBuilder.DropIndex(
                name: "IX_Adicionais_ItensPedidoId1_ItensPedidoProdutoId",
                table: "Adicionais");

            migrationBuilder.DropColumn(
                name: "ItensPedidoId1",
                table: "Adicionais");

            migrationBuilder.DropColumn(
                name: "ItensPedidoProdutoId",
                table: "Adicionais");

            migrationBuilder.AlterColumn<int>(
                name: "ItensPedidoId",
                table: "ItensPedidos",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "IgredienteProdutoId",
                table: "IgredienteProdutos",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItensPedidos",
                table: "ItensPedidos",
                column: "ItensPedidoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IgredienteProdutos",
                table: "IgredienteProdutos",
                column: "IgredienteProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_IgredienteProdutos_ProdutoId",
                table: "IgredienteProdutos",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_Adicionais_ItensPedidoId",
                table: "Adicionais",
                column: "ItensPedidoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Adicionais_ItensPedidos_ItensPedidoId",
                table: "Adicionais",
                column: "ItensPedidoId",
                principalTable: "ItensPedidos",
                principalColumn: "ItensPedidoId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adicionais_ItensPedidos_ItensPedidoId",
                table: "Adicionais");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItensPedidos",
                table: "ItensPedidos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IgredienteProdutos",
                table: "IgredienteProdutos");

            migrationBuilder.DropIndex(
                name: "IX_IgredienteProdutos_ProdutoId",
                table: "IgredienteProdutos");

            migrationBuilder.DropIndex(
                name: "IX_Adicionais_ItensPedidoId",
                table: "Adicionais");

            migrationBuilder.AlterColumn<int>(
                name: "ItensPedidoId",
                table: "ItensPedidos",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "IgredienteProdutoId",
                table: "IgredienteProdutos",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "ItensPedidoId1",
                table: "Adicionais",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItensPedidoProdutoId",
                table: "Adicionais",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItensPedidos",
                table: "ItensPedidos",
                columns: new[] { "ItensPedidoId", "ProdutoId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_IgredienteProdutos",
                table: "IgredienteProdutos",
                columns: new[] { "ProdutoId", "IgredienteId" });

            migrationBuilder.CreateIndex(
                name: "IX_Adicionais_ItensPedidoId1_ItensPedidoProdutoId",
                table: "Adicionais",
                columns: new[] { "ItensPedidoId1", "ItensPedidoProdutoId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Adicionais_ItensPedidos_ItensPedidoId1_ItensPedidoProdutoId",
                table: "Adicionais",
                columns: new[] { "ItensPedidoId1", "ItensPedidoProdutoId" },
                principalTable: "ItensPedidos",
                principalColumns: new[] { "ItensPedidoId", "ProdutoId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
