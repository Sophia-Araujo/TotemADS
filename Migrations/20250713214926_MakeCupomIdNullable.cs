using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TotemPWA.Migrations
{
    /// <inheritdoc />
    public partial class MakeCupomIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensCombos_Cupons_CupomId",
                table: "ItensCombos");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensCombos_Cupons_CupomId",
                table: "ItensCombos",
                column: "CupomId",
                principalTable: "Cupons",
                principalColumn: "CupomId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensCombos_Cupons_CupomId",
                table: "ItensCombos");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensCombos_Cupons_CupomId",
                table: "ItensCombos",
                column: "CupomId",
                principalTable: "Cupons",
                principalColumn: "CupomId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
