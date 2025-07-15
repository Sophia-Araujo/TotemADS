using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TotemPWA.Migrations
{
    /// <inheritdoc />
    public partial class CorrecaoCupom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cupons_Administradores_AdministradorId",
                table: "Cupons");

            migrationBuilder.DropForeignKey(
                name: "FK_Cupons_Produto_ProdutoId",
                table: "Cupons");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensCombos_Cupons_CupomId1",
                table: "ItensCombos");

            migrationBuilder.DropIndex(
                name: "IX_ItensCombos_CupomId1",
                table: "ItensCombos");

            migrationBuilder.DropColumn(
                name: "CupomId1",
                table: "ItensCombos");

            migrationBuilder.AlterColumn<int>(
                name: "ProdutoId",
                table: "Cupons",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<decimal>(
                name: "Desconto",
                table: "Cupons",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "REAL");

            migrationBuilder.AddColumn<int>(
                name: "AdministradorId1",
                table: "Cupons",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cupons_AdministradorId1",
                table: "Cupons",
                column: "AdministradorId1");

            migrationBuilder.CreateIndex(
                name: "IX_Cupons_Codigo",
                table: "Cupons",
                column: "Codigo",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cupons_Administradores_AdministradorId",
                table: "Cupons",
                column: "AdministradorId",
                principalTable: "Administradores",
                principalColumn: "AdministradorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cupons_Administradores_AdministradorId1",
                table: "Cupons",
                column: "AdministradorId1",
                principalTable: "Administradores",
                principalColumn: "AdministradorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cupons_Produto_ProdutoId",
                table: "Cupons",
                column: "ProdutoId",
                principalTable: "Produto",
                principalColumn: "ProdutoId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cupons_Administradores_AdministradorId",
                table: "Cupons");

            migrationBuilder.DropForeignKey(
                name: "FK_Cupons_Administradores_AdministradorId1",
                table: "Cupons");

            migrationBuilder.DropForeignKey(
                name: "FK_Cupons_Produto_ProdutoId",
                table: "Cupons");

            migrationBuilder.DropIndex(
                name: "IX_Cupons_AdministradorId1",
                table: "Cupons");

            migrationBuilder.DropIndex(
                name: "IX_Cupons_Codigo",
                table: "Cupons");

            migrationBuilder.DropColumn(
                name: "AdministradorId1",
                table: "Cupons");

            migrationBuilder.AddColumn<int>(
                name: "CupomId1",
                table: "ItensCombos",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProdutoId",
                table: "Cupons",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Desconto",
                table: "Cupons",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.CreateIndex(
                name: "IX_ItensCombos_CupomId1",
                table: "ItensCombos",
                column: "CupomId1",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cupons_Administradores_AdministradorId",
                table: "Cupons",
                column: "AdministradorId",
                principalTable: "Administradores",
                principalColumn: "AdministradorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cupons_Produto_ProdutoId",
                table: "Cupons",
                column: "ProdutoId",
                principalTable: "Produto",
                principalColumn: "ProdutoId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItensCombos_Cupons_CupomId1",
                table: "ItensCombos",
                column: "CupomId1",
                principalTable: "Cupons",
                principalColumn: "CupomId");
        }
    }
}
