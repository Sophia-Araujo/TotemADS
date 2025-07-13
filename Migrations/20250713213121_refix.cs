using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TotemPWA.Migrations
{
    /// <inheritdoc />
    public partial class refix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensCombos_Produto_ProdutoId",
                table: "ItensCombos");

            migrationBuilder.DropIndex(
                name: "IX_ItensCombos_CupomId",
                table: "ItensCombos");

            migrationBuilder.AlterColumn<int>(
                name: "CupomId",
                table: "ItensCombos",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "CupomId1",
                table: "ItensCombos",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProdutoId1",
                table: "ItensCombos",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItensCombos_CupomId",
                table: "ItensCombos",
                column: "CupomId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensCombos_CupomId1",
                table: "ItensCombos",
                column: "CupomId1",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItensCombos_ProdutoId1",
                table: "ItensCombos",
                column: "ProdutoId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensCombos_Cupons_CupomId1",
                table: "ItensCombos",
                column: "CupomId1",
                principalTable: "Cupons",
                principalColumn: "CupomId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensCombos_Produto_ProdutoId",
                table: "ItensCombos",
                column: "ProdutoId",
                principalTable: "Produto",
                principalColumn: "ProdutoId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItensCombos_Produto_ProdutoId1",
                table: "ItensCombos",
                column: "ProdutoId1",
                principalTable: "Produto",
                principalColumn: "ProdutoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensCombos_Cupons_CupomId1",
                table: "ItensCombos");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensCombos_Produto_ProdutoId",
                table: "ItensCombos");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensCombos_Produto_ProdutoId1",
                table: "ItensCombos");

            migrationBuilder.DropIndex(
                name: "IX_ItensCombos_CupomId",
                table: "ItensCombos");

            migrationBuilder.DropIndex(
                name: "IX_ItensCombos_CupomId1",
                table: "ItensCombos");

            migrationBuilder.DropIndex(
                name: "IX_ItensCombos_ProdutoId1",
                table: "ItensCombos");

            migrationBuilder.DropColumn(
                name: "CupomId1",
                table: "ItensCombos");

            migrationBuilder.DropColumn(
                name: "ProdutoId1",
                table: "ItensCombos");

            migrationBuilder.AlterColumn<int>(
                name: "CupomId",
                table: "ItensCombos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItensCombos_CupomId",
                table: "ItensCombos",
                column: "CupomId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ItensCombos_Produto_ProdutoId",
                table: "ItensCombos",
                column: "ProdutoId",
                principalTable: "Produto",
                principalColumn: "ProdutoId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
